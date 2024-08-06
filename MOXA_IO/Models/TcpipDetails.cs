using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace MOXA_IO.Models
{
    public class TcpipDetails : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<string> _NetworkList;
        public ObservableCollection<string> NetworkList
        {
            get { return _NetworkList; }
            set { SetProperty(ref _NetworkList, value); }
        }

        private string _ipaddress;
        public string IPAddress
        {
            get { return _ipaddress; }
            set { SetProperty(ref _ipaddress, value); }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        private string _selectedNetwork;
        public string SelectedNetwork
        {
            get { return _selectedNetwork; }
            set { SetProperty(ref _selectedNetwork, value); }
        }

        private ObservableCollection<MessageDetails> _ReceivedMsg;
        public ObservableCollection<MessageDetails> ReceivedMsg
        {
            get { return _ReceivedMsg; }
            set
            { SetProperty(ref _ReceivedMsg, value); }
        }

        private ObservableCollection<MessageDetails> _SentMsg;
        public ObservableCollection<MessageDetails> SentMsg
        {
            get { return _SentMsg; }
            set
            { SetProperty(ref _SentMsg, value); }
        }

        private string _SendMessage;
        public string SendMessage
        {
            get { return _SendMessage; }
            set { SetProperty(ref _SendMessage, value); }
        }

        private string _lblConnection = "Start";
        public string LblConnection
        {
            get { return _lblConnection; }
            set { SetProperty(ref _lblConnection, value); }
        }

        private string _lblConnectivityStatus = "Disconnected";
        public string LblConnectivityStatus
        {
            get { return _lblConnectivityStatus; }
            set { SetProperty(ref _lblConnectivityStatus, value); }
        }

        private bool _isEnable_NetList = true;
        public bool IsEnable_NetList
        {
            get { return _isEnable_NetList; }
            set
            {
                SetProperty(ref _isEnable_NetList, value);
                if (!value)
                {
                    NetworkList.Clear();
                }
            }
        }

        public bool AutoReconnect { get; set; }
    }

    public class MessageDetails : BindableBase
    {
        private string _DateTime;
        public string DateTime
        {
            get { return _DateTime; }
            set { SetProperty(ref _DateTime, value); }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }
    }
}
