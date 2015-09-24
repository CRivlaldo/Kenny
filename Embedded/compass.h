#ifndef COMPASS_H
#define COMPASS_H

void compass_enable(void);
void compass_update(void);
float compass_getX(void);
float compass_getY(void);
float compass_getZ(void);

#endif