#ifndef I2C_H
#define I2C_H

void i2c_start(void);
void i2c_writeByte(unsigned char byte);
unsigned char i2c_readByte(void);
unsigned char i2c_readLastByte(void); 
void i2c_stop(void);

void i2c_writeRegister(unsigned char deviceAddress, unsigned char registerAddress, unsigned char byte);
unsigned char i2c_readRegister(unsigned char deviceAddress, unsigned char registerAddress);

unsigned char i2c_readStatus(void);

void printStatus(void);//!!!!vladimir: debug

#endif