#include <avr/io.h>
#include <util/delay.h>  
#include "ultraSonic.h"
#include "timers.h"
#include "interruptions.h"

//ATTENTION! In current moment this module is using only one ultrasonic sensor.

//TODO:
//ports -> defines
//more than one sensor
//* interruptions in separate module
//error check (infinite loop)

unsigned short distances[MAX_ULTRASONIC_SENSORS];

volatile unsigned char isMeasurementFinished;
volatile unsigned char error;

volatile unsigned short elapsedTicksWhenEchoStarted = 0;
volatile unsigned short elapsedTicksWhenEchoFinished = 0;

unsigned short getDistanceByTimerCount(void)
{
	unsigned long deltaTicks = elapsedTicksWhenEchoFinished - elapsedTicksWhenEchoStarted;
	unsigned long distance = deltaTicks * ELAPSED_TICKS_TO_MILLIMETERS_FACTOR;
	return (unsigned short)distance;
}

void onEchoPinChanged(void)
{
	if(ECHO_PIN & (1 << SENSOR0_ECHO_BIT)) //rising edge
	{
		elapsedTicksWhenEchoStarted = timers_getTimer1Count();
	}
	else //failing edge
	{
		elapsedTicksWhenEchoFinished = timers_getTimer1Count();
		isMeasurementFinished = 1;

		distances[0] = getDistanceByTimerCount();
	}
}

void startMeasurement(unsigned char pin)
{
	TRIG_PORT	 |= (1 << pin);
	_delay_us(10);
	TRIG_PORT	 &= ~(1 << pin);
}

void waitForResult(void)
{
	return;//!!!!vladimir: doesn't work yet

	while(!isMeasurementFinished && !error)//!!!vladimir: this loop disables interruptions
	{
		 error = (timers_getTimer1Count() > ERROR_TIMEOUT);
		 //_delay_us(1000);
	}
}

void ultraSonic_update(void)
{	
	error = 0;
	isMeasurementFinished = 0;

	timers_timer1Reset();

	startMeasurement(SENSOR0_TRIG_BIT);

	waitForResult();//!!!

	if(error)
		distances[0] = INFINITE_DISTANCE;
}

void ultraSonic_init(void)
{
	isMeasurementFinished = 0;
	error = 0;

	for(unsigned short i = 0; i < MAX_ULTRASONIC_SENSORS; i++)
		distances[i] = 0;

	TRIG_DDR |= (1 << SENSOR0_TRIG_BIT);
	ECHO_DDR &= ~(1 << SENSOR0_ECHO_BIT);
	//PORTD &= ~((1 << SENSOR0_TRIG_PIN) | (1 << SENSOR0_ECHO_PIN));
	TRIG_PORT &= ~(1 << SENSOR0_TRIG_BIT);

	interruptions_waitAnyChange();
	interruptions_enable(INTERRUPTION);
	interruptions_setOnINT0InterruptionDelegate(onEchoPinChanged);

	timers_initTimer1(64);
}

unsigned short ultraSonic_getDistance(unsigned short sensorIndex)
{
	return distances[sensorIndex];
}