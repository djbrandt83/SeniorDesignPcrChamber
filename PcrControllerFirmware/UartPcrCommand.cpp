/*
 * UartPcrCommand.cpp
 *
 *  Created on: Nov 11, 2018
 *      Author: dbran
 */
#include "driverlib.h"
#include <UartPcrCommand.h>

namespace UartManagement
{
    UartPcrCommand::UartPcrCommand()
    {
        clearCommand();
        CharsInCommand = 0;
        CharsInValue = 0;
    }

    UartPcrCommand::~UartPcrCommand()
    {
        // TODO Auto-generated destructor stub
    }

    void UartPcrCommand::parseCommand(char* buffer, uint8_t size)
    {
        uint8_t i = 0;

        while (buffer[i] != ',' && i < size)
        {
            if(buffer[i] == 0x0A)
            {
                //Character is a new line so we can ignore it
                i++;
            }
            else if(buffer[i] == '%')
            {
                //Character signals the end of the command so we can ignore it
               i++;
            }
            else
            {
                CommandName[CharsInCommand] = buffer[i];
                i++;
                CharsInCommand++;
            }
        }

        i++; //The ',' will not be part of the value so we need to skip it

        while (buffer[i] != ',' && i < size)
        {
            CommandValue[CharsInValue] = buffer[i];
            i++;
            CharsInValue++;
        }
    }

    void UartPcrCommand::clearCommand()
    {
        for (int8_t i=63; i>=0; i--)
        {
            CommandName[i] = 0;
            CommandValue[i] = 0;
        }

        CharsInCommand = 0;
        CharsInValue = 0;
    }
}
