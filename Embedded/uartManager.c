#include <avr/interrupt.h>
#include "uartManager.h"
#include "messaging.h"
#include "crc16.h"

receiver_state_t receiverState;

// onMessageRead_callback_func onMessageRead = 0;

unsigned char currentMessageCode;

unsigned char parametersBuffer[255];
unsigned short parametersBytesRead;
unsigned short needParametersBytesRead;

unsigned char crcBuffer[CRC_LENGTH];
unsigned short crcBytesRead;
	
unsigned char crcCheckBuffer[255];

//!!!need some reset method

//Подпрограмма обработки прерывания
ISR(USART_RXC_vect)
{
	unsigned char b = UDR;
	uart_onByteRead(b);
}

void startIdle(void)
{
	receiverState = IDLE;
}

void startReadParameters(void)
{
	receiverState = READING_PARAMETERS;
	parametersBytesRead = 0;
}

void startReadCRC(void)
{
	receiverState = READING_CRC;
	crcBytesRead = 0;
}

void readMessageCode(unsigned char code)
{
	currentMessageCode = code;
	needParametersBytesRead = messaging_getParametersLength(currentMessageCode);

	if(needParametersBytesRead == 0)
	{
		parametersBytesRead = 0;
		startReadCRC();
	}
	else
		startReadParameters();
}

void readParameterByte(unsigned char parameterByte)
{
	parametersBuffer[parametersBytesRead] = parameterByte;
	parametersBytesRead++;

	if(parametersBytesRead == needParametersBytesRead)
		startReadCRC();
}

char checkCRC16(void)
{
	crcCheckBuffer[0] = currentMessageCode;
	for(unsigned short i = 0; i < parametersBytesRead; i++)
		crcCheckBuffer[i + 1] = parametersBuffer[i];

	unsigned short checkSum = getCRC16CheckSum(crcCheckBuffer, parametersBytesRead + 1);

	return ((unsigned char)(checkSum % 256) == crcBuffer[0]) && ((unsigned char)(checkSum / 256) == crcBuffer[1]);
}

void readCRCByte(unsigned char crcByte)
{
	crcBuffer[crcBytesRead] = crcByte;
	crcBytesRead++;

	if(crcBytesRead == CRC_LENGTH)
	{
		if(checkCRC16())
		{
			messaging_onMessageRead(currentMessageCode, parametersBuffer, parametersBytesRead); 
			//if(onMessageRead != 0)
				
				// onMessageRead(currentMessageCode, parametersBuffer, parametersBytesRead);
		}

		startIdle();
	}
}

void USART_Init( unsigned int ubrr)//Инициализация модуля USART
{
	/* Задаем скорость работы USART */	
	UBRRH = (unsigned char)(ubrr>>8);	
	UBRRL = (unsigned char)ubrr;
	/* Разрешаем прием и передачу по USART */	 
	UCSRB=(1<<RXEN)|(1<<TXEN)|(1<<RXCIE);
	/* Устанавливаем формат данных 8 бит данных, 2 стоп бита */
	UCSRC = (1<<URSEL)|(1<<USBS)|(1<<UCSZ1)|(1<<UCSZ0);
}

// void uart_init_receiver(onMessageRead_callback_func onMessageRead_callback)
void uart_initReceiver(void)
{
	USART_Init(51);//9600 -- мало!!!!
	// onMessageRead = onMessageRead_callback;
	startIdle();
}

void uart_onByteRead(unsigned char x)
{	
	//uart_sendByte(x);//!!!debug
	switch(receiverState)
	{
		case IDLE:
			readMessageCode(x);
			break;
		case READING_PARAMETERS:
			readParameterByte(x);
			break;
		case READING_CRC:
			readCRCByte(x);
			break;
	}
}

void uart_sendBuffer(unsigned char* buffer, unsigned short length)
{
	for(unsigned short i = 0; i < length; i++)
		uart_sendByte(buffer[i]);
}

void uart_sendByte(unsigned char byte)
{
	while ( !(UCSRA & (1<<UDRE)) ); //Ожидание опустошения буфера приема
		UDR = byte; //Начало передачи данных	
}