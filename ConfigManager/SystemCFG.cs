using System.Configuration;
using System.Diagnostics;

namespace ConfigManager
{
    public class SystemCFG : BaseCFG
    {
        #region Variables
        private static SystemCFG m_Instance = null;
        private static Configuration m_Config = null;
        private static readonly string SectionName = "System_Config";
        #endregion Variables

        #region Methods
        public static SystemCFG NewOpen(string Path)
        {
            if (m_Instance == null)
            {
                m_Instance = Open(typeof(SystemCFG).Name, Path, SectionName, out m_Config) as SystemCFG;
            }
            Debug.Assert(m_Instance != null);
            return m_Instance;
        }

        public static SystemCFG Open()
        {
            // Once we have gotten the instance, other class can obtain the same instance without passing in the path.
            // Don't have provision to open default config since we do not know which one to refer.
            Debug.Assert(m_Instance != null);
            return m_Instance;
        }

        public void Save()
        {
            Debug.Assert(m_Config != null);
            if (m_Config == null)
            {
                return;
            }
            m_Config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection(SectionName);
        }
        #endregion Methods

        #region Propterties
        [ConfigurationProperty("Regional")]
        public Regional Regional
        {
            set { this["Regional"] = value; }
            get { return (Regional)this["Regional"]; }
        }

        [ConfigurationProperty("ErrLibCfg")]
        public ErrLibCfgCollection ErrLibCfg
        {
            set { this["ErrLibCfg"] = value; }
            get { return (ErrLibCfgCollection)this["ErrLibCfg"]; }
        }

        [ConfigurationProperty("Machine")]
        public Machine Machine
        {
            set { this["Machine"] = value; }
            get { return (Machine)this["Machine"]; }
        }

        [ConfigurationProperty("DigitalIO")]
        public DigitalIO DigitalIO
        {
            set { this["DigitalIO"] = value; }
            get { return (DigitalIO)this["DigitalIO"]; }
        }

        [ConfigurationProperty("IOCards")]
        public IOCardCollection IOCards
        {
            set { this["IOCards"] = value; }
            get { return (IOCardCollection)this["IOCards"]; }
        }

        [ConfigurationProperty("IOInDevices")]
        public IODeviceCollection IOInDevices
        {
            set { this["IOInDevices"] = value; }
            get { return (IODeviceCollection)this["IOInDevices"]; }
        }

        [ConfigurationProperty("IOOutDevices")]
        public IODeviceCollection IOOutDevices
        {
            set { this["IOOutDevices"] = value; }
            get { return (IODeviceCollection)this["IOOutDevices"]; }
        }

        [ConfigurationProperty("Motion")]
        public Motion Motion
        {
            set { this["Motion"] = value; }
            get { return (Motion)this["Motion"]; }
        }

        [ConfigurationProperty("MotionCards")]
        public MotionCardCollection MotionCards
        {
            set { this["MotionCards"] = value; }
            get { return (MotionCardCollection)this["MotionCards"]; }
        }

        [ConfigurationProperty("ACSMotionControllers")]
        public ACSMotionControllerCollection ACSMotionControllers
        {
            set { this["ACSMotionControllers"] = value; }
            get { return (ACSMotionControllerCollection)this["ACSMotionControllers"]; }
        }

        [ConfigurationProperty("AdlinkMotion")]
        public AdlinkMotion AdlinkMotion
        {
            set { this["AdlinkMotion"] = value; }
            get { return (AdlinkMotion)this["AdlinkMotion"]; }
        }

        [ConfigurationProperty("Database")]
        public Database Database
        {
            set { this["Database"] = value; }
            get { return (Database)this["Database"]; }
        }

        [ConfigurationProperty("FileLogger")]
        public FileLogger FileLogger
        {
            set { this["FileLogger"] = value; }
            get { return (FileLogger)this["FileLogger"]; }
        }

        [ConfigurationProperty("FileLoggerFiles")]
        public FileLoggerCollection FileLoggerFiles
        {
            set { this["FileLoggerFiles"] = value; }
            get { return (FileLoggerCollection)this["FileLoggerFiles"]; }
        }

        [ConfigurationProperty("SerialPort")]
        public SerialPort SerialPort
        {
            set { this["SerialPort"] = value; }
            get { return (SerialPort)this["SerialPort"]; }
        }

        [ConfigurationProperty("SerialPortConnection")]
        public SerialPortConnectionCollection SerialPortConnection
        {
            set { this["SerialPortConnection"] = value; }
            get { return (SerialPortConnectionCollection)this["SerialPortConnection"]; }
        }

        [ConfigurationProperty("TCPIP")]
        public TCPIP TCPIP
        {
            set { this["TCPIP"] = value; }
            get { return (TCPIP)this["TCPIP"]; }
        }

        [ConfigurationProperty("TCPIPConnection")]
        public TCPIPConnectionCollection TCPIPConnection
        {
            set { this["TCPIPConnection"] = value; }
            get { return (TCPIPConnectionCollection)this["TCPIPConnection"]; }
        }

        [ConfigurationProperty("MotionCfg")]
        public MotionCfg MotionCfg
        {
            set { this["MotionCfg"] = value; }
            get { return (MotionCfg)this["MotionCfg"]; }
        }

        [ConfigurationProperty("TeachPointCfg")]
        public TeachPointCfg TeachPointCfg
        {
            set { this["TeachPointCfg"] = value; }
            get { return (TeachPointCfg)this["TeachPointCfg"]; }
        }

        [ConfigurationProperty("Setting")]
        public Setting Setting
        {
            set { this["Setting"] = value; }
            get { return (Setting)this["Setting"]; }
        }
        #endregion Propterties
    }

    public class Regional : ConfigurationElement
    {
        [ConfigurationProperty("Language", IsRequired = true)]
        public string Language
        {
            set { this["Language"] = value; }
            get { return (string)this["Language"]; }
        }
    }
    public class ErrLibCfg : ConfigurationElement
    {
        #region Err Lib Cfg 
        public ErrLibCfg()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("ErrLibEnUS", DefaultValue = "", IsRequired = false)]
        public string ErrLibEnUS
        {
            set { this["ErrLibEnUS"] = value; }
            get { return (string)this["ErrLibEnUS"]; }
        }

        [ConfigurationProperty("ErrLibZhHans", DefaultValue = "", IsRequired = false)]
        public string ErrLibZhHans
        {
            set { this["ErrLibZhHans"] = value; }
            get { return (string)this["ErrLibZhHans"]; }
        }

        [ConfigurationProperty("ErrLibZhHant", DefaultValue = "", IsRequired = false)]
        public string ErrLibZhHant
        {
            set { this["ErrLibZhHant"] = value; }
            get { return (string)this["ErrLibZhHant"]; }
        }

        #endregion Err Lib Cfg
    }
    public class ErrLibCfgCollection : ConfigurationElementCollection
    {
        #region Err Lib Cfg Collection
        public ErrLibCfg this[int idx]
        {
            get { return BaseGet(idx) as ErrLibCfg; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ErrLibCfg();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ErrLibCfg)element).ID;
        }
        #endregion Err Lib Cfg Collection
    }
    public class Machine : ConfigurationElement
    {
        #region Machine
        [ConfigurationProperty("MachineName", IsRequired = true)]
        public string MachineName
        {
            set { this["MachineName"] = value; }
            get { return (string)this["MachineName"]; }
        }

        [ConfigurationProperty("SoftwareVersion", IsRequired = true)]
        public string SoftwareVersion
        {
            set { this["SoftwareVersion"] = value; }
            get { return (string)this["SoftwareVersion"]; }
        }

        [ConfigurationProperty("Copyright", IsRequired = true)]
        public string Copyright
        {
            set { this["Copyright"] = value; }
            get { return (string)this["Copyright"]; }
        }

        [ConfigurationProperty("CompanyName", IsRequired = true)]
        public string CompanyName
        {
            set { this["CompanyName"] = value; }
            get { return (string)this["CompanyName"]; }
        }

        [ConfigurationProperty("CompanyAddress", IsRequired = true)]
        public string CompanyAddress
        {
            set { this["CompanyAddress"] = value; }
            get { return (string)this["CompanyAddress"]; }
        }

        [ConfigurationProperty("CompanyTel", IsRequired = true)]
        public string CompanyTel
        {
            set { this["CompanyTel"] = value; }
            get { return (string)this["CompanyTel"]; }
        }

        [ConfigurationProperty("CompanyFax", IsRequired = true)]
        public string CompanyFax
        {
            set { this["CompanyFax"] = value; }
            get { return (string)this["CompanyFax"]; }
        }

        [ConfigurationProperty("CompanyEmail", IsRequired = true)]
        public string CompanyEmail
        {
            set { this["CompanyEmail"] = value; }
            get { return (string)this["CompanyEmail"]; }
        }

        [ConfigurationProperty("CompanyWebsite", IsRequired = true)]
        public string CompanyWebsite
        {
            set { this["CompanyWebsite"] = value; }
            get { return (string)this["CompanyWebsite"]; }
        }

        [ConfigurationProperty("MachineModel", IsRequired = true)]
        public string MachineModel
        {
            set { this["MachineModel"] = value; }
            get { return (string)this["MachineModel"]; }
        }

        [ConfigurationProperty("SerialNo", IsRequired = true)]
        public string SerialNo
        {
            set { this["SerialNo"] = value; }
            get { return (string)this["SerialNo"]; }
        }

        [ConfigurationProperty("MachineBuildDate", IsRequired = true)]
        public string MachineBuildDate
        {
            set { this["MachineBuildDate"] = value; }
            get { return (string)this["MachineBuildDate"]; }
        }

        [ConfigurationProperty("Current", IsRequired = true)]
        public string Current
        {
            set { this["Current"] = value; }
            get { return (string)this["Current"]; }
        }

        [ConfigurationProperty("LineVoltageVAC", IsRequired = true)]
        public string LineVoltageVAC
        {
            set { this["LineVoltageVAC"] = value; }
            get { return (string)this["LineVoltageVAC"]; }
        }

        [ConfigurationProperty("Frequency", IsRequired = true)]
        public string Frequency
        {
            set { this["Frequency"] = value; }
            get { return (string)this["Frequency"]; }
        }

        [ConfigurationProperty("PowerVA", IsRequired = true)]
        public string PowerVA
        {
            set { this["PowerVA"] = value; }
            get { return (string)this["PowerVA"]; }
        }

        [ConfigurationProperty("ContryOfOrigin", IsRequired = true)]
        public string ContryOfOrigin
        {
            set { this["ContryOfOrigin"] = value; }
            get { return (string)this["ContryOfOrigin"]; }
        }
        #endregion Machine
    }
    public class DigitalIO : ConfigurationElement
    {
        #region Digital IO
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }

        [ConfigurationProperty("TotalIOCardNum", DefaultValue = 1, IsRequired = true)]
        public int TotalIOCardNum
        {
            set { this["TotalIOCardNum"] = value; }
            get { return (int)this["TotalIOCardNum"]; }
        }

        [ConfigurationProperty("MaxPortNum", DefaultValue = 2, IsRequired = true)]
        public int MaxPortNum
        {
            set { this["MaxPortNum"] = value; }
            get { return (int)this["MaxPortNum"]; }
        }

        [ConfigurationProperty("MaxBitPerPort", DefaultValue = 8, IsRequired = true)]
        public int MaxBitPerPort
        {
            set { this["MaxBitPerPort"] = value; }
            get { return (int)this["MaxBitPerPort"]; }
        }

        [ConfigurationProperty("InputFileName", DefaultValue = "input Laser.csv", IsRequired = true)]
        public string InputFileName
        {
            set { this["InputFileName"] = value; }
            get { return (string)this["InputFileName"]; }
        }

        [ConfigurationProperty("OutputFileName", DefaultValue = "output Laser.csv", IsRequired = true)]
        public string OutputFileName
        {
            set { this["OutputFileName"] = value; }
            get { return (string)this["OutputFileName"]; }
        }

        #endregion Digital IO
    }
    public class IOCards : ConfigurationElement
    {
        #region IO Cards
        public IOCards()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("NumOfSetID", DefaultValue = 1, IsRequired = true)]
        public int NumOfSetID
        {
            set { this["NumOfSetID"] = value; }
            get { return (int)this["NumOfSetID"]; }
        }
        #endregion IO Cards
    }
    public class IOCardCollection : ConfigurationElementCollection
    {
        #region IO Cards Collection
        public IOCards this[int idx]
        {
            get { return BaseGet(idx) as IOCards; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IOCards();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IOCards)element).ID;
        }
        #endregion IO Cards Collection
    }
    public class IODevices : ConfigurationElement
    {
        #region IO Devices
        public IODevices()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("IOName", DefaultValue = "MXIO Input Card", IsRequired = true)]
        public string IOName
        {
            set { this["Name"] = value; }
            get { return (string)this["IOName"]; }
        }

        [ConfigurationProperty("DeviceAddress", DefaultValue = "192.0.0.10", IsRequired = true)]
        public string DeviceAddress
        {
            set { this["DeviceAddress"] = value; }
            get { return (string)this["DeviceAddress"]; }
        }

        [ConfigurationProperty("MaxPortNum", DefaultValue = "16", IsRequired = false)]
        public int MaxPortNum
        {
            set { this["MaxPortNum"] = value; }
            get { return (int)this["MaxPortNum"]; }
        }
        #endregion IO Devices
    }
    public class IODeviceCollection : ConfigurationElementCollection
    {
        #region IO Devices Collection
        public IODevices this[int idx]
        {
            get { return BaseGet(idx) as IODevices; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IODevices();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IODevices)element).ID;
        }
        #endregion IO Devices Collection
    }
    public class Motion : ConfigurationElement
    {
        #region Motion
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }

        [ConfigurationProperty("TotalAxisNum", DefaultValue = 1, IsRequired = true)]
        public int TotalAxisNum
        {
            set { this["TotalAxisNum"] = value; }
            get { return (int)this["TotalAxisNum"]; }
        }
        #endregion Motion
    }
    public class MotionCard : ConfigurationElement
    {
        #region Motion Card
        public MotionCard()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("CardNo", DefaultValue = 0, IsRequired = true)]
        public int CardNo
        {
            set { this["CardNo"] = value; }
            get { return (int)this["CardNo"]; }
        }

        [ConfigurationProperty("DeviceAddress", DefaultValue = "192.0.0.10", IsRequired = true)]
        public string DeviceAddress
        {
            set { this["DeviceAddress"] = value; }
            get { return (string)this["DeviceAddress"]; }
        }
        #endregion Motion Card
    }
    public class MotionCardCollection : ConfigurationElementCollection
    {
        #region Motion Cards Collection
        public MotionCard this[int idx]
        {
            get { return BaseGet(idx) as MotionCard; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MotionCard();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MotionCard)element).ID;
        }
        #endregion Motion Cards Collection
    }  
    public class ACSMotionController : ConfigurationElement
    {
        #region ACS Motion Controller
        public ACSMotionController()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("IsSimulator", DefaultValue = "False", IsRequired = true)]
        public bool IsSimulator
        {
            set { this["IsSimulator"] = value; }
            get { return (bool)this["IsSimulator"]; }
        }

        [ConfigurationProperty("DeviceAddress", DefaultValue = "192.0.0.100", IsRequired = true)]
        public string DeviceAddress
        {
            set { this["DeviceAddress"] = value; }
            get { return (string)this["DeviceAddress"]; }
        }

        [ConfigurationProperty("Port", DefaultValue = "701", IsRequired = true)]
        public string Port
        {
            set { this["Port"] = value; }
            get { return (string)this["Port"]; }
        }
        #endregion ACS Motion Controller
    }
    public class ACSMotionControllerCollection : ConfigurationElementCollection
    {
        #region ACS Motion Controller Collection
        public ACSMotionController this[int idx]
        {
            get { return BaseGet(idx) as ACSMotionController; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ACSMotionController();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ACSMotionController)element).ID;
        }
        #endregion
    }
    public class AdlinkMotion : ConfigurationElement
    {
        #region Adlink Motion
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }
        #endregion Adlink Motion
    }
    public class Database : ConfigurationElement
    {
        #region Database
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }
        #endregion Database
    }
    public class FileLogger : ConfigurationElement
    {
        #region File Logger
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }

        [ConfigurationProperty("FilePath", IsRequired = true)]
        public string FilePath
        {
            set { this["FilePath"] = value; }
            get { return (string)this["FilePath"]; }
        }
        #endregion File Logger
    }
    public class FileLoggerFiles : ConfigurationElement
    {
        #region File Logger Files
        public FileLoggerFiles()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("FolderName", DefaultValue = "UnNamedFolder", IsRequired = true)]
        public string FolderName
        {
            set { this["FolderName"] = value; }
            get { return (string)this["FolderName"]; }
        }

        [ConfigurationProperty("FileName", DefaultValue = "UnNamedFile", IsRequired = true)]
        public string FileName
        {
            set { this["FileName"] = value; }
            get { return (string)this["FileName"]; }
        }

        [ConfigurationProperty("Header", DefaultValue = "", IsRequired = true)]
        public string Header
        {
            set { this["Header"] = value; }
            get { return (string)this["Header"]; }
        }

        [ConfigurationProperty("FileExtension", DefaultValue = ".log", IsRequired = true)]
        public string FileExtension
        {
            set { this["FileExtension"] = value; }
            get { return (string)this["FileExtension"]; }
        }
        #endregion File Logger Files
    }
    public class FileLoggerCollection : ConfigurationElementCollection
    {
        #region File Logger Collection
        public FileLoggerFiles this[int idx]
        {
            get { return BaseGet(idx) as FileLoggerFiles; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileLoggerFiles();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileLoggerFiles)element).ID;
        }
        #endregion File Logger Collection
    }
    public class SerialPort : ConfigurationElement
    {
        #region SerialPort
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }
        #endregion SerialPort
    }
    public class SerialPortConnection : ConfigurationElement
    {
        #region Serial Port Connection
        public SerialPortConnection()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("DeviceName", DefaultValue = "", IsRequired = true)]
        public string DeviceName
        {
            set { this["DeviceName"] = value; }
            get { return (string)this["DeviceName"]; }
        }

        [ConfigurationProperty("PortName", DefaultValue = "", IsRequired = true)]
        public string PortName
        {
            set { this["PortName"] = value; }
            get { return (string)this["PortName"]; }
        }

        [ConfigurationProperty("Boundrate", DefaultValue = "192000", IsRequired = true)]
        public int Boundrate
        {
            set { this["Boundrate"] = value; }
            get { return (int)this["Boundrate"]; }
        }

        #endregion Serial Port Connection
    }
    public class SerialPortConnectionCollection : ConfigurationElementCollection
    {
        #region Serial Port Connection Collection
        public SerialPortConnection this[int idx]
        {
            get { return BaseGet(idx) as SerialPortConnection; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SerialPortConnection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SerialPortConnection)element).ID;
        }
        #endregion Serial Port Connection Collection
    }
    public class TCPIP : ConfigurationElement
    {
        #region TCPIP
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName
        {
            set { this["ClassName"] = value; }
            get { return (string)this["ClassName"]; }
        }
        #endregion TCPIP
    }
    public class TCPIPConnection : ConfigurationElement
    {
        #region TCPIP Connection 
        public TCPIPConnection()
        {
        }

        [ConfigurationProperty("ID", DefaultValue = 0, IsRequired = true)]
        public int ID
        {
            set { this["ID"] = value; }
            get { return (int)this["ID"]; }
        }

        [ConfigurationProperty("DeviceName", DefaultValue = "", IsRequired = true)]
        public string DeviceName
        {
            set { this["DeviceName"] = value; }
            get { return (string)this["DeviceName"]; }
        }

        [ConfigurationProperty("IPAddress", DefaultValue = "0.0.0.0", IsRequired = true)]
        public string IPAddress
        {
            set { this["IPAddress"] = value; }
            get { return (string)this["IPAddress"]; }
        }

        [ConfigurationProperty("Port", DefaultValue = "0", IsRequired = true)]
        public int Port
        {
            set { this["Port"] = value; }
            get { return (int)this["Port"]; }
        }

        [ConfigurationProperty("IsServer", DefaultValue = "False", IsRequired = true)]
        public bool IsServer
        {
            set { this["IsServer"] = value; }
            get { return (bool)this["IsServer"]; }
        }

        [ConfigurationProperty("AutoReconnect", DefaultValue = "False", IsRequired = true)]
        public bool AutoReconnect
        {
            set { this["AutoReconnect"] = value; }
            get { return (bool)this["AutoReconnect"]; }
        }
        #endregion TCPIP Connection
    }
    public class TCPIPConnectionCollection : ConfigurationElementCollection
    {
        #region TCPIP Connection Collection
        public TCPIPConnection this[int idx]
        {
            get { return BaseGet(idx) as TCPIPConnection; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TCPIPConnection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TCPIPConnection)element).ID;
        }
        #endregion TCPIP Connection Collection
    }
    public class MotionCfg : ConfigurationElement
    {
        #region MotionCFG
        [ConfigurationProperty("FilePath", IsRequired = true)]
        public string FilePath
        {
            set { this["FilePath"] = value; }
            get { return (string)this["FilePath"]; }
        }
        #endregion MotionCFG
    }
    public class TeachPointCfg : ConfigurationElement
    {
        #region TeachPoint
        [ConfigurationProperty("FilePath", IsRequired = true)]
        public string FilePath
        {
            set { this["FilePath"] = value; }
            get { return (string)this["FilePath"]; }
        }
        #endregion TeachPoint
    }

     public class Setting : ConfigurationElement
    {
        #region Setting
        [ConfigurationProperty("EnableVirtualKeyboard", DefaultValue = "False", IsRequired = true)]
        public bool EnableVirtualKeyboard
        {
            set { this["EnableVirtualKeyboard"] = value; }
            get { return (bool)this["EnableVirtualKeyboard"]; }
        }

        [ConfigurationProperty("EnableVirtualKeypad", DefaultValue = "False", IsRequired = true)]
        public bool EnableVirtualKeypad
        {
            set { this["EnableVirtualKeypad"] = value; }
            get { return (bool)this["EnableVirtualKeypad"]; }
        }

        [ConfigurationProperty("LogFileDuration_Days", DefaultValue = 30, IsRequired = true)]
        public int LogFileDuration_Days
        {
            set { this["LogFileDuration_Days"] = value; }
            get { return (int)this["LogFileDuration_Days"]; }
        }
        #endregion Setting
    }

}

