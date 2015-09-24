#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>  
#include "uartManager.h"
#include "messaging.h"
#include "motors.h"
#include "ultraSonic.h"
#include "timers.h"
#include "compass.h"
#include "accelerometer.h"
#include "gyroscope.h"
 
void updateSensors(void)
{
	//vladimir: who freezes???
	//compass_update();
	//accelerometer_update();
	//gyroscope_update();
	ultraSonic_update();
}

int main(void)
{       
	PORTB = 0x00;
	DDRB = 255;

	motors_initMotors();
	ultraSonic_init();
	uart_initReceiver();
	analogComparator_init();
	compass_enable();
	accelerometer_enable();
	gyroscope_enable();
 
	sei();

	timers_initTimer0(1000, updateSensors);

	uart_sendByte(4);//!!!!vladimir: debug
	uart_sendByte(8);//!!!!vladimir: debug
	uart_sendByte(15);//!!!!vladimir: debug
	uart_sendByte(16);//!!!!vladimir: debug
	uart_sendByte(23);//!!!!vladimir: debug
	uart_sendByte(42);//!!!!vladimir: debug
	uart_sendByte(0);//!!!!vladimir: debug

	while (1)
	{
		motors_updatePins();
	}

	return 0;
}