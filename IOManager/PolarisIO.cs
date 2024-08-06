using PolarisCommunication;
using PolarisCommunication.PolarisServerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static PolarisCommunication.PolarisServerAPI.PSU;

namespace IOManager
{
    public class PolarisIO : BaseIO, IPolarisIO
    {
        internal const int MaxCardSlot = 2;
        internal const int MaxBit = 32;
        internal string dev = "DM20017";
        internal bool[,] InputBitStatus = new bool[MaxCardSlot, MaxBit];
        internal bool[,] OutputBitStatus = new bool[MaxCardSlot, MaxBit];
        Thread[] IORefesh = new Thread[MaxCardSlot];
        public Script m_Script { get; set; }

        //private readonly ServerStatusModel _serverStatusModel = new ServerStatusModel();

        public override bool OpenDevice()
        {
            try
            {
                Max_Bit_PerPort = 32;
                m_Script = Script.GetInstance();
                m_Script.Connect("192.168.127.51");
                Script = m_Script;
                IsConnected = true;
                //_serverStatusController = new ServerStatusController(script, _serverStatusModel);
                //script.Connect("192.168.127.51");
                return true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                return false;
            }
        } /// <summary>
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

        bool InitIO()
        {
            int ret;
            IsConnected = true;
            try
            {
                //IsConnected = false;
                return IsConnected;
            }
            catch (Exception ex)
            {
                Err_Msg = ex.Message.ToString();
                return false;
            }
        }

        public bool DeviceIsConnected()
        {
            if (m_Script == null) return false;
            return m_Script.IsConnected;
        }

        public Script Script { get; set; }

        public void Connect()
        {
            try
            {
                if (!m_Script.IsConnected)
                {
                    //IORefesh[0] = new Thread(ScanInput);
                    //IORefesh[1] = new Thread(ScanOutput);
                    m_Script.Connect("192.168.127.51");
                    IsConnected = true;
                    //if (!IORefesh[0].IsAlive)
                    //{
                    //    StartScanIO();
                    //}
                }

            }
            catch (Exception ex)
            {
                IsConnected = false;
                MessageBox.Show(ex.Message);
            }
        }
        public void Disconnect()
        {
            try
            {
                IsConnected = false;
                m_Script.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This fucntion write zero value to all the output modules.
        /// </summary>
        /// <returns></returns>
        public override bool ClearOutput()
        {
            return true;
        }
        public override bool ReadInBit(ushort? bit)
        {
            if (m_Script == null ||! IsConnected)
                return false;
           else if (!m_Script.IsConnected)
                return false;
            else return GetInputBitStatus((int)bit);
        }

        public override bool ReadOutBit(ushort? bit)
        {
            if (m_Script == null ||!IsConnected) return false;
            if (!m_Script.IsConnected) return false;
            return GetOutputBitStatus((int)bit);
        }

        private bool GetOutputBitStatus(int bit)
        {
            lock (this)
            {
                int slot = bit / Max_Bit_PerPort;
                int index = bit % Max_Bit_PerPort;
                return OutputBitStatus[slot, index];
            }
        }

        private bool GetInputBitStatus(int bit)
        {
            lock (this)
            {
                int slot = bit / Max_Bit_PerPort;
                int index = bit % Max_Bit_PerPort;
                return InputBitStatus[slot, index];
            }
        }


        void ScanInput(object slot)
        {
            int Slot = (int)slot;
            byte[] byteStatus = new byte[1];

            do
            {
                Thread.Sleep(1);
                if (m_Script.IsConnected)
                {
                    try
                    {


                        IList<DigitalIODisplayInfo> test = PSU.GetDigitalInputDisplayInfo(m_Script, dev);
                        for (int i = 0; i < 32; i++)
                        {
                            InputBitStatus[Slot, i] = Convert.ToBoolean(test[i].State);
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

        void ScanOutput(object slot)
        {
            int Slot = (int)slot;
            byte[] byteStatus = new byte[1];

            do
            {
                Thread.Sleep(1);
                if (m_Script.IsConnected)
                {
                    try
                    {

                        IList<DigitalIODisplayInfo> test = PSU.GetDigitalOutputDisplayInfo(m_Script, dev);
                        for (int i = 0; i < 32; i++)
                        {
                            OutputBitStatus[Slot, i] = Convert.ToBoolean(test[i].State);
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

        public void StartScanIO()
        {
            //InitIO();
            IORefesh[0] = new Thread(ScanInput);
            IORefesh[0].Start(0);
            IORefesh[1] = new Thread(ScanOutput);
            IORefesh[1].Start(0);
            //for (int i = 0; i < 1; i++)
            //{
            //    IORefesh[i] = new Thread(ScanInput);
            //    IORefesh[i].Start(i);
            //}
            //for (int i = 0; i < 1; i++)
            //{
            //    IORefesh[i] = new Thread(ScanOutput);
            //    IORefesh[i].Start(i);
            //}
        }

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
                //OutputBitStatus[slot, _output] = Convert.ToBoolean(_state);

                SetOutput(slot, _output, _state);
                return true;
            }
        }

        private void SetOutput(int slot, byte bit, uint dwSetDOValue)
        {
            try
            {
                IList<DigitalIODisplayInfo> test = PSU.GetDigitalOutputDisplayInfo(m_Script,dev);
                if (test.Count >= 31)
                {
                    if (test[bit].State == 1)
                    {
                        PSU.ClearDigitalOutputBit(m_Script, dev, bit);   // working
                    }
                    else
                    {
                        PSU.SetDigitalOutputBit(m_Script, dev, bit);
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
}
