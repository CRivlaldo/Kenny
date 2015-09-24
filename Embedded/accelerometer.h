#ifndef ACCELEROMETER_H
#define ACCELEROMETER_H

void accelerometer_enable(void);
void accelerometer_update(void);
float accelerometer_getX(void);
float accelerometer_getY(void);
float accelerometer_getZ(void);

#endif