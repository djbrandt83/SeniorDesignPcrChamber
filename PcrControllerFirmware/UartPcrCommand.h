/*
 * UartPcrCommand.h
 *
 *  Created on: Nov 11, 2018
 *      Author: dbran
 */

#ifndef UARTPCRCOMMAND_H_
#define UARTPCRCOMMAND_H_

namespace UartManagement
{
    class UartPcrCommand
    {
        public:
            UartPcrCommand();
            virtual ~UartPcrCommand();

            void parseCommand(char*,uint8_t);
            void clearCommand(void);

            char CommandName[64];
            char CommandValue[64];

            uint8_t CharsInCommand;
            uint8_t CharsInValue;
    };
}
#endif /* UARTPCRCOMMAND_H_ */
