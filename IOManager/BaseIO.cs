using System.Text;

namespace IOManager
{

    /// <summary>
    /// Hold the constant value for ON / OFF
    /// </summary>
    public sealed class State
    {
        #region Variables
        public const ushort OFF = 0;
        public const ushort ON = 1;
        #endregion Variables

        #region Constructor
        private State()
        {
        }
        #endregion Constructor
    }

    /// <summary>
    /// BaseIO - Abstract Base Class for IO Server Implementation.
    /// </summary>
    public abstract class BaseIO : IBaseIO
    {
        #region Variables
        //[StructLayout(LayoutKind.Sequential)]
        protected struct tagDIOInfo
        {
            internal ushort devNum;
            internal ushort cardID;
            internal ushort setID;
            internal ushort slaveNo;
            internal ushort port;
            internal ushort bit;
        }

        protected tagDIOInfo m_oInfo = new tagDIOInfo();
        protected tagDIOInfo m_iInfo = new tagDIOInfo();
        protected tagDIOInfo m_DOInfo = new tagDIOInfo();

        private StringBuilder m_ErrMsg = new StringBuilder();

        protected int m_RetCode = 0;
        #endregion Variables

        #region Constructor
        public BaseIO()
        {
        }
        #endregion Constructor

        #region Methods
        public virtual bool OpenDevice()
        {
            return false;
        }

        public virtual bool CloseDevice()
        {
            return false;
        }

        public virtual bool ClearOutput()
        {
            return false;
        }

        public virtual bool ReadInBit(ushort? bit)
        {
            return false;
        }

        public virtual bool ReadInBit(ushort? bit, bool invert)
        {
            return false;
        }

        public virtual bool ReadOutBit(ushort? bit)
        {
            return false;
        }

        public virtual bool WriteOutBit(ushort? bit, ushort state)
        {
            return false;
        }

        public virtual uint ReadPort(ushort startBitNum)
        {
            return 0;
        }

        public virtual bool WritePort(ushort startBitNum, uint oData)
        {
            return false;
        }

        #endregion Methods

        #region Properties
        /// <summary>
        /// Maximum number of bit exist in a port / card.
        /// For Adlink Card, this will represent the maximum number of bit exsit in a slave module.
        /// For Advantech Card, it will be 8.
        /// </summary>
        public ushort Max_Bit_PerPort { set; get; }
        //public ushort Max_Bit_PerPort { set; get; } = 16;

        /// <value>
        /// Error message reported after a call into the actual IO library.
        /// </value>
        public string Err_Msg
        {
            set
            {
                m_ErrMsg.Remove(0, m_ErrMsg.Length);
                m_ErrMsg.Append(value);
            }
            get { return m_ErrMsg.ToString(); }
        }

        public bool IsConnected { set; get; } = false;

        #endregion Properties
    }
}
