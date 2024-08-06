using System;
using System.Runtime.InteropServices;

namespace IOManager
{

    /// <summary>
    /// CAdlinkIO - To provide implementation for IBaseIO and IAdlinkIO interfaces.
    /// Based on DLL version 512220523 (ver 3.1) as appear when you click the
    /// About button in HSL LinkMaster editor.
    /// </summary>
    public class AdlinkIO : BaseIO, IAdlinkIO
    {
        #region Dll Imports
        [DllImport("HSL.dll", EntryPoint = "HSL_initial")]
        static extern ushort W_HSL_Initial(ushort cardID);

        [DllImport("HSL.dll", EntryPoint = "HSL_close")]
        static extern ushort W_HSL_Close(ushort cardID);

        [DllImport("HSL.dll", EntryPoint = "HSL_auto_start")]
        static extern ushort W_HSL_Auto_Start(ushort cardID, ushort setID);

        [DllImport("HSL.dll", EntryPoint = "HSL_stop")]
        static extern ushort W_HSL_Stop(ushort cardID, ushort setID);

        [DllImport("HSL.dll", EntryPoint = "HSL_slave_live")]
        static extern ushort W_HSL_Slave_Live(ushort cardID,
            ushort setID,
            ushort slaveNo,
            out byte liveData);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("HSL.dll", EntryPoint = "HSL_D_read_input")]
        static extern ushort W_HSL_DIO_In(ushort cardID,
            ushort setID,
            ushort slaveNo,
            out UInt32 inData);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("HSL.dll", EntryPoint = "HSL_D_read_channel_input")]
        static extern ushort W_HSL_DIO_Channel_In(ushort cardID,
            ushort setID,
            ushort slaveNo,
            ushort bit,
            out ushort inData);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("HSL.dll", EntryPoint = "HSL_D_write_output")]
        static extern ushort W_HSL_DIO_Out(ushort cardID,
            ushort setID,
            ushort slaveNo,
            UInt32 outData);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("HSL.dll", EntryPoint = "HSL_D_write_channel_output")]
        static extern ushort W_HSL_DIO_Channel_Out(ushort cardID,
            ushort setID,
            ushort slaveNo,
            ushort bit,
            ushort outData);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("HSL.dll", EntryPoint = "HSL_D_read_output")]
        static extern ushort HSL_D_read_output(ushort card_ID,
            ushort setID,
            ushort slave_No,
            out UInt32 oData);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("HSL.dll", EntryPoint = "HSL_D_write_all_slave_output")]
        static extern ushort W_HSL_DIO_Memory_Out(ushort cardID,
            ushort setID,
            ushort[] outData);

        // Analog card functions
        [DllImport("HSL.dll")]
        static extern ushort HSL_A_set_signal_range(ushort cardID,
            ushort setID,
            ushort slaveNo,
            ushort signalRange);

        [DllImport("HSL.dll")]
        static extern ushort HSL_A_set_last_channel(ushort cardID,
            ushort setID,
            ushort slaveNo,
            ushort lastChannel);

        [DllImport("HSL.dll")]
        static extern ushort HSL_A_start_read(ushort cardID, ushort setID);

        [DllImport("HSL.dll")]
        static extern ushort HSL_A_stop_read(ushort cardID, ushort setID);

        [DllImport("HSL.dll")]
        static extern ushort HSL_A_read_input(ushort cardID, ushort setID,
            ushort slaveNo, ushort AIChan, out double AIData);

        [DllImport("HSL.dll")]
        static extern ushort HSL_A_write_output(ushort cardID, ushort setID,
            ushort slaveNo, ushort AOChan, double AOData);

        #endregion

        enum ERRCODE
        {
            ERR_NoError,                //0
            ERR_BoardNoInit,            //1
            ERR_InvalidBoardNumber,     //2
            ERR_PCIBiosNotExist,        //3
            ERR_OpenDriverFail,         //4
            ERR_MemoryMapping,          //5
            ERR_ConnectIndex,           //6
            ERR_SatelliteNumber,        //7
            ERR_CountNumber,            //8
            ERR_SatelliteType,          //9
            ERR_NotADLinkSlaveType,     //10
            ERR_ChannelNumber,          //11
            ERR_OverMaxAddress,         //12
            ERR_AIRange,                //13
            ERR_AISignalType,           //14
            ERR_AICJCStatus,            //15
            ERR_CJCDirection,           //16
            ERR_Timeout,                //17
            ERR_CreateTimer,            //18
            ERR_PIDCreateFailed,        //19
            ERR_PIDStartFailed,         //20
            ERR_PIDNoOutput,            //21
            ERR_PIDNoFeedBack,          //22
            ERR_NoPIDController,        //23
            ERROR_LogicInput,           //24
            ERROR_OS_UNKNOWN,           //25
            ERROR_AI16AO2_SignalRangeError,     //26
            ERROR_AI16AO2_ReadError,            //27
            ERROR_AI16AO2_LastChannelError,     //28
            ERROR_AI16AO2_SetDataError,     //29
            ERROR_Read_Signal_Type,         //30
            ERROR_AO_Channel_Input,         //31
            ERROR_AI_Channel_Input,         //32
            ERROR_DA_Channel_Input,         //33
            ERROR_Over_Voltage_Spec,        //34
            ERROR_File_Open_Fail,           //35
            ERROR_TrimDAC_Channel,          //36
            ERR_Over_Current_Spec,        //37
            ERR_Axis_Out_Of_Range,			//38
            ERR_Send_Motion_Command,		//39
            ERR_Read_Motion_HexFile,		//40
            ERR_Flash_Data_Transfer,		//41
            ERR_Unkown_Data_Type,			//42
            ERR_CheckSum,					//43
            ERR_Point_Index,				//44
            ERR_DI_Channel_Input,			//45
            ERR_DO_Channel_Output,			//46
            ERR_No_GCode,					//47
            ERR_Code_Syntax,				//48
            ERR_Read_GC_TexTFile,			//49
            ERR_No_Motion_Module,			//50
            ERR_Owner_Set,					//51
            ERR_Signal_Notify,				//52
            ERR_Communication_Type_Range,	//53
            ERR_Transfer_Rate,				//54
            ERR_Hub_Number,					//55
            ERR_Slave_Number,				//56
            ERR_Slave_Not_Stop,				//57
        };

        #region Variables
        private ushort m_MaxBitSize = 0;
        private double m_AIData = 0;
        private ushort m_Num_IO_Card = 1;
        private ushort m_Max_Port_Num = 1;
        private ushort[] m_Num_Of_SetID;
        #endregion Variables

        #region Constructor
        public AdlinkIO()
        {
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        ///Initialize the hardware and software states of an HSL master
        ///card(PCI(PMC)-7851 or PCI(PMC)-7852), and then return a status
        ///that corresponds to the card initialized. HSL_initial() must be called
        ///before any other HSL DLL functions can be called for the card. The
        ///function initializes the card and variables internal to HSL DLL.
        ///Because HSL master card meets the plug-and-play design, the
        ///base address and IRQ level are assigned by BIOS directly.
        /// </summary>
        /// <returns></returns>
        public override bool OpenDevice()
        {
            // Create the output array
            m_MaxBitSize = (ushort)(Max_Bit_PerPort * m_Max_Port_Num);
            for (int i = 0; i < m_Num_IO_Card; i++)
            {
                m_RetCode = W_HSL_Initial((ushort)i);
                if (m_RetCode > 0)
                {
                    // Error Detected
                    IsConnected = false;
                    return EvalRetCode(m_RetCode);
                }
                for (int j = 0; j < m_Num_Of_SetID[i]; j++)
                {
                    //This function is used to auto-detect the total connect slave I/O
                    //module numbers of the HSL master card (PCI(PMC)-7851 or
                    //PCI(PMC)-7852) with connect_index and start to scan these slave
                    //I/O modules.
                    m_RetCode = W_HSL_Auto_Start((ushort)i, (ushort)j);
                    if (m_RetCode > 0)
                    {
                        // Error Detected
                        IsConnected = false;
                        return EvalRetCode(m_RetCode);
                    }
                }
            }

            IsConnected = true;
            return true;
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
            for (int i = 0; i < m_Num_IO_Card; i++)
            {
                for (int j = 0; j < m_Num_Of_SetID[i]; j++)
                {
                    m_RetCode = W_HSL_Stop((ushort)i, (ushort)j);
                    if (m_RetCode > 0)
                    {
                        // Error Detected
                        return EvalRetCode(m_RetCode);
                    }
                }
                m_RetCode = W_HSL_Close((ushort)i);
                if (m_RetCode > 0)
                {
                    // Error Detected
                    return EvalRetCode(m_RetCode);
                }
            }
            return true;
        }

        /// <summary>
        /// This fucntion write zero value to all the output modules.
        /// </summary>
        /// <returns></returns>
        public override bool ClearOutput()
        {
            lock (this)
            {
                // Clear all output
                ushort[] _oData = new ushort[63];
                for (int i = 0; i < m_Num_IO_Card; i++)
                {
                    for (int j = 0; j < m_Num_Of_SetID[i]; j++)
                    {
                        m_RetCode = W_HSL_DIO_Memory_Out((ushort)i, (ushort)j, _oData);
                        if (m_RetCode > 0)
                        {
                            return EvalRetCode(m_RetCode);
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// This function is to read the digital input value of the specified
        /// channel/bit on the slave I/O module.
        /// </summary>
        /// <param name="bit">Value from IN enum</param>
        /// <returns></returns>
        public override bool ReadInBit(ushort? bit)
        {
            lock (this)
            {
                if (bit == null)
                {
                    return false;
                }
                ushort _inData = 0;
                DecodeDIOInfo((ushort)bit, ref m_iInfo);
                m_RetCode = W_HSL_DIO_Channel_In(m_iInfo.cardID,
                    m_iInfo.setID,
                    m_iInfo.slaveNo,
                    m_iInfo.bit,
                    out _inData);
                EvalRetCode(m_RetCode);
                return Convert.ToBoolean(_inData);
            }
        }

        /// <summary>
        /// This function is to read the digital input value of the specified
        /// channel/bit on the slave I/O module.
        /// </summary>
        /// <param name="bit">Value from IN enum</param>
        /// <param name="invert">Invert the input result</param>
        /// <returns></returns>
        public override bool ReadInBit(ushort? bit, bool invert)
        {
            if (invert)
            {
                return !ReadInBit(bit);
            }
            else
            {
                return ReadInBit(bit);
            }
        }

        /// <summary>
        /// This function reads the output status in the card's RAM
        /// </summary>
        /// <param name="bit">Value from OUT enum</param>
        /// <returns></returns>
        public override bool ReadOutBit(ushort? bit)
        {
            lock (this)
            {
                if (bit == null)
                {
                    return false;
                }
                uint _oData = 0;
                uint _mask = 1;
                DecodeDIOInfo((ushort)bit, ref m_DOInfo);
                m_RetCode = HSL_D_read_output(m_DOInfo.cardID,
                    m_DOInfo.setID,
                    m_DOInfo.slaveNo,
                    out _oData);
                EvalRetCode(m_RetCode);
                return System.Convert.ToBoolean(_oData & (_mask << m_DOInfo.bit));
            }
        }
        /// <summary>
        /// This function is to write the digital output value to the specified
        /// digital channel of slave I/O module.
        /// </summary>
        /// <param name="bit">Value from OUT enum</param>
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
                DecodeDIOInfo((ushort)bit, ref m_oInfo);
                m_RetCode = W_HSL_DIO_Channel_Out(m_oInfo.cardID,
                    m_oInfo.setID,
                    m_oInfo.slaveNo,
                    m_oInfo.bit,
                    state);

                return EvalRetCode(m_RetCode);
            }
        }

        /// <summary>
        /// This function is to read the digital input value of the slave I/O module.
        /// </summary>
        /// <param name="startBitNum">The first bit in the slave module</param>
        /// <returns></returns>
        public override uint ReadPort(ushort startBitNum)
        {
            lock (this)
            {
                uint _iData = 0;
                DecodeDIOInfo(startBitNum, ref m_iInfo);
                m_RetCode = W_HSL_DIO_In(m_iInfo.cardID,
                    m_iInfo.setID,
                    m_iInfo.slaveNo,
                    out _iData);
                EvalRetCode(m_RetCode);
                return _iData;
            }
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
            lock (this)
            {
                DecodeDIOInfo(startBitNum, ref m_oInfo);
                m_RetCode = W_HSL_DIO_Out(m_oInfo.cardID,
                    m_oInfo.setID,
                    m_oInfo.slaveNo,
                    oData);
                return EvalRetCode(m_RetCode);
            }
        }

        // Analog Card Functions
        /// <summary>
        /// This function is used to set the input range of the specified HSL AIO
        ///	modules.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="signalRange"></param>
        /// <returns></returns>
        bool IAdlinkIO.SetSignalRange(ushort cardID, ushort setID, ushort slaveNo, ushort signalRange)
        {
            m_RetCode = HSL_A_set_signal_range(cardID, setID, slaveNo, signalRange);
            return EvalRetCode(m_RetCode);
        }

        /// <summary>
        /// This function is used to set the last of analog input channels about
        /// HSL AIO modules.
        /// Example
        /// HSL_A_set_last_channel(0, 0, 1, 5);
        /// The last channel is set to be 5, it means that analog input channels
        ///	0 to 5 are enabled and the others are disabled.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="lastChannel"></param>
        /// <returns></returns>
        bool IAdlinkIO.SetLastChannel(ushort cardID, ushort setID, ushort slaveNo, ushort lastChannel)
        {
            m_RetCode = HSL_A_set_last_channel(cardID, setID, slaveNo, lastChannel);
            return EvalRetCode(m_RetCode);
        }

        /// <summary>
        /// This function is used to initialize the Analog I/O channels reading
        /// operation of all HSL AIO modules.
        /// Before using HSL_A_read_input(), HSL_A_write_output() and
        /// HSL_A_sync_rw() functions to acquire the AIO channel value,
        /// please perform HSL_A_start_read() function to initialize the
        ///	initialization task.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <returns></returns>
        bool IAdlinkIO.StartRead(ushort cardID, ushort setID)
        {
            m_RetCode = HSL_A_start_read(cardID, setID);
            return EvalRetCode(m_RetCode);
        }

        /// <summary>
        /// This function is used to stop the Analog I/O channel reading
        /// operation of all HSL AIO modules which connect to the controller
        ///	with the connect_index and card_ID.
        /// When you want to stop the AI channels value acquisition task,
        /// please use W_HSL_AI_Stopt_Read to stop.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <returns></returns>
        bool IAdlinkIO.StopRead(ushort cardID, ushort setID)
        {
            m_RetCode = HSL_A_stop_read(cardID, setID);
            return EvalRetCode(m_RetCode);
        }

        /// <summary>
        /// This function is used to read the specified AI channel on the slave
        /// module.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="inChan"></param>
        /// <returns></returns>
        bool IAdlinkIO.ReadAI(ushort cardID, ushort setID, ushort slaveNo, ushort inChan)
        {
            m_RetCode = HSL_A_read_input(cardID, setID, slaveNo, inChan, out m_AIData);
            return EvalRetCode(m_RetCode);
        }

        /// <summary>
        /// This function is used to write analog output data to HSL AIO module.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="outChan"></param>
        /// <param name="outValue"></param>
        /// <returns></returns>
        bool IAdlinkIO.WriteAO(ushort cardID, ushort setID, ushort slaveNo, ushort outChan, double outValue)
        {
            m_RetCode = HSL_A_write_output(cardID, setID, slaveNo, outChan, outValue);
            return EvalRetCode(m_RetCode);
        }

        private string GetLastErr(int errCode)
        {
            string _errMsg = "Error Code # " + errCode.ToString();

            switch ((ERRCODE)errCode)
            {
                case ERRCODE.ERR_NoError:
                    break;

                case ERRCODE.ERR_AICJCStatus:
                    _errMsg = _errMsg + " - Analog Module CJC Status Error";
                    break;

                case ERRCODE.ERR_AIRange:
                    _errMsg = _errMsg + " - Invalid Analog Input Range.";
                    break;

                case ERRCODE.ERR_AISignalType:
                    _errMsg = _errMsg + " - Invalid AI Signal Type";
                    break;

                case ERRCODE.ERR_BoardNoInit:
                    _errMsg = _errMsg + " - Board is not initialized.";
                    break;

                case ERRCODE.ERR_ChannelNumber:
                    _errMsg = _errMsg + " - Invalid AI Channel Number.";
                    break;

                case ERRCODE.ERR_CJCDirection:
                    _errMsg = _errMsg + " - Wrong AI CJC Direction.";
                    break;

                case ERRCODE.ERR_ConnectIndex:
                    _errMsg = _errMsg + " - Invalid Index number for the connected modules.";
                    break;

                case ERRCODE.ERR_CountNumber:
                    _errMsg = _errMsg + " - Invalid Count Number.";
                    break;

                case ERRCODE.ERR_CreateTimer:
                    _errMsg = _errMsg + " - Fail to create timer.";
                    break;

                case ERRCODE.ERR_InvalidBoardNumber:
                    _errMsg = _errMsg + " - Invalid Board Number.";
                    break;

                case ERRCODE.ERR_MemoryMapping:
                    _errMsg = _errMsg + " - Memory Mapping Error.";
                    break;

                case ERRCODE.ERR_NoPIDController:
                    _errMsg = _errMsg + " - Can't find PID Controller.";
                    break;

                case ERRCODE.ERR_NotADLinkSlaveType:
                    _errMsg = _errMsg + " - Not an ADLink Slave Type.";
                    break;

                case ERRCODE.ERR_OpenDriverFail:
                    _errMsg = _errMsg + " - Fail to open driver.";
                    break;

                case ERRCODE.ERR_OverMaxAddress:
                    _errMsg = _errMsg + " - Over the maximum allowable Address assignment.";
                    break;

                case ERRCODE.ERR_PCIBiosNotExist:
                    _errMsg = _errMsg + " - PCI Bios Not Exist.";
                    break;

                case ERRCODE.ERR_PIDCreateFailed:
                    _errMsg = _errMsg + " - Fail to create PID.";
                    break;

                case ERRCODE.ERR_PIDNoFeedBack:
                    _errMsg = _errMsg + " - No feedback from PID.";
                    break;

                case ERRCODE.ERR_PIDNoOutput:
                    _errMsg = _errMsg + " - No output from PID.";
                    break;

                case ERRCODE.ERR_PIDStartFailed:
                    _errMsg = _errMsg + " - Fail to start PID.";
                    break;

                case ERRCODE.ERR_SatelliteNumber:
                    _errMsg = _errMsg + " - Invalid Satellite Number.";
                    break;

                case ERRCODE.ERR_SatelliteType:
                    _errMsg = _errMsg + " - Invlaid Satellite Type.";
                    break;

                case ERRCODE.ERR_Timeout:
                    _errMsg = _errMsg + " - Time Out Error.";
                    break;

                case ERRCODE.ERROR_AI16AO2_LastChannelError:
                    _errMsg = _errMsg + " - AI16AO2 Last Channel Assignment Error.";
                    break;

                case ERRCODE.ERROR_AI16AO2_ReadError:
                    _errMsg = _errMsg + " - AI16AO2 Read Error.";
                    break;

                case ERRCODE.ERROR_AI16AO2_SetDataError:
                    _errMsg = _errMsg + " - AI16AO2 Set Data Error.";
                    break;

                case ERRCODE.ERROR_AI16AO2_SignalRangeError:
                    _errMsg = _errMsg + " - AI16AO2 Signal Range Error.";
                    break;

                case ERRCODE.ERROR_AI_Channel_Input:
                    _errMsg = _errMsg + " - AI Channel Input Error.";
                    break;

                case ERRCODE.ERROR_AO_Channel_Input:
                    _errMsg = _errMsg + " - AO Channel Input Error.";
                    break;

                case ERRCODE.ERROR_DA_Channel_Input:
                    _errMsg = _errMsg + " - DA Channel Input Error.";
                    break;

                case ERRCODE.ERROR_File_Open_Fail:
                    _errMsg = _errMsg + " - Fail to open file.";
                    break;

                case ERRCODE.ERROR_LogicInput:
                    _errMsg = _errMsg + " - Logic Input Error.";
                    break;

                case ERRCODE.ERROR_OS_UNKNOWN:
                    _errMsg = _errMsg + " - Unknown OS Error.";
                    break;

                case ERRCODE.ERR_Over_Current_Spec:
                    _errMsg = _errMsg + " - The set current is over the specification.";
                    break;

                case ERRCODE.ERROR_Read_Signal_Type:
                    _errMsg = _errMsg + " - Read Signal Type Error.";
                    break;

                case ERRCODE.ERROR_TrimDAC_Channel:
                    _errMsg = _errMsg + " - Trim DAC Channel Error.";
                    break;

                case ERRCODE.ERR_Axis_Out_Of_Range:         //38
                    _errMsg = _errMsg + " - Axis Out of Range.";
                    break;

                case ERRCODE.ERR_Send_Motion_Command:       //39
                    _errMsg = _errMsg + " - Send Motion Command Error.";
                    break;

                case ERRCODE.ERR_Read_Motion_HexFile:       //40
                    _errMsg = _errMsg + " - Read Motion HexFile Error.";
                    break;

                case ERRCODE.ERR_Flash_Data_Transfer:       //41
                    _errMsg = _errMsg + " - Flash Data Transfer Error.";
                    break;

                case ERRCODE.ERR_Unkown_Data_Type:          //42
                    _errMsg = _errMsg + " - Unknown Data Type.";
                    break;

                case ERRCODE.ERR_CheckSum:                  //43
                    _errMsg = _errMsg + " - Check Sum Error.";
                    break;

                case ERRCODE.ERR_Point_Index:               //44
                    _errMsg = _errMsg + " - Index Point Error.";
                    break;

                case ERRCODE.ERR_DI_Channel_Input:         //45
                    _errMsg = _errMsg + " - Digital Input Channel Error.";
                    break;

                case ERRCODE.ERR_DO_Channel_Output:         //46
                    _errMsg = _errMsg + " - Digital Output Channel Error.";
                    break;

                case ERRCODE.ERR_No_GCode:                  //47
                    _errMsg = errCode.ToString() + " - No GCode.";
                    break;

                case ERRCODE.ERR_Code_Syntax:               //48
                    _errMsg = _errMsg + " - Code Syntax Error.";
                    break;

                case ERRCODE.ERR_Read_GC_TexTFile:          //49
                    _errMsg = _errMsg + " - Read GC Text File Error.";
                    break;

                case ERRCODE.ERR_No_Motion_Module:          //50
                    _errMsg = _errMsg + " - No Motion Module Error.";
                    break;

                case ERRCODE.ERR_Owner_Set:                 //51
                    _errMsg = _errMsg + " - Owner Set Error.";
                    break;

                case ERRCODE.ERR_Signal_Notify:             //52
                    _errMsg = _errMsg + " - Signal Notification Error.";
                    break;

                case ERRCODE.ERR_Communication_Type_Range:  //53
                    _errMsg = _errMsg + " - Communication Type Range Error.";
                    break;

                case ERRCODE.ERR_Transfer_Rate:             //54
                    _errMsg = _errMsg + " - Transfer Rate Error.";
                    break;

                case ERRCODE.ERR_Hub_Number:                //55
                    _errMsg = errCode.ToString() + " - Hub Number Error.";
                    break;

                case ERRCODE.ERR_Slave_Number:                //56
                    _errMsg = _errMsg + " - Slave Number Error.";
                    break;

                case ERRCODE.ERR_Slave_Not_Stop:             //57
                    _errMsg = _errMsg + " - Slave Not Yet Stop.";
                    break;

                default:
                    _errMsg = _errMsg + " - Undefined Error Code. " +
                        "Refer to HSL Manual for more information.";
                    break;

            }
            return _errMsg;
        }

        private bool EvalRetCode(int retCode)
        {
            if (retCode == 0)
            {
                return true;
            }
            else
            {
                Err_Msg = GetLastErr(m_RetCode);
                return false;
            }
        }

        private void DecodeDIOInfo(ushort bit, ref tagDIOInfo lpDioInfo)
        {
            // --------------------------------------------------------------------------
            // MaxDIOPort should be = 1 and each port could be connected with 32 slaves.
            // Total Slaves per HSL master = 63 (not 64 because 0 is reserved).
            // m_MaxBitSize will be actually the max number of bit per slave module.
            // What needed to be computed is the slave no and bit no (channel no).
            // For adlink function, slave no starts with 1. However, the user will still
            // configure the io assignment with slave no as zero base index.
            // For slave no assignment, take 128 as a factor.
            //
            // Example
            // --------
            // iNum = 38
            // MaxBitSize = 16
            // bit_no = iNum / MaxBitSize
            //        = 38 / 16
            //        = 2 (Quotient) | 6 (Remainder)
            //        = 6 (Remainder)
            // 
            // slave_no = 2
            // 
            // iNum = 2080
            // MaxBitSize = 32
            // bit_no = 2209 / 32
            //        = 69 (Quotien - Slave No) | 1 (Remainder - Bit No)
            // 69 / 128 = 0 (Quotien - Card ID)
            // 69 / 64 = 1 (Quotient)
            // 1 / 2 = 0 (Quotien) | 1 (Remainder - Set ID)
            // Set ID > 0
            // ----------
            // 69 / 64 = 1; 5 (Remainder - Slave No - recalculate)
            // -------------------------------------------------------------------------
            int _quotient = 0, _remainder = 0;
            _quotient = Math.DivRem(bit, m_MaxBitSize, out _remainder);
            // Get the Bit data.
            lpDioInfo.bit = (ushort)_remainder;
            // Get the Slave No.
            lpDioInfo.slaveNo = (ushort)_quotient;
            // Get the CardID data.
            _quotient = Math.DivRem(lpDioInfo.slaveNo, 128, out _remainder);
            lpDioInfo.cardID = (ushort)_quotient;
            // Get the Set ID.
            _quotient = Math.DivRem(lpDioInfo.slaveNo, 64, out _remainder);
            Math.DivRem(_quotient, 2, out _remainder);
            lpDioInfo.setID = (ushort)_remainder;

            if (lpDioInfo.setID > 0)
            {
                // Since adlink funciton only recognise the slave no (1-63),
                // then for the 2nd set id, the slave no (65-127)need to be changed as well.
                _quotient = Math.DivRem((int)lpDioInfo.slaveNo, 64, out _remainder);
                lpDioInfo.slaveNo = (ushort)_remainder;
            }
        }

        #endregion Methods

        #region Properties
        double IAdlinkIO.AI_Data
        {
            get { return m_AIData; }
        }
        ushort IAdlinkIO.Num_IO_Card
        {
            set
            {
                m_Num_IO_Card = value;
                m_Num_Of_SetID = new ushort[m_Num_IO_Card];
            }
            get { return m_Num_IO_Card; }
        }
        ushort IAdlinkIO.Max_Port_Num
        {
            set { m_Max_Port_Num = value; }
            get { return m_Max_Port_Num; }
        }
        ushort[] IAdlinkIO.Num_Of_SetID
        {
            set { m_Num_Of_SetID = value; }
            get { return m_Num_Of_SetID; }
        }
        #endregion Properties
    }
}
