#ifndef ULTRASONIC_H
#define ULTRASONIC_H

#define MAX_ULTRASONIC_SENSORS 1

#define TRIG_PORT PORTD
#define TRIG_DDR DDRD

#define ECHO_PIN PIND
#define ECHO_DDR DDRD

#define SENSOR0_TRIG_BIT 3
#define SENSOR0_ECHO_BIT 2

#define INTERRUPTION INT0

#define INFINITE_DISTANCE 65535

#define ERROR_TIMEOUT 2500 /*   20 * F_CPU / 1000 / 64   */

#define ELAPSED_TICKS_TO_MILLIMETERS_FACTOR 40 / 29  /*   1000000.0 / F_CPU * 64 * 10 / 58   */

void ultraSonic_init(void);
void ultraSonic_update(void);
unsigned short ultraSonic_getDistance(unsigned short sensorIndex);

#endif