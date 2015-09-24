#include "gyroscope.h"
#include "i2c.h"

//TODO
//* Callibration

float gyroscope_x = 0;
float gyroscope_y = 0;
float gyroscope_z = 0;

// void printStatus(void)//!!!!vladimir: debug
// {
// 	uart_sendByte(i2c_readStatus());
// }

void gyroscope_enable(void)
{
	// printStatus();
	i2c_writeRegister(GYROSCOPE_ADDRESS, CTRL_REG1, 0x0F);//delay(10);  //try 8F
	// printStatus();
    i2c_writeRegister(GYROSCOPE_ADDRESS, CTRL_REG2, 0x00);//delay(10);
	// printStatus();
    i2c_writeRegister(GYROSCOPE_ADDRESS, CTRL_REG3, 0x00);//delay(10);
	// printStatus();
    //i2c_writeRegister(GYROSCOPE_ADDRESS, CTRL_REG4, 0x00);//delay(10); //<< 250dps (works)
    //I2C_write(L3G4200D_Address,CTRL_REG4,0x10);delay(10); //<< 500dps
    i2c_writeRegister(GYROSCOPE_ADDRESS, CTRL_REG4, 0x20);//delay(10); //<< 2000dps
	// printStatus();
    i2c_writeRegister(GYROSCOPE_ADDRESS, CTRL_REG5, 0x00);//delay(10);
	// printStatus();
}

void gyroscope_update(void)
{
	// i2c_start(); 
	// i2c_writeByte((GYROSCOPE_ADDRESS << 1) | 0);
	// i2c_writeByte(OUT_X_L);
	// i2c_start();		  
	// i2c_writeByte((GYROSCOPE_ADDRESS << 1) | 1); 
	// unsigned char gxl = i2c_readByte();
	// unsigned char gxh = i2c_readByte();
	// unsigned char gyl = i2c_readByte();
	// unsigned char gyh = i2c_readByte();
	// unsigned char gzl = i2c_readByte();
	// unsigned char gzh = i2c_readLastByte();
	// i2c_stop();

	// printStatus();
	unsigned char gxl = i2c_readRegister(GYROSCOPE_ADDRESS, OUT_X_L);
	// printStatus();
	unsigned char gxh = i2c_readRegister(GYROSCOPE_ADDRESS, OUT_X_H);
	// printStatus();

	unsigned char gyl = i2c_readRegister(GYROSCOPE_ADDRESS, OUT_Y_L);
	// printStatus();
	unsigned char gyh = i2c_readRegister(GYROSCOPE_ADDRESS, OUT_Y_H);
	// printStatus();

	unsigned char gzl = i2c_readRegister(GYROSCOPE_ADDRESS, OUT_Z_L);
	// printStatus();
	unsigned char gzh = i2c_readRegister(GYROSCOPE_ADDRESS, OUT_Z_H);
	// printStatus();

	gyroscope_x = gxh << 8 | gxl;
	gyroscope_y = gyh << 8 | gyl;
	gyroscope_z = gzh << 8 | gzl;
}

float gyroscope_getX(void)
{
	return gyroscope_x;
}

float gyroscope_getY(void)
{
	return gyroscope_y;
}

float gyroscope_getZ(void)
{
	return gyroscope_z;
}