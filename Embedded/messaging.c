#include "messaging.h"
#include "uartManager.h"
#include "crc16.h"
#include "motors.h"
#include "ultraSonic.h"
#include "compass.h"
#include "accelerometer.h"
#include "gyroscope.h"
#include "charger.h"

unsigned char answerBuffer[255];

void sendAnswer(unsigned short length)
{
	unsigned short checkSum = getCRC16CheckSum(answerBuffer, length);
	answerBuffer[length] = (unsigned char)(checkSum % 256);
	answerBuffer[length + 1] = (unsigned char)(checkSum / 256);
	uart_sendBuffer(answerBuffer, length + 2);
}

void sendOK(void)
{
	answerBuffer[0] = 0;
	sendAnswer(1);
}

void testChannel(void)
{
	sendOK();
}

void sendUnsignedShort(unsigned short value)
{
	answerBuffer[0] = (unsigned char)(value / 256);
	answerBuffer[1] = (unsigned char)(value % 256);	
	sendAnswer(2);
}

void getValueFromUltrasonicSensor(unsigned char sensorIndex)
{
	sendUnsignedShort(ultraSonic_getDistance(sensorIndex));
	// long integerPart = (long) value;
	// answerBuffer[0] = (unsigned char)(integerPart / 256);
	// answerBuffer[1] = (unsigned char)(integerPart % 256);

	// double fractionalValue = value - (double)integerPart;
	// fractionalValue *= 65536;
	// long fractionalPart = (long)value;
	// answerBuffer[2] = (unsigned char)(fractionalPart / 256);
	// answerBuffer[3] = (unsigned char)(fractionalPart % 256);

	//sendAnswer(4);
}

void writeFloatToBuffer(float value, unsigned short offset)
{
	// long* pTemp = (long*) &value;

	// answerBuffer[offset + 0] = (((*pTemp) <<  0) >> 24);
	// answerBuffer[offset + 1] = (((*pTemp) <<  8) >> 24);
	// answerBuffer[offset + 2] = (((*pTemp) << 16) >> 24);
	// answerBuffer[offset + 3] = (((*pTemp) << 24) >> 24);

	unsigned char* pByte = (unsigned char*)&value;
	for(unsigned char i = 0; i < 4; i++)
	{
		answerBuffer[offset + i] = *pByte;
		pByte++;
	}
}

void getValuesFromMagnetometer(void)
{
	writeFloatToBuffer(compass_getX(), 0);
	writeFloatToBuffer(compass_getY(), 4);
	writeFloatToBuffer(compass_getZ(), 8);

	sendAnswer(12);
}

void getValuesFromAccelerometer(void)
{
	writeFloatToBuffer(accelerometer_getX(), 0);
	writeFloatToBuffer(accelerometer_getY(), 4);
	writeFloatToBuffer(accelerometer_getZ(), 8);

	sendAnswer(12);
}

void getValuesFromGyroscope(void)
{
	writeFloatToBuffer(gyroscope_getX(), 0);
	writeFloatToBuffer(gyroscope_getY(), 4);
	writeFloatToBuffer(gyroscope_getZ(), 8);

	sendAnswer(12);
}

void getBatteryVoltage(void)
{
	writeFloatToBuffer(charger_getBatteryVoltage(), 0);
	sendAnswer(4);
}

void getChargerVoltage(void)
{
	writeFloatToBuffer(charger_getChargerVoltage(), 0);
	sendAnswer(4);
}

void changeMotorState(unsigned char side, unsigned char state)
{
	motors_changeMotorState(side, state);
	sendOK();
}

unsigned short messaging_getParametersLength(unsigned char messageCode)
{
	switch((unsigned short) messageCode)
	{
		case MESSAGE_CODE_TEST_CONNECTION:
			return 0;

		case MESSAGE_CODE_GET_VALUE_FROM_ULTRASONIC_SENSOR:
			return 1;
		case MESSAGE_CODE_GET_VALUES_FROM_MAGNETOMETER:
		case MESSAGE_CODE_GET_VALUES_FROM_ACCELEROMETER:
		case MESSAGE_CODE_GET_VALUES_FROM_GYROSCOPE:
			return 0;

		case MESSAGE_CODE_GET_BATTERY_VOLTAGE:
		case MESSAGE_CODE_GET_CHARGER_VOLTAGE:
			return 0;

		case MESSAGE_CODE_CHANGE_MOTOR_STATE:
			return 2;
	}

	return -1;
}

void messaging_onMessageRead(unsigned char messageCode, unsigned char* parametersBuffer, unsigned short parametersBytesRead)
{
	switch((unsigned short) messageCode)
	{
		case MESSAGE_CODE_TEST_CONNECTION:
			testChannel();
			break;
		case MESSAGE_CODE_GET_VALUE_FROM_ULTRASONIC_SENSOR:
			getValueFromUltrasonicSensor(parametersBuffer[0]);
			break;
		case MESSAGE_CODE_GET_VALUES_FROM_MAGNETOMETER:
			getValuesFromMagnetometer();
			break;
		case MESSAGE_CODE_GET_VALUES_FROM_ACCELEROMETER:
			getValuesFromAccelerometer();
			break;
		case MESSAGE_CODE_GET_VALUES_FROM_GYROSCOPE:
			getValuesFromGyroscope();
			break;
		case MESSAGE_CODE_GET_BATTERY_VOLTAGE:
			getBatteryVoltage();
			break;
		case MESSAGE_CODE_GET_CHARGER_VOLTAGE:
			getChargerVoltage();
			break;
		case MESSAGE_CODE_CHANGE_MOTOR_STATE:
			changeMotorState(parametersBuffer[0], parametersBuffer[1]);
			break;
	}
}