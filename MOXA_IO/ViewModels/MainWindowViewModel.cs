using ConfigManager;
using IOManager;
using Prism.Mvvm;
using Core.Facilities;
using Core.Variables;
using System;
using Core.Events;
using Prism.Commands;
using System.Net;
using Core.Enums;
using Prism.Services.Dialogs;
using System.Windows.Threading;
using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Timers;
using MOXA_IO.CustomControls;
using System.IO;
using Library.Model;
using PolarisCommunication;
using Library.Controller;
using System.Threading;
using System.ComponentModel;
using static PolarisCommunication.PolarisServerAPI.PSU;
using PolarisCommunication.PolarisServerAPI;

namespace MOXA_IO.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Variables
        Device _device = new Device();
        public IDialogService m_dialogService;
        private bool searchDone;
        private CustomControls.SplashScreen _splashScreen;
        //private static Timer tmrScheduleTask;


        private IScript _script;
        private ServerStatusController _serverStatusController;

        // We only ever have one of the following Model's; AxisModel and DataSourceModel 
        // we have many of.
        private readonly ServerStatusModel _serverStatusModel = new ServerStatusModel();

        private SynchronizationContext _context;
        private string _ipAddress;
        #endregion Variables

        #region Constructor
        public MainWindowViewModel(IDialogService dialogService)
        {
            if (!IsFirstInstance())
            {
                CustomMsgBox _customMsgBox = new CustomMsgBox();
                _customMsgBox.ShowWindow(new string[] { "Ok" }, "A program already running.", "Warning", System.Drawing.SystemIcons.Warning);

                Environment.Exit(0);
            }
            else
            {
                WindowLoaded = new DelegateCommand(Loaded);
                m_dialogService = dialogService;

                #region Parts Composition

                // --- Configuration --- 
                GlobalVar.systemCFG = SystemCFG.NewOpen(@"..\Config Section\General\System.Config");

                // --- IO --- 
                MapObject<IBaseIO> ioCards = new MapObject<IBaseIO>();
                GlobalVar.IIO = ioCards.CreateObject(GlobalVar.systemCFG.DigitalIO.ClassName);


                // --- File Logger ---
                MapObject<ILogger> logger = new MapObject<ILogger>();
                GlobalVar.ILog = logger.CreateObject(GlobalVar.systemCFG.FileLogger.ClassName);


                // --- Tower Light ---
                //_towerLight = new TowerLight(); 


                #endregion Parts Composition

                _splashScreen = new CustomControls.SplashScreen(GlobalVar.systemCFG.Machine.MachineName, GlobalVar.systemCFG.Machine.SoftwareVersion);
                _splashScreen.Show();

                //in order to ensure the UI splash screen stays responsive, we need to do the work on a different thread
                var startupTask = new Task(() =>
                {
                    #region IO Setting
                    _splashScreen.UpdateStatus("IO", 20);

                    GlobalVar.IIO.Max_Bit_PerPort = (ushort)GlobalVar.systemCFG.DigitalIO.MaxBitPerPort;

                    if (GlobalVar.IIO.GetType() == typeof(MxioModbus))
                    {
                        GlobalVar.IIO.OpenDevice();
                        //(GlobalVar.IIO as IMoxaIO).Num_IO_Card_Input = GlobalVar.systemCFG.IOInDevices.Count;
                        //(GlobalVar.IIO as IMoxaIO).Num_IO_Card_Output = GlobalVar.systemCFG.IOOutDevices.Count;

                        //for (int i = 0; i < GlobalVar.systemCFG.IOInDevices.Count; i++)
                        //{
                        //    (GlobalVar.IIO as IMoxaIO).DI_IpAddress[i] = GlobalVar.systemCFG.IOInDevices[i].DeviceAddress;
                        //    (GlobalVar.IIO as IMoxaIO).MaxDIPortNum[i] = GlobalVar.systemCFG.IOInDevices[i].MaxPortNum;
                        //}
                        //for (int i = 0; i < GlobalVar.systemCFG.IOOutDevices.Count; i++)
                        //{
                        //    (GlobalVar.IIO as IMoxaIO).DO_IpAddress[i] = GlobalVar.systemCFG.IOOutDevices[i].DeviceAddress;
                        //    (GlobalVar.IIO as IMoxaIO).MaxDOPortNum[i] = GlobalVar.systemCFG.IOInDevices[i].MaxPortNum;
                        //}

                        (GlobalVar.IIO as IMoxaIO).StartScanIO();
                    }
                    else if (GlobalVar.IIO.GetType() == typeof(PolarisIO))
                    {
                        SetSelectedController();
                        //_script = Script.GetInstance();
                        if ((GlobalVar.IIO).OpenDevice())
                        {
                            //(GlobalVar.IIO as IPolarisIO).StartScanIO();
                        }
                    }
                    else if (GlobalVar.IIO.GetType() == typeof(AdlinkIO))
                    {
                        (GlobalVar.IIO as IAdlinkIO).Num_IO_Card = (ushort)GlobalVar.systemCFG.DigitalIO.TotalIOCardNum;//for AdlinkIO only
                        for (int i = 0; i < GlobalVar.systemCFG.DigitalIO.TotalIOCardNum; i++)
                        {
                            // Applicable to Adlink Only - refers to the number of master in the card.
                            // For example - PCI-7851 has one master, PCI-7852 has two master.
                            (GlobalVar.IIO as IAdlinkIO).Num_Of_SetID[i] = (ushort)GlobalVar.systemCFG.IOCards[i].NumOfSetID;
                        }
                        (GlobalVar.IIO as IAdlinkIO).Max_Port_Num = (ushort)GlobalVar.systemCFG.DigitalIO.MaxPortNum;
                    }

                    #endregion IO Setting

                    //#region ACS Motion Setting
                    //_splashScreen.UpdateStatus("ACS Controller", 40);
                    //try
                    //{
                    //    if (GlobalVar.systemCFG.ACSMotionControllers[0].IsSimulator)
                    //    {
                    //        TernminateUMD_Connection();
                    //        GlobalVar.ACS.OpenCommSimulator();
                    //    }
                    //    else
                    //    {
                    //        TernminateUMD_Connection();
                    //        GlobalVar.ACS.OpenCommEthernetTCP(GlobalVar.systemCFG.ACSMotionControllers[0].DeviceAddress, Convert.ToInt32(GlobalVar.systemCFG.ACSMotionControllers[0].Port));
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    GlobalVar.ILog.LogData("Failed to Open ACS Controller" + "\n" + ex.Message, (int)LoggerFileList.Error);
                    //}

                    //#endregion ACS Motion Setting

                    _splashScreen.UpdateStatus("Done", 100);
                });

                //when Task loading finished, show main window
                startupTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        var a = t.Exception.Message;
                        GlobalVar.ILog.LogData("Failed to load devices \n" + t.Exception.Message, (int)LoggerFileList.Error);
                        Environment.Exit(0);
                    }
                    else
                    {
                        DeviceConnectivity(); //Get All Connectivity devices
                        _splashScreen.Close(); //Cose SplashScreen
                        MainWindowVisibility = Visibility.Visible; //Open Main Window Screen
                        //ScheduleTask(); //Set Schedule to delete log file
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startupTask.Start();

            }
        }

        #endregion Constructor

        #region Methods

        private void SetSelectedController()
        {
            _ipAddress = "192.168.127.51";
        }

        /// <summary>
        /// Create new Controller objects;     
        /// Note that these constructors do not perform any polaris communication connect mechanisms.
        /// </summary>
        private void InstantiateControllers()
        {
            _serverStatusController = new ServerStatusController(_script, _serverStatusModel);
        }


        /// <summary>
        /// Associate Property Changed Events with local Handlers.
        /// </summary>
        private void AttachEventHandlers()
        {
            _serverStatusModel.PropertyChanged += ServerStatusModelPropertyChanged;

            // should this message be coming from the model? or is it ok to be coming
            // from the controller? It's not a model property  that's being updated
            //_axisController.AxesInitialized += AxisListInitialized;

        }


        /// <summary>
        /// Connects the ServerStatusController. Does not depend on the state of Polaris Server.
        /// The first component of Polaris Communication to initialize.
        /// </summary>
        private void ConnectToPolarisController()
        {
            _serverStatusController.ConnectAndStartStatusMonitor(_ipAddress, _context);
        }

        // todo - this really needs to be in the mainform controller/presenter paw 21feb19
        /// <summary>
        /// Event handler for various properties of the status monitor reflecting the status of the polaris system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerStatusModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PolarisServerStarted":
                    PolarisServerInitializationSequence();
                    break;
            }
        }

        private void PolarisServerInitializationSequence()
        {
            //DisableControls(Controls);

            // Initialize or de-initialize polaris based on the status of PolarisServer.
            if (_serverStatusModel.PolarisServerStarted)
            {
                try
                {
                    _script.Connect(_ipAddress);
                }
                catch (Exception ex)
                {
                    // notify the user that the call to connect the script failed
                    MessageBox.Show($"Failed to connect to {_ipAddress}, the application will close\n{ex}");
                }

                // this is basically injecting the axisModelList dependency on each view
                // and doing a minimal setup in the view - needs to be set up before we
                // call any of these functions
                //_homingController.InitializeAxes();
                //_autophaseController.InitializeAxes();
                //_positionJogController.InitializeAxes();
                //_moveController.InitializeAxes();
                //_axisStatusController.InitializeAxes();
                //_asyncMessagingController.InitializeAxes();

                //// do this one last as it updates axis properties that fire property updated
                //// events that the other controllers are interested in paw 12mar19
                //_axisController.InitializeAxes();

                //EnableControls(Controls);
            }
            else
            {
                PolarisUninitialized();
            }
        }    /// <summary>
             /// Helper method called to update the status monitor controller and the UI that polaris server has stopped.
             /// </summary>
        private void PolarisUninitialized()
        {
            // Updates display with status of polaris server.
            _serverStatusController.GetPolarisServerStatus();

            // Set the flag to denote polaris server has been stopped and polaris components
            // uninitialized.
            _serverStatusController.SetPolarisState(PolarisState.Stopped);
        }


        private void Loaded()
        {
            _context = SynchronizationContext.Current;
           
            
            //InstantiateControllers();

            //AttachEventHandlers();
            //ConnectToPolarisController();
            MainWindowVisibility = Visibility.Collapsed;

            Title = GlobalVar.systemCFG.Machine.MachineName + " Ver. " + GlobalVar.systemCFG.Machine.SoftwareVersion;
        }

        private void DeviceConnectivity()
        {
            //TODO : add/remove the device connetivity

            _device.DeviceName = "IO";
            _device.DeviceConnectivity = GlobalVar.IIO.IsConnected;
            GlobalVar.Publisher_DeviceConnectivity.GetEvent<Device>().Publish(_device);

        }

        private void TernminateUMD_Connection()
        {
            /// <summary>
            /// Terminate connections from SPiiPlus User Mode Driver
            ///  - Maximum connections up to 10 in UMD
            /// </summary>

            //string terminateExceptionConnName = "ACS.Framework.exe";

            //ACSC_CONNECTION_DESC[] connectionList = GlobalVar.ACS.GetConnectionsList();
            //for (int index = 0; index < connectionList.Length; index++)
            //{

            //    if (terminateExceptionConnName.CompareTo((string)connectionList[index].Application) != 0)
            //        GlobalVar.ACS.TerminateConnection(connectionList[index]);
            //}
        }

        private bool IsFirstInstance()
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
                return false;
            }
            return true;
        }

        private void ScheduleTask()
        {
            //DateTime nowTime = DateTime.Now;
            //DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 10, 10, 0, 0); //Specify your scheduled time HH,MM,SS [10am and 10 minutes]
            //if (nowTime > scheduledTime)
            //{
            //    scheduledTime = scheduledTime.AddDays(1);
            //}

            //double tickTime = (scheduledTime - DateTime.Now).TotalMilliseconds;
            //tmrScheduleTask = new Timer(tickTime);
            //tmrScheduleTask.Elapsed += TmrScheduleTask_Elapsed;
            //tmrScheduleTask.Start();
        }

        private void TmrScheduleTask_Elapsed(object sender, ElapsedEventArgs e)
        {
            string FilePath = @GlobalVar.systemCFG.FileLogger.FilePath;
            string[] AllLogFile;
            string[] LogFile1;
            string[] LogFile2;

            try
            {
                //Search log file
                LogFile1 = Directory.GetFiles(FilePath, "*.log", SearchOption.AllDirectories);
                LogFile2 = Directory.GetFiles(FilePath, "*.csv", SearchOption.AllDirectories);

                AllLogFile = LogFile1.Concat(LogFile2).ToArray();//join all log file

                for (int i = 0; i < AllLogFile.Length; i++)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AllLogFile[i]);//Get File Name
                    FileName = FileName.Substring(FileName.Length - 10).Replace('_', '-');
                    DateTime.TryParse(FileName, out DateTime _dateTime);//Convert to date time format
                    int diffDays = DateTime.Now.Subtract(_dateTime).Days;

                    if (diffDays > GlobalVar.systemCFG.Setting.LogFileDuration_Days)
                    {
                        File.Delete(AllLogFile[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVar.ILog.LogData("Failed to delete log file" + "\n" + ex.Message, (int)LoggerFileList.Error);
            }
        }

        #endregion Methods

        #region Properties
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Visibility _mainWindowVisibility = Visibility.Collapsed;
        public Visibility MainWindowVisibility
        {
            get { return _mainWindowVisibility; }
            set { SetProperty(ref _mainWindowVisibility, value); }
        }

        public DelegateCommand WindowLoaded { get; private set; }

        #endregion Properties
    }
}
