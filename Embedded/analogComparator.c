#include <avr/io.h>
#include <util/delay.h>  
#include "analogComparator.h"
#include "mergeSort.h"

float adcBuffer[NUMBER_OF_MEASUREMENTS];

void setChannel(unsigned char port)
{
	//ADMUX = (1 << REFS0) | (1 << ADLAR) | (1 << MUX4) | (1 << MUX3) | (port << 0); // ADCx - ADC2 (x = 0..1)
	ADMUX = (1 << REFS0) | (1 << MUX4) | (1 << MUX3) | (port << 0); // ADCx - ADC2 (x = 0..1)
	_delay_us(20);
}

void startConversion(void)
{
	ADCSRA |= (1 << ADSC); //start AD
}

void waitForConversionEnd(void)
{
	while(!(ADCSRA & (1<<ADIF)));
}

float getConvertedValue(void)
{
	return (AD_VALUE_TO_VOLTAGE_SCALER * (float) ADCW) - ADC2_VOLTAGE;
}

void resetADState(void)
{
	ADCSRA |= (1<<ADIF);
}

float getResultFilteredByMedian(void)
{	
	mergeSort_sortFloatArray(adcBuffer, NUMBER_OF_MEASUREMENTS);

	unsigned char middle = NUMBER_OF_MEASUREMENTS / 2;

	if(NUMBER_OF_MEASUREMENTS % 2 == 0)
		return (adcBuffer[middle - 1] + adcBuffer[middle]) / 2;
	else
		return adcBuffer[middle];
}

void analogComparator_init(void)
{
	//!!!!выставить ADIE

	SFIOR = 0x00;

	//ADMUX = 0x00;  //port
	ADCSRA = (1 << ADEN) | (1 << ADIF) | (1 << ADPS2) | (1 << ADPS1) | (0 << ADPS0); //AD enabled, free-running mode, scaler = 64
}

float analogComparator_getValue(unsigned char port)
{
	//!!!!кондер на опорное обязательно

	setChannel(port);

	for(unsigned char i = 0; i < NUMBER_OF_MEASUREMENTS; i++)
	{
		startConversion();
		waitForConversionEnd();

		adcBuffer[i] = getConvertedValue();

		resetADState();
	}

	return getResultFilteredByMedian();
}