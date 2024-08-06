using PolarisCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOManager
{
    public interface IPolarisIO
    {
        void StartScanIO();
        //bool OpenDevice(IScript script);
        bool DeviceIsConnected();
        void Connect();
        void Disconnect();

        Script Script { get; set; }
    }
}
