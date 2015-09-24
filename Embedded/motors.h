#ifndef MOTORS_H
#define MOTORS_H

#define MOTORS_SIDE_LEFT 0
#define MOTORS_SIDE_RIGHT 1

#define MOTORS_STATE_STOP 0
#define MOTORS_STATE_FORWARD 1
#define MOTORS_STATE_BACKWARD 2

void motors_initMotors(void);
void motors_changeMotorState(unsigned char side, unsigned char state);
unsigned char motors_getMotorState(unsigned char side);
void motors_updatePins(void);

#endif