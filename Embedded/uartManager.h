#ifndef UART_MANAGER_H
#define UART_MANAGER_H

#define CRC_LENGTH 2

typedef enum {IDLE, READING_PARAMETERS, READING_CRC} receiver_state_t;

// typedef void (*onMessageRead_callback_func)(unsigned char, unsigned char*, unsigned short);

extern const unsigned short crcLength;

// void uart_init_receiver(onMessageRead_callback_func onMessageRead_callback);
void uart_initReceiver(void);
void uart_onByteRead(unsigned char x);

void uart_sendBuffer(unsigned char* buffer, unsigned short length);
void uart_sendByte(unsigned char byte);

#endif