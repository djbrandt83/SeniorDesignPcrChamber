/*
 * UartManager.h
 *
 *  Created on: Nov 10, 2018
 *      Author: dbran
 */
#include <UartPcrCommand.h>

#ifndef UARTMANAGER_H_
#define UARTMANAGER_H_

namespace UartManagement
{
    class UartManager
    {
    public:
        UartManager();
        virtual ~UartManager();
        void UartConfigure(void);
        void UartDataReceived(void);
        void UartDataSend(char*, uint8_t);

        UartPcrCommand Command;
        bool ConnectedToGui;
        bool PcrCommandAvailable;

    private:
        void clearRxBuffer(char*);
        uint8_t getCommand(char*, uint8_t);
        bool compareCharArrays(char*, char*, uint8_t);

        char rxBuffer[64];
        uint8_t rxBufferIndex;
    };
}


#endif /* UARTMANAGER_H_ */
