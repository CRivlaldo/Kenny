#ifndef ANALOG_COMPARATOR_H
#define ANALOG_COMPARATOR_H

#define ADC2_VOLTAGE 5.0f
#define AD_VALUE_TO_VOLTAGE_SCALER 0.009766f /*   5 / 512   */
// #define AD_VALUE_TO_VOLTAGE_SCALER 0.0048828f /*   5 / 1024   */
#define NUMBER_OF_MEASUREMENTS 10

void analogComparator_init(void);
float analogComparator_getValue(unsigned char port);

#endif