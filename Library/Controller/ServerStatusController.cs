using Library.Model;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace Library.Controller
{
    public class ServerStatusController
    {
        private StatusMonitor _statusMonitor;

        private readonly IScript _script;
        private readonly ServerStatusModel _serverStatusModel;

        private BackgroundWorker _backgroundStartup;
        private BackgroundWorker _backgroundReset;

        public ServerStatusController(IScript script, ServerStatusModel serverStatusModel)
        {
            _script = script;
            _serverStatusModel = serverStatusModel;
        }

        /// <summary>
        /// Connects and Starts the StatusMonitor, and attaches event handlers.
        /// Updates the StatusMonitorConnected property of the ServerStatusModel once connected.
        /// </summary>
        /// <param name="ipAddress">The ip v4 address.</param>
        /// <param name="context">The context.</param>
        public void ConnectAndStartStatusMonitor(string ipAddress, SynchronizationContext context)
        {
            try
            {
                // Get an Instance of StatusMonitor and Connects to it, using our Script (which may or may not of been successfully connected).
                _statusMonitor = StatusMonitor.GetInstance();

                _statusMonitor.Connect(ipAddress, context);
                _statusMonitor.StartStatusMonitor();

                // Connect event handlers.
                _statusMonitor.LinkStatusChangedToUp += LinkChangedToUp;
                _statusMonitor.LinkStatusChangedToDown += LinkChangedToDown;
                _statusMonitor.LinkStatusChangedToPause += LinkChangedToPause;
                _statusMonitor.StatusUpdated += StatusUpdate;

                _serverStatusModel.StatusMonitorConnected = true;
                _serverStatusModel.SelectedController = ipAddress;
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException || e is FormatException || e is PolarisServerUnreachableException)
                {
                    _serverStatusModel.FatalException = new FormatException("An invalid IP address (" + ipAddress + ") was specified in the configuration file app.config. Please correct and try again.");
                }
                else
                {
                    _serverStatusModel.FatalException = e;
                     MessageBox.Show(e.Message);
                }
            }
        }

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

        /// <summary>Handles the status updated event. Updates each status property accordingly.</summary>
        ///
        /// <param name="sender">The sender.</param>
        /// <param name="e">     Event information.</param>

        private void StatusUpdate(object sender, EventArgs e)
        {
            _serverStatusModel.CPULoad = _statusMonitor.CPULoad;
            _serverStatusModel.FreeDiskSpace = _statusMonitor.FreeDiskSpace;
            _serverStatusModel.FreeMemory = _statusMonitor.FreeMemory;
            _serverStatusModel.TotalMemory = _statusMonitor.TotalMemory;
        }

        // todo - move this to serverStartStopController paw 
        /// <summary>
        /// Starts Polaris Server using the specified log level.
        /// This is an asynchronous call. Refer to a link status update to determine when the server has started.
        /// </summary>
        /// <param name="selectedLogLevel"></param>
        private void StartPolarisServer(string selectedLogLevel)
        {
            if (_statusMonitor == null) return;

            switch (selectedLogLevel)
            {
                case "Errors":
                    _statusMonitor.StartSystemAsync(LogLevel.Error, RunMode.StandardOperation);
                    break;
                case "Warnings":
                    _statusMonitor.StartSystemAsync(LogLevel.Warning, RunMode.StandardOperation);
                    break;
                case "Notice":
                    _statusMonitor.StartSystemAsync(LogLevel.Notice, RunMode.StandardOperation);
                    break;
                case "Info":
                    _statusMonitor.StartSystemAsync(LogLevel.Info, RunMode.StandardOperation);
                    break;
                case "Debug":
                    _statusMonitor.StartSystemAsync(LogLevel.Debug, RunMode.StandardOperation);
                    break;
            }
        }

        /// <summary>
        /// Stops Polaris Server.
        /// This is an asynchronous call. Refer to a link status update to determine when the server has stopped.
        /// Note: Tells the DataSourceController to Disconnect. - how? paw 2feb17
        /// </summary>
        // todo - The StopSystemAsync call is only asynchronous in the sense 
        // todo - that it returns immediately while the 
        // todo link state may take some time to reflect the new state of the server. paw 21feb19
        public void StopPolarisServer()
        {
            _statusMonitor?.StopSystemAsync();
        }

        /// <summary>
        /// Sets the state of the Polaris Server from one of enum values,
        /// this is used to determine when polaris communication services have finished initializing after a startup.
        /// </summary>
        /// <param name="state"></param>
        public void SetPolarisState(PolarisState state)
        {
            _serverStatusModel.PolarisState = state;
        }

        /// <summary>
        /// Displays the start-up status of the Polaris System. 
        /// See the "PolarisServer Utilities" (PSU) API documentation for information on the psuGetPolarisStartupStatus script.
        /// </summary>
        public void GetPolarisServerStatus()
        {
            if (_serverStatusModel.PolarisServerStarted)
            {
                _serverStatusModel.PolarisServerStatus = PSU.GetPolarisStartupStatus(_script);
            }
            else
            {
                if (_serverStatusModel.PolarisServerPaused)
                {
                    _serverStatusModel.PolarisServerStatus = -2;
                }
                else
                {
                    _serverStatusModel.PolarisServerStatus = -1;
                }
            }
        }

        #region BackgroundWorker Startup and Reset

        /// <summary>
        /// Spawns a background worker to start Polaris Server
        /// </summary>
        public void StartPolarisServerAsync()
        {
            _backgroundStartup = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _backgroundStartup.DoWork += StartupPolaris;
            _backgroundStartup.ProgressChanged += ReportProgress;
            _backgroundStartup.RunWorkerCompleted += RunWorkerCompleted;
            _backgroundStartup.RunWorkerAsync();

        }

        /// <summary>
        /// Performs the asynchronous call to start Polaris Server and then waits for it to be started.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartupPolaris(object sender, DoWorkEventArgs e)
        {
            int count = 1;

            if (!_serverStatusModel.PolarisServerStarted)
            {
                StartPolarisServer("Notice");
            }

            while (_serverStatusModel.PolarisState != PolarisState.Initialized)
            {
                _backgroundStartup.ReportProgress(count++);
                Thread.Sleep(100);
            }

            Thread.Sleep(10);
        }

        /// <summary>
        /// Sets the background of the loading screen by a simple modulus operator to alternate value pairs (0,0,1,1,0,0,1,1...).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportProgress(object sender, ProgressChangedEventArgs e)
        {
        }

        /// <summary>
        /// Hides the loading screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="completedEventArgs"></param>
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs completedEventArgs)
        {
        }

        /// <summary>
        /// Spawns a background worker which is responsible for resetting the Polaris Server (Stopping and Starting).
        /// </summary>
        internal void CreateBackgroundReset()
        {
            _backgroundReset = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _backgroundReset.DoWork += ResetPolaris;
            _backgroundReset.ProgressChanged += ReportProgress;
            _backgroundReset.RunWorkerCompleted += RunWorkerCompleted;
            _backgroundReset.RunWorkerAsync();

        }

        /// <summary>
        /// Performs the asynchronous call to stop Polrais Server and waits for it to be stopped;
        /// then calls start Polaris Server and waits for it to be started again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPolaris(object sender, DoWorkEventArgs e)
        {
            int count = 1;

            StopPolarisServer();

            while (_serverStatusModel.PolarisState != PolarisState.Stopped)
            {
                _backgroundReset.ReportProgress(count++);
                Thread.Sleep(100);
            }

            StartPolarisServer("Notice");

            while (_serverStatusModel.PolarisState != PolarisState.Initialized)
            {
                _backgroundReset.ReportProgress(count++);
                Thread.Sleep(100);
            }

            Thread.Sleep(10);
        }

        #endregion


    }
}