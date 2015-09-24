#include <avr/io.h>
#include "motors.h"

//TODO:
//* ports -> defines

unsigned char motorState[2];

void motors_initMotors(void)
{
	motorState[MOTORS_SIDE_LEFT] = MOTORS_STATE_STOP;
	motorState[MOTORS_SIDE_RIGHT] = MOTORS_STATE_STOP;
}

void motors_changeMotorState(unsigned char side, unsigned char state)
{
	motorState[side] = state;
}

unsigned char motors_getMotorState(unsigned char side)
{
	return motorState[side];
}

void motors_updatePins(void)
{	
	// unsigned char pin0, pin1;
	// getMotorDriverPins(motors_getMotorState(MOTORS_SIDE_LEFT), &pin0, &pin1);

	// unsigned char pin2, pin3;
	// getMotorDriverPins(motors_getMotorState(MOTORS_SIDE_RIGHT), &pin2, &pin3);

	// PORTB = (pin0 << 0) | (pin1 << 1) | (pin2 << 2) | (pin3 << 3);

	PORTB = (motors_getMotorState(MOTORS_SIDE_LEFT) << 0) | (motors_getMotorState(MOTORS_SIDE_RIGHT) << 2);
}