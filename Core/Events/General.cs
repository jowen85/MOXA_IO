using Core.Enums;
using Core.Variables;
using Prism.Events;

namespace Core.Events
{
    public class Device : PubSubEvent<Device>
    {
        public string DeviceName;
        public bool DeviceConnectivity;
    }

    public class MachineAlarm : PubSubEvent<ErrCode>
    {
    }

    public class MachineOperation:PubSubEvent<OperationCmd>
    {
    }

    public class MachineStatus :PubSubEvent<Machine_Status>
    {
    }
}
