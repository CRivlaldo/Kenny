#include "compass.h"
#include "i2c.h"

//TODO
//* Callibration

float compass_x = 0;
float compass_y = 0;
float compass_z = 0;

void compass_enable(void)
{
	i2c_start(); 
	i2c_writeByte(0x3C); // write mag
	i2c_writeByte(0x02); // MR_REG_M
	i2c_writeByte(0x00); // continuous conversion mode
	i2c_stop();
}

void compass_update(void)
{
	i2c_start(); 
	i2c_writeByte(0x3C); // write mag
	i2c_writeByte(0x03); // OUTXH_M
	i2c_start();		  // repeated start
	i2c_writeByte(0x3D); // read mag
	unsigned char mxh = i2c_readByte();
	unsigned char mxl = i2c_readByte();
	unsigned char myh = i2c_readByte();
	unsigned char myl = i2c_readByte();
	unsigned char mzh = i2c_readByte();
	unsigned char mzl = i2c_readLastByte();
	i2c_stop();

	compass_x = mxh << 8 | mxl;
	compass_y = myh << 8 | myl;
	compass_z = mzh << 8 | mzl;
}

float compass_getX(void)
{
	return compass_x;
}

float compass_getY(void)
{
	return compass_y;
}

float compass_getZ(void)
{
	return compass_z;
}