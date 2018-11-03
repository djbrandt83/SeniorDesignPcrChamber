using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE595PcrController
{
    class PCRDevice
    {
        private static String ConnectedMessage = "No device is currently connected";
        private static String DisconnectedMessage = "The device is currently connected";

        public String ConnectionMessage;
        public Boolean DeviceOnline;

        public PCRDevice()
        {
            ConnectionMessage = DisconnectedMessage;
            DeviceOnline = false;
        }
    }
}
