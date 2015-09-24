#include "accelerometer.h"
#include "i2c.h"

float accelerometer_x = 0;
float accelerometer_y = 0;
float accelerometer_z = 0;

void accelerometer_enable(void)
{
	i2c_start(); 
	i2c_writeByte(0x30); // write acc
	i2c_writeByte(0x20); // CTRL_REG1_A
	i2c_writeByte(0x27); // normal power mode, 50 Hz data rate, all axes enabled
	i2c_stop();
}

void accelerometer_update(void)
{
	// printStatus();//F8 
	i2c_start();
	// printStatus();//08 
	i2c_writeByte(0x30); // write acc
	// printStatus();//18
	i2c_writeByte(0xa8); // OUT_X_L_A, MSB set to enable auto-increment
	// printStatus();//28  
	i2c_start();		  // repeated start
	// printStatus();//10
	i2c_writeByte(0x31); // read acc
	// printStatus(); //40 
	unsigned char axl = i2c_readByte();
	unsigned char axh = i2c_readByte();
	unsigned char ayl = i2c_readByte();
	unsigned char ayh = i2c_readByte();
	unsigned char azl = i2c_readByte();
	unsigned char azh = i2c_readLastByte();
	i2c_stop();
	// printStatus();//F8

	accelerometer_x = axh << 8 | axl;
	accelerometer_y = ayh << 8 | ayl;
	accelerometer_z = azh << 8 | azl;
}

float accelerometer_getX(void)
{
	return accelerometer_x;
}

float accelerometer_getY(void)
{
	return accelerometer_y;
}

float accelerometer_getZ(void)
{
	return accelerometer_z;
}