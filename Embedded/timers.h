#ifndef TIMERS_H
#define TIMERS_H

#define TIMER0_OVERFLOW_IN_MILLISECOND 61 / 500 /*   F_CPU / 1000 / 256 / 256     */
#define TIMER1_TICKS_IN_MICROSECOND 8           /*   F_CPU / 1000000              */

typedef void (*onTimerTickDelegate)(void);

void timers_initTimer0(unsigned long intervalInMilliseconds, onTimerTickDelegate onTimerTick);
void timers_stopTimer0(void);

void timers_initTimer1(unsigned short prescaler);
void timers_timer1Reset(void);
unsigned long timers_getTimer1ElapsedTimeInMicroseconds(void);
unsigned short timers_getTimer1Count(void);
void timers_stopTimer1(void);

#endif