using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE595PcrController
{
    class PCRDevice
    {
        private static readonly String ConnectedMessage = "No device is currently connected";
        private static readonly String DisconnectedMessage = "The device is currently connected";

        public String ConnectionMessage;
        public Boolean DeviceOnline;
        public Int32 OutputTemperature;
        public Int32 ReferenceTemperature;

        public PCRDevice()
        {
            ConnectionMessage = DisconnectedMessage;
            DeviceOnline = false;
            OutputTemperature = 20;
            ReferenceTemperature = 20;
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

        private void StopExperiment()
        {

        }

        private void StartExperiment()
        {

        }
    }
}
