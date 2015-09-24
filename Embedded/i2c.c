#include <avr/io.h>
#include "i2c.h"
//#include "uartManager.h"//!!!!vladimir: debug

// void printStatus(void)//!!!!vladimir: debug
// {
// 	uart_sendByte(i2c_readStatus());
// }

void i2c_start(void) 
{
	TWCR = (1 << TWINT) | (1 << TWSTA) | (1 << TWEN); // send start condition  
	while (!(TWCR & (1 << TWINT)));  
}

void i2c_writeByte(unsigned char byte) 
{
	TWDR = byte;              
	TWCR = (1 << TWINT) | (1 << TWEN); // start address transmission  
	while (!(TWCR & (1 << TWINT)));  
}

unsigned char i2c_readByte(void) 
{
	TWCR = (1 << TWINT) | (1 << TWEA) | (1 << TWEN); // start data reception, transmit ACK  
	while (!(TWCR & (1 << TWINT)));  
	return TWDR;  
}

unsigned char i2c_readLastByte(void)
{
	TWCR = (1 << TWINT) | (1 << TWEN); // start data reception
	while (!(TWCR & (1 << TWINT)));  
	return TWDR;  
}

void i2c_stop(void)
{
	TWCR = (1 << TWINT) | (1 << TWSTO) | (1 << TWEN); // send stop condition  
}

void i2c_writeRegister(unsigned char deviceAddress, unsigned char registerAddress, unsigned char byte)
{
	i2c_start();
	i2c_writeByte((deviceAddress << 1) | 0);
	i2c_writeByte(registerAddress);
	i2c_writeByte(byte);
	i2c_stop();
}

unsigned char i2c_readRegister(unsigned char deviceAddress, unsigned char registerAddress)
{
	// printStatus();//F8  
	i2c_start();
	// printStatus();//08 
	i2c_writeByte((deviceAddress << 1) | 0);
	// printStatus(); //20 
	i2c_writeByte(registerAddress);
	// printStatus();//30 
	i2c_start();
	// printStatus();//10 
	i2c_writeByte((deviceAddress << 1) | 1);
	// printStatus();//48
	unsigned char byte = i2c_readLastByte();
	// printStatus();//
	i2c_stop();
	// printStatus();//

	return byte;
}

unsigned char i2c_readStatus()
{ 
    return TWSR & 0xF8;
}