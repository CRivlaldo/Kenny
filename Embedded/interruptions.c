#include <avr/io.h>
#include <avr/interrupt.h>
#include "interruptions.h"

onInterruptionDelegate onINT0Interruption = 0;

ISR(INT0_vect)
{
	if(onINT0Interruption != 0)
		onINT0Interruption();
}

void interruptions_waitForRisingEdge(void)
{
	MCUCR &= ~((1 << ISC01) | (1 << ISC00)); //reset MCURC
	MCUCR |= ((1 << ISC01) | (1 << ISC00)); //rising edge
}

void interruptions_waitForFailingEdge(void)
{
	MCUCR &= ~((1 << ISC01) | (1 << ISC00)); //reset MCURC
	MCUCR |= ((1 << ISC01) | (0 << ISC00)); //failing edge
}

void interruptions_waitAnyChange(void)
{
	MCUCR &= ~((1 << ISC01) | (1 << ISC00)); //reset MCURC
	MCUCR |= ((0 << ISC01) | (1 << ISC00)); // any change
}

void interruptions_enable(unsigned char interruption)
{
	GICR |= (1 << interruption); //enable INTx
}

void interruptions_disable(unsigned char interruption)
{
	GICR &= ~(1 << interruption); //disable INTx
}

void interruptions_setOnINT0InterruptionDelegate(onInterruptionDelegate func)
{
	onINT0Interruption = func;
}