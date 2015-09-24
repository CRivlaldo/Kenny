#ifndef INTERRUPTIONS_H
#define INTERRUPTIONS_H

typedef void (*onInterruptionDelegate)(void);
void interruptions_waitForRisingEdge(void);
void interruptions_waitForFailingEdge(void);
void interruptions_waitAnyChange(void);
void interruptions_enable(unsigned char interruption);
void interruptions_disable(unsigned char interruption);
void interruptions_setOnINT0InterruptionDelegate(onInterruptionDelegate func);

#endif