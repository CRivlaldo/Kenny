#include "charger.h"
#include "analogComparator.h"

float charger_getBatteryVoltage(void)
{
	return analogComparator_getValue(BATTERY_PORT);
}

float charger_getChargerVoltage(void)
{
	return analogComparator_getValue(CHARGER_PORT);
}