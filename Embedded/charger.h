#ifndef CHARGER_H
#define CHARGER_H

#define BATTERY_PORT 0
#define CHARGER_PORT 1

float charger_getBatteryVoltage(void);
float charger_getChargerVoltage(void);

#endif