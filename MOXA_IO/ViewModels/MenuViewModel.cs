using Core.Enums;
using Core.Events;
using Core.Variables;
using IOManager;
using Library.Controller;
using Library.Model;
using MOXA_IO.CustomControls;
using MOXA_IO.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MOXA_IO.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        #region Variable

        private readonly IRegionManager _regionManager;
        public IDialogService _dialogService;
        private DataTable dtAccessControl = new DataTable();
        public ServerStatusController _serverStatusController;
        private readonly ServerStatusModel _serverStatusModel = new ServerStatusModel();
        private SynchronizationContext _context;
        #endregion Variable

        #region Constructor
        public MenuViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            OperationCommand = new DelegateCommand<string>(Operation);
            SetAccessControl();
            _context = SynchronizationContext.Current;
            GlobalVar.Publisher_MachineStatus.GetEvent<MachineStatus>().Subscribe(machineStatus);
            AttachEventHandlers();
            IsEnable_IO = false;
        }

        #endregion Constructor

        #region Methods
        private void AttachEventHandlers()
        {
            _serverStatusModel.PropertyChanged += ServerStatusModelPropertyChanged;

        }
        private void ServerStatusModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "StatusMonitorConnected":
                    //ConnectAndUpdateFileSystem();
                    break;
                case "PolarisServerStarted":
                    PolarisServerInitializationSequence();
                    break;
            }
        }

        private void PolarisServerInitializationSequence()
        {

            // Initialize or de-initialize polaris based on the status of PolarisServer.
            if (_serverStatusModel.PolarisServerStarted)
            {
                try
                {
                    //if ((GlobalVar.IIO).OpenDevice())
                    //{
                    //    (GlobalVar.IIO as IPolarisIO).StartScanIO();
                    //}
                }
                catch (Exception ex)
                {
               
                }

                // this is basically injecting the axisModelList dependency on each view
                // and doing a minimal setup in the view - needs to be set up before we
                // call any of these functions
                //_homingController.InitializeAxes();

                // Updates display with status of polaris server.
                //_serverStatusController.GetPolarisServerStatus();

                // Set the flag to denote we are finished loading (initialized), and enable the controls.
                _serverStatusController.SetPolarisState(PolarisState.Initialized);
                IsEnable_IO = true;
            }
            else
            {
                PolarisUninitialized();
            }
        }  /// <summary>
           /// Helper method called to update the status monitor controller and the UI that polaris server has stopped.
           /// </summary>
        private void PolarisUninitialized()
        {
            // Updates display with status of polaris server.
            _serverStatusController.GetPolarisServerStatus();

            // Set the flag to denote polaris server has been stopped and polaris components
            // uninitialized.
            _serverStatusController.SetPolarisState(PolarisState.Stopped);
            IsEnable_IO = false;
            // reinitialize the start button
            //serverStartStopView.EnableStartButton();
        }

        private void Navigate(string Command)
        {
            if (Command == "Exit")
            {
                CustomMsgBox _customMsgBox = new CustomMsgBox();
                string _result = _customMsgBox.ShowWindow(new string[] { "Yes", "No" }, "Are You Sure Want to Exit ?", "Confirmation", System.Drawing.SystemIcons.Information);

                if (_result == "Yes")
                {
                    Environment.Exit(0);
                }
            }
            else if (Command == "Connect")
            {
                if (_serverStatusController == null)
                {
                    _serverStatusController = new ServerStatusController((GlobalVar.IIO as IPolarisIO).Script, _serverStatusModel);
                    ConnectToPolarisController();
                }
                (GlobalVar.IIO as IPolarisIO).Connect();
                (GlobalVar.IIO as IPolarisIO).StartScanIO();
            }
            else if (Command == "Disconnect")
            {
                (GlobalVar.IIO as IPolarisIO).Disconnect();
            }
            else if (Command == "ONPolaris")
            {
                if (_serverStatusController == null)
                {
                    _serverStatusController = new ServerStatusController((GlobalVar.IIO as IPolarisIO).Script, _serverStatusModel);
                    ConnectToPolarisController();
                }
                _serverStatusController.StartPolarisServerAsync();
            }
            else if (Command == "OFFPolaris")
            {
                _serverStatusController.StopPolarisServer();
            }
        }


        private void ConnectToPolarisController()
        {
            _serverStatusController.ConnectAndStartStatusMonitor("192.168.127.51", _context);
        }

        private void Operation(string Command)
        {
            if (Command == "Start")
            {
                GlobalVar.Publisher_MachineOperation.GetEvent<MachineOperation>().Publish(OperationCmd.Start);
            }
            else if (Command == "Stop")
            {
                GlobalVar.Publisher_MachineOperation.GetEvent<MachineOperation>().Publish(OperationCmd.Stop);
            }
            else if (Command == "Init")
            {
                GlobalVar.Publisher_MachineOperation.GetEvent<MachineOperation>().Publish(OperationCmd.Init);
            }
            else if (Command == "Reset")
            {
                GlobalVar.Publisher_MachineOperation.GetEvent<MachineOperation>().Publish(OperationCmd.Reset);
            }
        }


        private void SetAccessControl()
        {
            if (GlobalVar.IsLoggedIn)
            {
                dtAccessControl.Clear();

                IsEnable_About = dtAccessControl.Rows[0]["About"].ToString() == "1" ? true : false;
                IsEnable_Communication = dtAccessControl.Rows[0]["Communication"].ToString() == "1" ? true : false;
                //IsEnable_IO = dtAccessControl.Rows[0]["IO"].ToString() == "1" ? true : false;
                IsEnable_LogHistory = dtAccessControl.Rows[0]["LogHistory"].ToString() == "1" ? true : false;
                IsEnable_UserMgmt = dtAccessControl.Rows[0]["UserMgmt"].ToString() == "1" ? true : false;
                IsEnable_GalilController = dtAccessControl.Rows[0]["GalilController"].ToString() == "1" ? true : false;
                IsEnable_TeachPoint = dtAccessControl.Rows[0]["TeachPoint"].ToString() == "1" ? true : false;
                IsEnable_ACSController = dtAccessControl.Rows[0]["ACSController"].ToString() == "1" ? true : false;
                IsEnable_AdvantechController = dtAccessControl.Rows[0]["AdvtechController"].ToString() == "1" ? true : false;
                IsEnable_AdlinkController = dtAccessControl.Rows[0]["AdlinkController"].ToString() == "1" ? true : false;
                IsEnable_RobotController = dtAccessControl.Rows[0]["RobotController"].ToString() == "1" ? true : false;
                IsEnable_OEE = dtAccessControl.Rows[0]["OEE"].ToString() == "1" ? true : false;
                IsEnable_Setting = dtAccessControl.Rows[0]["Setting"].ToString() == "1" ? true : false;
            }
            else
            {
                IsEnable_About = false;
                IsEnable_Communication = false;
                IsEnable_IO = false;
                IsEnable_LogHistory = false;
                IsEnable_UserMgmt = false;
                IsEnable_GalilController = false;
                IsEnable_TeachPoint = false;
                IsEnable_ACSController = false;
                IsEnable_AdvantechController = false;
                IsEnable_AdlinkController = false;
                IsEnable_RobotController = false;
                IsEnable_OEE = false;
                IsEnable_Setting = false;
            }
        }
        #endregion Methods

        #region Events
        /// <summary>
        /// Handles the link changed to up event.
        /// Sets the PolarisServerStarted property of ServerStatusModel accordingly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LinkChangedToUp(object sender, EventArgs e)
        {
            _serverStatusModel.PolarisServerPaused = false;
            _serverStatusModel.PolarisServerStarted = true;
        }

        /// <summary>
        /// Handles the link changed to down event.
        /// Sets the PolarisServerStarted property of ServerStatusModel accordingly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LinkChangedToDown(object sender, EventArgs e)
        {
            _serverStatusModel.PolarisServerPaused = false;
            _serverStatusModel.PolarisServerStarted = false;
        }

        /// <summary>
        /// Handles the link changed to paused event.
        /// Sets the PolarisServerStarted property of ServerStatusModel accordingly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LinkChangedToPause(object sender, EventArgs e)
        {
            _serverStatusModel.PolarisServerPaused = true;
            _serverStatusModel.PolarisServerStarted = false;
        }
        public void machineStatus(Machine_Status Status)
        {
            GlobalVar.CurrentMachineStatus = Status;

            if (Status == Machine_Status.Idle)
            {
                if (GlobalVar.IsInitNeeded)
                {
                    IsEnable_BtnStart = false;
                    IsEnable_BtnStop = false;
                    IsEnable_BtnInit = true;
                    IsEnable_BtnReset = false;
                }
                else
                {
                    IsEnable_BtnStart = true;
                    IsEnable_BtnStop = false;
                    IsEnable_BtnInit = false;
                    IsEnable_BtnReset = false;
                }
            }
            else if (Status == Machine_Status.Initializing)
            {
                IsEnable_BtnStart = false;
                IsEnable_BtnStop = false;
                IsEnable_BtnInit = false;
                IsEnable_BtnReset = false;
            }
            else if (Status == Machine_Status.Running)
            {
                IsEnable_BtnStart = false;
                IsEnable_BtnStop = true;
                IsEnable_BtnInit = false;
                IsEnable_BtnReset = false;
            }
            else if (Status == Machine_Status.Stop)
            {
                IsEnable_BtnStart = true;
                IsEnable_BtnStop = false;
                IsEnable_BtnInit = true;
                IsEnable_BtnReset = true;
            }
            else if (Status == Machine_Status.Error)
            {
                IsEnable_BtnStart = true;
                IsEnable_BtnStop = false;
                IsEnable_BtnInit = true;
                IsEnable_BtnReset = true;
            }
            else if (Status == Machine_Status.Ending_Lot)
            {
                IsEnable_BtnStart = false;
                IsEnable_BtnStop = true;
                IsEnable_BtnInit = false;
                IsEnable_BtnReset = false;
            }
        }

        #endregion Events

        #region Properties
        private string _title = "Menu";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _txtLogin = "Login";
        public string TxtLogin
        {
            get { return _txtLogin; }
            set { SetProperty(ref _txtLogin, value); }
        }

        private Visibility _imgLogin = Visibility.Visible;
        public Visibility imgLogin
        {
            get { return _imgLogin; }
            set
            {
                SetProperty(ref _imgLogin, value);
                if (value == Visibility.Visible)
                {
                    imgLogout = Visibility.Collapsed;
                }
                else
                {
                    imgLogout = Visibility.Visible;
                }
            }
        }

        private Visibility _imgLogout = Visibility.Collapsed;
        public Visibility imgLogout
        {
            get { return _imgLogout; }
            set { SetProperty(ref _imgLogout, value); }
        }

        LoginDetails _LoginDetails;
        public LoginDetails LoginDetails
        {
            get { return _LoginDetails; }
            set { SetProperty(ref _LoginDetails, value); }
        }

        private bool _isEnable_About;
        public bool IsEnable_About
        {
            get { return _isEnable_About; }
            set { SetProperty(ref _isEnable_About, value); }
        }

        private bool _isEnable_Communication;
        public bool IsEnable_Communication
        {
            get { return _isEnable_Communication; }
            set { SetProperty(ref _isEnable_Communication, value); }
        }

        private bool _isEnable_IO;
        public bool IsEnable_IO
        {
            get { return _isEnable_IO; }
            set { SetProperty(ref _isEnable_IO, value); }
        }

        private bool _isEnable_LogHistory;
        public bool IsEnable_LogHistory
        {
            get { return _isEnable_LogHistory; }
            set { SetProperty(ref _isEnable_LogHistory, value); }
        }

        private bool _isEnable_UserMgmt;
        public bool IsEnable_UserMgmt
        {
            get { return _isEnable_UserMgmt; }
            set { SetProperty(ref _isEnable_UserMgmt, value); }
        }

        private bool _isEnable_GalilController;
        public bool IsEnable_GalilController
        {
            get { return _isEnable_GalilController; }
            set { SetProperty(ref _isEnable_GalilController, value); }
        }

        private bool _isEnable_TeachPoint;
        public bool IsEnable_TeachPoint
        {
            get { return _isEnable_TeachPoint; }
            set { SetProperty(ref _isEnable_TeachPoint, value); }
        }

        private bool _isEnable_ACSController;
        public bool IsEnable_ACSController
        {
            get { return _isEnable_ACSController; }
            set { SetProperty(ref _isEnable_ACSController, value); }
        }

        private bool _isEnable_AdvantechController;
        public bool IsEnable_AdvantechController
        {
            get { return _isEnable_AdvantechController; }
            set { SetProperty(ref _isEnable_AdvantechController, value); }
        }

        private bool _isEnable_AdlinkController;
        public bool IsEnable_AdlinkController
        {
            get { return _isEnable_AdlinkController; }
            set { SetProperty(ref _isEnable_AdlinkController, value); }
        }

        private bool _isEnable_RobotController;
        public bool IsEnable_RobotController
        {
            get { return _isEnable_RobotController; }
            set { SetProperty(ref _isEnable_RobotController, value); }
        }

        private bool _isEnable_OEE;
        public bool IsEnable_OEE
        {
            get { return _isEnable_OEE; }
            set { SetProperty(ref _isEnable_OEE, value); }
        }

        private bool _isEnable_Setting;
        public bool IsEnable_Setting
        {
            get { return _isEnable_Setting; }
            set { SetProperty(ref _isEnable_Setting, value); }
        }

        private bool _isEnable_BtnStart;
        public bool IsEnable_BtnStart
        {
            get { return _isEnable_BtnStart; }
            set { SetProperty(ref _isEnable_BtnStart, value); }
        }

        private bool _isEnable_BtnStop;
        public bool IsEnable_BtnStop
        {
            get { return _isEnable_BtnStop; }
            set { SetProperty(ref _isEnable_BtnStop, value); }
        }

        private bool _isEnable_BtnInit = true;
        public bool IsEnable_BtnInit
        {
            get { return _isEnable_BtnInit; }
            set { SetProperty(ref _isEnable_BtnInit, value); }
        }

        private bool _isEnable_BtnReset;
        public bool IsEnable_BtnReset
        {
            get { return _isEnable_BtnReset; }
            set { SetProperty(ref _isEnable_BtnReset, value); }
        }

        private string _tbUsername;
        public string TbUsername
        {
            get { return _tbUsername; }
            set { SetProperty(ref _tbUsername, value); }
        }

        private string _tbName;
        public string TbName
        {
            get { return _tbName; }
            set { SetProperty(ref _tbName, value); }
        }

        private string _tbLevel;
        public string TbLevel
        {
            get { return _tbLevel; }
            set { SetProperty(ref _tbLevel, value); }
        }

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand<string> OperationCommand { get; private set; }

        #endregion Properties
    }
}
