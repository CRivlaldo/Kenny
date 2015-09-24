#include <avr/io.h>
#include <avr/interrupt.h>
#include "timers.h"

volatile long timer0Overflow;
volatile unsigned char timer0_mutex = 0;//!!!!debug
unsigned long timer0IntervalOverflows = 0;
onTimerTickDelegate onTimer0Tick;

unsigned short timer1Prescaler = 0;

void onTimerO(void)
{
	if(timer0_mutex)
		return;

	timer0_mutex = 1;
	// //!!!!debug
	// {
	// 	unsigned char state = motors_getMotorState(MOTORS_SIDE_LEFT);
	// 	state = (state + 1) % 3;
	// 	motors_changeMotorState(MOTORS_SIDE_LEFT, state);
	// }

	onTimer0Tick();

	timer0_mutex = 0;
}

ISR(TIMER0_OVF_vect)
{
	timer0Overflow++;
	if(timer0Overflow >= 122 /*timer0IntervalOverflows*/)
	{
		timer0Overflow = 0;
		onTimerO();
	}
}

void timers_initTimer0(unsigned long intervalInMilliseconds, onTimerTickDelegate onTimerTick)
{	
	timers_stopTimer0();

	TCCR0 |= (1 << CS02); //1/256 prescaler
	TCNT0 = 0;
	TIMSK |= (1 << TOIE0); //interruption by overflow

	timer0Overflow = 0;
	timer0IntervalOverflows = intervalInMilliseconds * TIMER0_OVERFLOW_IN_MILLISECOND;
	onTimer0Tick = onTimerTick;
}

void timers_stopTimer0(void)
{
	TCCR0 &= ~((1 << CS02) | (1 << CS01) | (1 << CS00));
}

void timers_initTimer1(unsigned short prescaler)
{
	timer1Prescaler = prescaler;
	timers_stopTimer1();

	switch(prescaler)
	{
		case 8:
			TCCR1B |= (0 << CS12) | (1 << CS11) | (0 << CS10);
			break;
		case 64:
			TCCR1B |= (0 << CS12) | (1 << CS11) | (1 << CS10);
			break;
		case 256:
			TCCR1B |= (1 << CS12) | (0 << CS11) | (0 << CS10);
			break;
		case 1024:
			TCCR1B |= (1 << CS12) | (0 << CS11) | (1 << CS10);
			break;
		default:
			TCCR1B |= (0 << CS12) | (0 << CS11) | (1 << CS10);
	}
}

void timers_stopTimer1(void)
{
	TCCR1B &= ~((1 << CS12) | (1 << CS11) | (1 << CS10));
}

unsigned long timers_getTimer1ElapsedTimeInMicroseconds(void)
{
	long counter = TCNT1;
	return TIMER1_TICKS_IN_MICROSECOND * timer1Prescaler * counter;
}

unsigned short timers_getTimer1Count(void)
{
	return TCNT1;
}

void timers_timer1Reset(void)
{
	TCNT1 = 0;
}