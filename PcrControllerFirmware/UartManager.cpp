/*
 * UartManager.cpp
 *
 *  Created on: Nov 10, 2018
 *      Author: dbran
 */
#include "driverlib.h"
#include <UartManager.h>
#include <assert.h>
#include <string.h>

using namespace UartManagement;

namespace UartManagement
{
    UartManager::UartManager()
    {
        ConnectedToGui = false;
        Command = UartPcrCommand();
        rxBufferIndex = 0;
    }

    UartManager::~UartManager()
    {
        // TODO Auto-generated destructor stub
    }

    void UartManager::UartConfigure()
    {
        //We are setting up the connection for 57600
        EUSCI_A_UART_initParam param = {0};
        param.selectClockSource = EUSCI_A_UART_CLOCKSOURCE_SMCLK;
        param.clockPrescalar = 8;
        param.firstModReg = 10;
        param.secondModReg = 0xF7;
        param.parity = EUSCI_A_UART_NO_PARITY;
        param.msborLsbFirst = EUSCI_A_UART_LSB_FIRST;
        param.numberofStopBits = EUSCI_A_UART_ONE_STOP_BIT;
        param.uartMode = EUSCI_A_UART_MODE;
        param.overSampling = EUSCI_A_UART_OVERSAMPLING_BAUDRATE_GENERATION;

        if (STATUS_FAIL == EUSCI_A_UART_init(EUSCI_A0_BASE, &param)) {
            return;
        }

        EUSCI_A_UART_enable(EUSCI_A0_BASE);

        EUSCI_A_UART_clearInterrupt(EUSCI_A0_BASE,
                                    EUSCI_A_UART_RECEIVE_INTERRUPT);

        // Enable USCI_A0 RX interrupt
        EUSCI_A_UART_enableInterrupt(EUSCI_A0_BASE,
                                     EUSCI_A_UART_RECEIVE_INTERRUPT);
    }

    void UartManager::UartDataReceived()
    {
        uint8_t data = EUSCI_A_UART_receiveData(EUSCI_A0_BASE);
        rxBuffer[rxBufferIndex] = (char)data;

        if(rxBuffer[rxBufferIndex] == '%')
        {
            getCommand(rxBuffer, rxBufferIndex + 1); //Index starts at zero so add one to get length
            clearRxBuffer(rxBuffer);
            rxBufferIndex = 0;
            return;
        }

        rxBufferIndex++;
    }

    uint8_t UartManager::getCommand(char* buffer, uint8_t size)
    {
        Command.parseCommand(buffer, size);

        if(compareCharArrays((char*) Command.CommandName, (char*) "PcrLeaderOnline", Command.CharsInCommand))
        {
            UartDataSend((char*)"PcrFollowerOnline",17);
        }
        else if(compareCharArrays((char*) Command.CommandName, (char*) "StartPcr", Command.CharsInCommand))
        {
            UartDataSend((char*)"StartingExperiment",17);
        }

        Command.clearCommand();

        return 2;
    }

    void UartManager::UartDataSend(char* dataToSend, uint8_t length)
    {
        for(uint8_t i = 0; i<length; i++)
        {
            EUSCI_A_UART_transmitData(EUSCI_A0_BASE, dataToSend[i]);
        }
    }

    bool UartManager::compareCharArrays(char* stringOne, char* stringTwo, uint8_t length)
    {
        for (uint8_t i = 0; i<length; i++)
        {
            if(stringOne[i] != stringTwo[i])
            {
                return false;
            }
        }
        return true;
    }

    void UartManager::clearRxBuffer(char* buffer)
    {
        for (int8_t i=64; i>=0; i--)
        {
            buffer[i] = 0;
        }
    }
}
