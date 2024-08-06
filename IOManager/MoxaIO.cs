using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace IOManager
{
    public class MoxaIO : BaseIO, IMoxaIO
    {
        #region  Variables
        internal int m_NumOfIOCardInput = 0;
        internal int m_NumOfIOCardOutput = 0;
        internal bool m_FailTriggerOutBit;
        internal const ushort Port = 502;
        internal const int MaxCardSlot = 100;
        internal const int MaxBit = 16;
        internal string Password = "";
        internal uint TimeOut = 2000;

        internal int[] DI_hConnection = null;
        internal int[] DO_hConnection = null;

        internal string[] m_DI_IpAddress = new string[MaxCardSlot];
        internal string[] m_DO_IpAddress = new string[MaxCardSlot];

        internal int[] m_MaxDIPortNum = new int[MaxCardSlot];
        internal int[] m_MaxDOPortNum = new int[MaxCardSlot];

        internal uint[] dwGetDIValue = null;
        internal uint[] dwGetDOValue = null;

        internal uint[] retDIValue = new uint[10];
        internal uint[] retDOValue = new uint[10];

        internal bool[,] InputBitStatus = new bool[MaxCardSlot, MaxBit];
        internal bool[,] OutputBitStatus = new bool[MaxCardSlot, MaxBit];

        static public bool[,] SetOutput = new bool[MaxCardSlot, MaxBit];
        Thread[] IORefesh = new Thread[MaxCardSlot];
        #endregion Variables

        #region Constructor
        public MoxaIO()
        {
        }
        #endregion Constructor

        #region Methods
        // Overrides
        /// <summary>
        /// Retrieves parameters pertaining to the device's operation 
        /// from the Registry or configuration file, and allocates memory 
        /// to store it for quick reference. 
        /// This function must be called before any other functions. 
        /// </summary>
        /// <returns></returns>
        public override bool OpenDevice()
        {
            return InitMoxaIO();
        }

        /// <summary>
        /// This function is to close the HSL master card with card_ID. This
        /// function is used to tell library that this registered card is not used
        /// currently and can be released. By the end of a program, you need to
        /// use this function to release all cards that were registered.
        /// </summary>
        /// <returns></returns>
        public override bool CloseDevice()
        {
            return true;
        }

        /// <summary>
        /// This fucntion write zero value to all the output modules.
        /// </summary>
        /// <returns></returns>
        public override bool ClearOutput()
        {
            return true;
        }

        /// <summary>
        /// This function is to read the digital input value of the specified
        /// channel/bit on the slave I/O module.
        /// </summary>
        /// <param name="iNum">Value from IN enum</param>
        /// <returns></returns>
        public override bool ReadInBit(ushort? bit)
        {
            return MoxaInputBitStatus((int)bit);
        }

        /// <summary>
        /// This function is to read the digital input value of the specified
        /// channel/bit on the slave I/O module.
        /// </summary>
        /// <param name="iNum">Value from IN enum</param>
        /// <param name="invert">Invert the input result</param>
        /// <returns></returns>
        public override bool ReadInBit(ushort? bit, bool invert)
        {
            if (invert)
            {
                return !MoxaInputBitStatus((int)bit);
            }
            else
            {
                return MoxaInputBitStatus((int)bit);
            }
        }

        /// <summary>
        /// This function reads the output status in the card's RAM
        /// </summary>
        /// <param name="oNum">Value from OUT enum</param>
        /// <returns></returns>
        public override bool ReadOutBit(ushort? bit)
        {
            return MoxaOutputBitStatus((int)bit);
        }

        /// <summary>
        /// This function is to write the digital output value to the specified
        /// digital channel of slave I/O module.
        /// </summary>
        /// <param name="oNum">Value from OUT enum</param>
        /// <param name="state">Specify ON/OFF state</param>
        /// <returns></returns>
        public override bool WriteOutBit(ushort? bit, ushort state)
        {
            lock (this)
            {
                if (bit == null)
                {
                    return false;
                }
                uint _state = Convert.ToUInt32(state);
                int slot = (ushort)bit / Max_Bit_PerPort;
                byte _output = (byte)(bit % Max_Bit_PerPort);
                SetMoxaOutput(slot, _output, _state);
                return true;
            }
        }

        /// <summary>
        /// Set Digital Output Value 
        /// </summary>
        /// <param name="slot"></param>Slot Number
        /// <param name="bit"></param>Bit Number (0-15)
        /// <param name="dwSetDOValue"></param>State (0/1)
        private void SetMoxaOutput(int slot, byte bit, uint dwSetDOValue)
        {
            try
            {
                int ret;
                ret = MXIO.E1K_DO_Writes(DO_hConnection[slot], bit, 0, dwSetDOValue);

                if (ret != MXIO.MXIO_OK)
                {
                    Err_Msg = string.Format("SetMoxaOutput(), m_IsConnected = false, m_Slot= {0},m_Bit= {1}, m_dwSetDOValue ={2}", slot, bit, dwSetDOValue);
                    m_FailTriggerOutBit = true;
                }
                else
                {
                    m_FailTriggerOutBit = false;
                }
            }
            catch (Exception ex)
            {
                Err_Msg = ex.Message.ToString();
                return;
            }
        }

        /// <summary>
        /// This function is to read the digital input value of the slave I/O module.
        /// </summary>
        /// <param name="startBitNum">The first bit in the slave module</param>
        /// <returns></returns>
        public override uint ReadPort(ushort startBitNum)
        {
            return 0;
        }

        /// <summary>
        /// This function is to write the digital output value to the slave I/O
        /// module.
        /// </summary>
        /// <param name="startBitNum">The first bit in the slave module</param>
        /// <param name="oData">Digital Output Value</param>
        /// <returns></returns>
        public override bool WritePort(ushort startBitNum, uint oData)
        {
            return true;
        }

        bool InitMoxaIO()
        {
            int ret;
            IsConnected = true;
            try
            {
                ret = MXIO.MXEIO_Init();

                DI_hConnection = new int[m_NumOfIOCardInput];
                DO_hConnection = new int[m_NumOfIOCardOutput];
                dwGetDIValue = new uint[m_NumOfIOCardInput + 1];
                dwGetDOValue = new uint[m_NumOfIOCardOutput + 1];

                for (int _i = 0; _i < DI_hConnection.Length; _i++)
                {
                    ret = MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DI_IpAddress[_i]), Port, TimeOut, out DI_hConnection[_i], Encoding.UTF8.GetBytes(Password));
                    if (ret != MXIO.MXIO_OK)
                    {
                        Debug.WriteLine("InitMoxaIO, MXEIO_E1K_Connect - return code :" + ret);
                        IsConnected = false;
                        Err_Msg = "Connect Input Module Fail\r\n";
                    }
                }

                for (int _i = 0; _i < DO_hConnection.Length; _i++)
                {
                    ret = MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DO_IpAddress[_i]), Port, TimeOut, out DO_hConnection[_i], Encoding.UTF8.GetBytes(Password));
                    if (ret != MXIO.MXIO_OK)
                    {
                        Debug.WriteLine("InitMoxaIO, MXEIO_E1K_Connect - return code :" + ret);
                        IsConnected = false;
                        Err_Msg = "Connect Output Module Fail\r\n";
                    }
                }

                return IsConnected;
            }
            catch (Exception ex)
            {
                Err_Msg = ex.Message.ToString();
                return false;
            }
        }

        private bool MoxaOutputBitStatus(int bit)
        {
            lock (this)
            {
                int slot = bit / Max_Bit_PerPort;
                int index = bit % Max_Bit_PerPort;
                return OutputBitStatus[slot, index];
            }
        }

        private bool MoxaInputBitStatus(int bit)
        {
            lock (this)
            {
                int slot = bit / Max_Bit_PerPort;
                int index = bit % Max_Bit_PerPort;
                return InputBitStatus[slot, index];
            }
        }

        void ScanMoxaInput(object slot)
        {
            int ret;
            int bitNo = 1;
            int Slot = (int)slot;
            byte[] byteStatus = new byte[1];

            do
            {
                Thread.Sleep(1);
                if (IsConnected)
                {
                    try
                    {
                        ret = MXIO.E1K_DI_Reads(DI_hConnection[Slot], 0, 16, ref dwGetDIValue[Slot]);
                        if (ret != MXIO.MXIO_OK)
                        {
                            Err_Msg = "ReadMoxaInput(), m_IsConnected = false.";
                            MXIO.MXEIO_CheckConnection(DI_hConnection[Slot], TimeOut, byteStatus);
                            if (byteStatus[0] == 1)
                            {
                                MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DI_IpAddress[Slot]), Port, TimeOut, out DI_hConnection[Slot], Encoding.UTF8.GetBytes(Password));
                            }
                        }

                        for (int i = 0; i <= 15; i++)
                        {
                            if ((dwGetDIValue[Slot] & bitNo) == bitNo)
                            {
                                InputBitStatus[Slot, i] = true;
                            }
                            else
                            {
                                InputBitStatus[Slot, i] = false;
                            }
                            bitNo *= 2;
                        }
                        bitNo = 1;
                    }
                    catch (Exception ex)
                    {
                        Err_Msg = ex.Message.ToString();
                        return;
                    }
                }
            }
            while (true);
        }

        void ScanMoxaOutput(object slot)
        {
            int ret;
            int bitNo = 1;
            int Slot = (int)slot;
            byte[] byteStatus = new byte[1];

            do
            {
                Thread.Sleep(1);
                if (IsConnected)
                {
                    try
                    {
                        ret = MXIO.E1K_DO_Reads(DO_hConnection[Slot], 0, 16, ref dwGetDOValue[Slot]);
                        if (ret != MXIO.MXIO_OK)
                        {
                            Err_Msg = "ReadMoxaOutput(), m_IsConnected = false.";
                            MXIO.MXEIO_CheckConnection(DO_hConnection[Slot], TimeOut, byteStatus);
                            if (byteStatus[0] == 1)
                            {
                                MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DO_IpAddress[Slot]), Port, TimeOut, out DO_hConnection[Slot], Encoding.UTF8.GetBytes(Password));
                            }
                        }
                        int val = 15;
                        //m_MaxDOPortNum[Slot];
                        for (int i = 0; i <= val; i++)
                        {
                            if ((dwGetDOValue[Slot] & bitNo) == bitNo)
                            {
                                OutputBitStatus[Slot, i] = true;
                            }
                            else
                            {
                                OutputBitStatus[Slot, i] = false;
                            }
                            bitNo *= 2;
                        }
                        bitNo = 1;

                    }
                    catch (Exception ex)
                    {
                        Err_Msg = ex.Message.ToString();
                        return;
                    }
                }
            }
            while (true);
        }

        public void StartScanIO()
        {
            InitMoxaIO();

            for (int i = 0; i < m_NumOfIOCardInput; i++)
            {
                IORefesh[i] = new Thread(ScanMoxaInput);
                IORefesh[i].Start(i);
            }
            for (int i = 0; i < m_NumOfIOCardOutput; i++)
            {
                IORefesh[i] = new Thread(ScanMoxaOutput);
                IORefesh[i].Start(i);
            }
        }
        #endregion Methods

        #region Properties

        string[] IMoxaIO.DI_IpAddress
        {
            set { m_DI_IpAddress = value; }
            get { return m_DI_IpAddress; }
        }

        string[] IMoxaIO.DO_IpAddress
        {
            set { m_DO_IpAddress = value; }
            get { return m_DO_IpAddress; }
        }

        int[] IMoxaIO.MaxDIPortNum
        {
            set { m_MaxDIPortNum = value; }
            get { return m_MaxDIPortNum; }
        }

        int[] IMoxaIO.MaxDOPortNum
        {
            set { m_MaxDOPortNum = value; }
            get { return m_MaxDOPortNum; }
        }
        int IMoxaIO.Num_IO_Card_Input
        {
            set { m_NumOfIOCardInput = value; }
            get { return m_NumOfIOCardInput; }
        }

        int IMoxaIO.Num_IO_Card_Output
        {
            set { m_NumOfIOCardOutput = value; }
            get { return m_NumOfIOCardOutput; }
        }

        bool IMoxaIO.FailTriggerOutBit
        {
            set { m_FailTriggerOutBit = value; }
            get { return m_FailTriggerOutBit; }
        }
        #endregion Properties
    }
}

