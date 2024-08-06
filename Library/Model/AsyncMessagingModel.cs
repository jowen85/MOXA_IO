using PolarisCommunication;

namespace Library.Model
{
    public class AsyncMessagingModel : ModelBase
    {
        private bool _asyncMessagingStarted;
        public bool AsyncMessagingStarted
        {
            get => _asyncMessagingStarted;
            set
            {
                _asyncMessagingStarted = value;
                FirePropertyChanged();
            }
        }

        private string _motionDoneReceived;
        /// <summary>
        /// 
        /// </summary>
        public string MotionDoneReceived
        {
            get => _motionDoneReceived;
            set
            {
                _motionDoneReceived = value;
                FirePropertyChanged();
            }
        }

        private string _genericDataReceived;

        public string GenericMessageReceived
        {
            get => _genericDataReceived;
            set
            {
                _genericDataReceived = value;
                FirePropertyChanged();
            }
        }

        private AsyncMessageEventArgs _errorReceived;

        public AsyncMessageEventArgs ErrorReceived
        {
            get => _errorReceived;
            set
            {
                _errorReceived = value;
                FirePropertyChanged();
            }
        }
    }
}
