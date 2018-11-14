using System;
using System.Windows;
using System.IO.Ports;

namespace EE595PcrController
{
    class PCRDevice
    {
        private static readonly String DisconnectedMessage = "No device is currently connected";
        private static readonly String ConnectedMessage = "The device is currently connected";

        public String ConnectionMessage;
        public String DeviceName;
        public Boolean DeviceOnline;
        public Int32 OutputTemperature;
        public Int32 ReferenceTemperature;

        private SerialPort port;
        private static readonly Int32 baudRate = 57600;
        private static readonly Parity parity = Parity.None;
        private static readonly Int32 serialDataBits = 8;
        private static readonly StopBits numStopBits = StopBits.One;
        private static readonly Int32 readTimeOut = 5000;
        private static readonly Int32 writeTimeOut = 5000;

        public event EventHandler OnDeviceConnected;
        public event EventHandler OnDeviceDisconnected;
        public event EventHandler OnExperimentStarted;
        public event EventHandler OnExperimentTerminated;

        public PCRDevice()
        {
            ConnectionMessage = DisconnectedMessage;
            DeviceOnline = false;
            OutputTemperature = 20;
            ReferenceTemperature = 20;
            DeviceName = String.Empty;
        }

        public void StartStopExperiment(Boolean experimentRunning)
        {
            if(!experimentRunning)
            {
                StartExperiment();
            }
            else
            {
                StopExperiment();
            }
        }

        public void ConnectToDevice()
        {
            if(!DeviceOnline)
            {
                port = new SerialPort(DeviceName, baudRate, parity, serialDataBits, numStopBits);
                port.ReadTimeout = readTimeOut;
                port.WriteTimeout = writeTimeOut;
                try
                {
                    port.Open();
                    port.WriteLine("PcrLeaderOnline%");
                    port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                }
                catch(UnauthorizedAccessException)
                {
                    MessageBox.Show("The Selected COM Port is unavailable. Please verify you have selected the correct device");
                }
            }
            else
            {
                //Disconnect from device.
                //TODO: Determine how to disconnect from device gracefully
                //      Some predetermined command should be sent to FW to tell it to go to
                //      a safe temperature range if it is not already there.

                port.Close();
                OnDeviceDisconnected(this, EventArgs.Empty);
            }
        }

        private void StopExperiment()
        {
            port.WriteLine("StopPcr%");
            port.Close();
            OnExperimentTerminated(this, EventArgs.Empty);
        }

        private void StartExperiment()
        {
            port.WriteLine("StartPcr%");
            OnExperimentStarted(this, EventArgs.Empty);
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!DeviceOnline)
            {
                String data = port.ReadExisting();

                if(data == "PcrFollowerOnline")
                {
                    ConnectionMessage = ConnectedMessage;
                    OnDeviceConnected(this, EventArgs.Empty);
                }
                else
                {
                    ConnectionMessage = DisconnectedMessage;
                    port.Close();
                    OnDeviceDisconnected(this, EventArgs.Empty);
                }
            }
        }
    }
}
