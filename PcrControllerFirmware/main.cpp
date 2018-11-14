#include "driverlib.h"
#include <stdbool.h>
#include <UartManager.h>

using namespace UartManagement;

UartManager UartConnection;

int main(void)
{
    // stop watchdog
    WDT_A_hold(WDT_A_BASE);

    //Initialize all pins on all ports to prevent floating pins
    //Set them up as needed afterwards
    P1DIR = 0xFF;
    P1OUT = 0x00;

    P2DIR = 0xFF;
    P2OUT = 0x00;

    P3DIR = 0xFF;
    P3OUT = 0x00;

    P4DIR = 0xFF;
    P4OUT = 0x00;

    // LFXT Setup
    //Set PJ.4 and PJ.5 as Primary Module Function Input.
    /*

     * Select Port J
     * Set Pin 4, 5 to input Primary Module Function, LFXT.
     */
    GPIO_setAsPeripheralModuleFunctionInputPin(
            GPIO_PORT_PJ,
            GPIO_PIN4 + GPIO_PIN5,
            GPIO_PRIMARY_MODULE_FUNCTION
    );

    //Set DCO frequency to 1 MHz
    CS_setDCOFreq(CS_DCORSEL_0,CS_DCOFSEL_6);
    //Set external clock frequency to 32.768 KHz
    CS_setExternalClockSource(32768,0);
    //Set ACLK=LFXT
    CS_initClockSignal(CS_ACLK,CS_LFXTCLK_SELECT,CS_CLOCK_DIVIDER_1);
    //Set SMCLK = DCO with frequency divider of 1
    CS_initClockSignal(CS_SMCLK,CS_DCOCLK_SELECT,CS_CLOCK_DIVIDER_1);
    //Set MCLK = DCO with frequency divider of 1
    CS_initClockSignal(CS_MCLK,CS_DCOCLK_SELECT,CS_CLOCK_DIVIDER_1);
    //Start XT1 with no time out
    CS_turnOnLFXT(CS_LFXT_DRIVE_0);


    // Configure UART pins
    //Set P2.0 and P2.1 as Secondary Module Function Input.
    /*

     * Select Port 2d
     * Set Pin 0, 1 to input Secondary Module Function, (UCA0TXD/UCA0SIMO, UCA0RXD/UCA0SOMI).
     */
    GPIO_setAsPeripheralModuleFunctionInputPin(
            GPIO_PORT_P2,
            GPIO_PIN0 + GPIO_PIN1,
            GPIO_SECONDARY_MODULE_FUNCTION
    );

    /*
     * Disable the GPIO power-on default high-impedance mode to activate
     * previously configured port settings
     */
    PMM_unlockLPM5();

    __enable_interrupt();

    UartConnection.UartConfigure();



    while (1)
    {
        //        __bis_SR_register(LPM3_bits + GIE);
    }
}

#if defined(__TI_COMPILER_VERSION__) || defined(__IAR_SYSTEMS_ICC__)
#pragma vector=USCI_A0_VECTOR
__interrupt
#elif defined(__GNUC__)
__attribute__((interrupt(USCI_A0_VECTOR)))
#endif
void USCI_A0_ISR(void)
{
    switch(__even_in_range(UCA0IV,USCI_UART_UCTXCPTIFG))
    {
    case USCI_NONE: break;
    case USCI_UART_UCRXIFG:
        UartConnection.UartDataReceived();
        break;
    case USCI_UART_UCTXIFG: break;
    case USCI_UART_UCSTTIFG: break;
    case USCI_UART_UCTXCPTIFG: break;
    }
}
