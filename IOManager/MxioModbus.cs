using ModbusLib;
using ModbusLib.Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOManager
{
    public class MxioModbus : BaseIO, IMoxaIO
    {
        #region  Variables
        private int _transactionId;
        internal ushort[] _registerData;
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
        protected Socket _socket;
        private ModbusClient _driver;
        private ICommClient _portClient;
        #endregion Variables

        #region Constructor
        public MxioModbus()
        {
            _registerData = new ushort[65600];
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
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                _socket.SendTimeout = 2000;
                _socket.ReceiveTimeout = 2000;
                _socket.Connect(new IPEndPoint(IPAddress.Parse("192.168.127.254"), Port));
                _portClient = _socket.GetClient();
                _driver = new ModbusClient(new ModbusTcpCodec()) { Address = 1 };
                _driver.OutgoingData += DriverOutgoingData;
                _driver.IncommingData += DriverIncommingData;
                IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void DriverIncommingData(byte[] data, int len)
        {
            //if (_logPaused)
            //    return;
            var hex = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                hex.AppendFormat("{0:x2} ", data[i]);
            }
            //AppendLog(String.Format("RX: {0}", hex));
        }

        protected void DriverOutgoingData(byte[] data)
        {
            //if (_logPaused)
            //    return;
            var hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2} ", b);
            //AppendLog(String.Format("TX: {0}", hex));
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

                try
                {
                    uint _state = Convert.ToUInt32(state);
                   
                    if (bit == 0)
                    {
                        int slot = (ushort)bit / Max_Bit_PerPort;
                        byte _output = (byte)(bit % Max_Bit_PerPort);
                        //SetMoxaOutput(slot, _output, _state);
                        var bulbHiByte = (bit & 0x0008) != 0;
                        ushort shifter = bulbHiByte ? (ushort)0x0001 : (ushort)0x0100;
                        var shift = bit & 0x0007;
                        var mask = Convert.ToUInt16(shifter << shift);

                        if (_state == 1)
                        {
                            _registerData[(int)bit] |= mask;
                        }
                        else
                        {
                            mask = (ushort)~mask; ;
                            _registerData[(int)bit] &= mask;
                        }
                    }
                    else
                    {
                        int slot = (ushort)0 / Max_Bit_PerPort;
                        byte _output = (byte)(0 % Max_Bit_PerPort);
                        //SetMoxaOutput(slot, _output, _state);
                        var bulbHiByte = (0 & 0x0008) != 0;
                        ushort shifter = bulbHiByte ? (ushort)0x0001 : (ushort)0x0100;
                        var shift = 0 & 0x0007;
                        var mask = Convert.ToUInt16(shifter << shift);

                        if (_state == 1)
                        {
                            _registerData[(int)bit] |= mask;
                        }
                        else
                        {
                            mask = (ushort)~mask; ;
                            _registerData[(int)bit] &= mask;
                        }
                    }

                    var command = new ModbusCommand(ModbusCommand.FuncWriteCoil)
                    {
                        Offset = (int)bit,
                        Count = 1,
                        TransId = _transactionId++,
                        Data = new ushort[1]
                    };
                    command.Data[0] = (ushort)(_registerData[(int)bit] & 0x0100);
                    var result = _driver.ExecuteGeneric(_portClient, command);
                    //AppendLog(result.Status == CommResponse.Ack
                    //              ? String.Format("Write succeeded: Function code:{0}", ModbusCommand.FuncWriteCoil)
                    //              : String.Format("Failed to execute Write: Error code:{0}", result.Status));

                    string msg = result.Status == CommResponse.Ack
                              ? String.Format("Write succeeded: Function code:{0}", ModbusCommand.FuncWriteCoil)
                              : String.Format("Failed to execute Write: Error code:{0}", result.Status);
                    return true;
                }
                catch (Exception ex)
                {

                    return true;
                    //AppendLog(ex.Message);
                }
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
            //int ret;
            //IsConnected = true;
            try
            {
                //    ret = MXIO.MXEIO_Init();

                //    DI_hConnection = new int[m_NumOfIOCardInput];
                //    DO_hConnection = new int[m_NumOfIOCardOutput];
                //    dwGetDIValue = new uint[m_NumOfIOCardInput + 1];
                //    dwGetDOValue = new uint[m_NumOfIOCardOutput + 1];

                //    for (int _i = 0; _i < DI_hConnection.Length; _i++)
                //    {
                //        ret = MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DI_IpAddress[_i]), Port, TimeOut, out DI_hConnection[_i], Encoding.UTF8.GetBytes(Password));
                //        if (ret != MXIO.MXIO_OK)
                //        {
                //            Debug.WriteLine("InitMoxaIO, MXEIO_E1K_Connect - return code :" + ret);
                //            IsConnected = false;
                //            Err_Msg = "Connect Input Module Fail\r\n";
                //        }
                //    }

                //    for (int _i = 0; _i < DO_hConnection.Length; _i++)
                //    {
                //        ret = MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DO_IpAddress[_i]), Port, TimeOut, out DO_hConnection[_i], Encoding.UTF8.GetBytes(Password));
                //        if (ret != MXIO.MXIO_OK)
                //        {
                //            Debug.WriteLine("InitMoxaIO, MXEIO_E1K_Connect - return code :" + ret);
                //            IsConnected = false;
                //            Err_Msg = "Connect Output Module Fail\r\n";
                //        }
                //    }

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
            int Slot = (int)slot;
            byte[] byteStatus = new byte[1];

            do
            {
                Thread.Sleep(500);
                if (IsConnected)
                {
                    try
                    {
                        var command = new ModbusCommand(ModbusCommand.FuncReadInputDiscretes) { Offset = 0, Count = 16, TransId = _transactionId++ };
                        var result = _driver.ExecuteGeneric(_portClient, command);
                        if (result.Status == CommResponse.Ack)
                        {
                            command.Data.CopyTo(_registerData, 0);

                            for (int i = 0; i <= 15; i++)
                            {
                                var index = i / 16;
                                var bulbHiByte = (i & 0x0008) != 0;
                                var shift = i & 0x0007;
                                ushort shifter = bulbHiByte ? (ushort)0x0001 : (ushort)0x0100;
                                var mask = Convert.ToUInt16(shifter << shift);
                                InputBitStatus[Slot, i] = (mask & _registerData[index]) != 0;
                            }
                        }
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
            int Slot = (int)slot;
            byte[] byteStatus = new byte[1];
            int val = 15;
            do
            {
                Thread.Sleep(500);
                if (IsConnected)
                {
                    try
                    {
                        var command = new ModbusCommand(ModbusCommand.FuncReadCoils) { Offset = 0, Count = 16, TransId = _transactionId++ };
                        var result = _driver.ExecuteGeneric(_portClient, command);
                        if (result.Status == CommResponse.Ack)
                        {
                            command.Data.CopyTo(_registerData, 0);
                            for (int i = 0; i <= val; i++)
                            {

                                var index = i / 16;
                                var bulbHiByte = (i & 0x0008) != 0;
                                var shift = i & 0x0007;
                                ushort shifter = bulbHiByte ? (ushort)0x0001 : (ushort)0x0100;
                                var mask = Convert.ToUInt16(shifter << shift);
                                OutputBitStatus[Slot, i] = (mask & _registerData[index]) != 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Err_Msg = ex.Message.ToString();
                        return;
                    }
                }
            }
            while (true);
            //int ret;
            //int bitNo = 1;
            //int Slot = (int)slot;
            //byte[] byteStatus = new byte[1];

            //do
            //{
            //    Thread.Sleep(1);
            //    if (IsConnected)
            //    {
            //        try
            //        {
            //            ret = MXIO.E1K_DO_Reads(DO_hConnection[Slot], 0, 16, ref dwGetDOValue[Slot]);
            //            if (ret != MXIO.MXIO_OK)
            //            {
            //                Err_Msg = "ReadMoxaOutput(), m_IsConnected = false.";
            //                MXIO.MXEIO_CheckConnection(DO_hConnection[Slot], TimeOut, byteStatus);
            //                if (byteStatus[0] == 1)
            //                {
            //                    MXIO.MXEIO_E1K_Connect(Encoding.UTF8.GetBytes(m_DO_IpAddress[Slot]), Port, TimeOut, out DO_hConnection[Slot], Encoding.UTF8.GetBytes(Password));
            //                }
            //            }
            //            int val = 15;
            //            //m_MaxDOPortNum[Slot];
            //            for (int i = 0; i <= val; i++)
            //            {
            //                if ((dwGetDOValue[Slot] & bitNo) == bitNo)
            //                {
            //                    OutputBitStatus[Slot, i] = true;
            //                }
            //                else
            //                {
            //                    OutputBitStatus[Slot, i] = false;
            //                }
            //                bitNo *= 2;
            //            }
            //            bitNo = 1;

            //        }
            //        catch (Exception ex)
            //        {
            //            Err_Msg = ex.Message.ToString();
            //            return;
            //        }
            //    }
            //}
            //while (true);
        }

        public void StartScanIO()
        {
            //InitMoxaIO();

            for (int i = 0; i < 1; i++)
            {
                IORefesh[i] = new Thread(ScanMoxaInput);
                IORefesh[i].Start(i);
            }
            for (int i = 0; i < 1; i++)
                //for (int i = 0; i < m_NumOfIOCardOutput; i++)
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
