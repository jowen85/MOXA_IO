using System;
using System.ComponentModel;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;
using PolarisDemonstration.View;

namespace PolarisDemonstration.Controller
{
    /// <summary>
    /// Sets up methods for changing the state of the GCode engine on the remote Polaris Server,
    /// and for executing GCode files.
    /// </summary>
    public class GCodeProcessController
    {
        // todo - removed the DataSourceController/Model, using the dataSourceMonitor directly paw 7feb19
        private readonly IScript _script;
        //private readonly DataSourceController _dataSourceController;
        private readonly GCodeModel _gCodeModel;

        private readonly GCodeView _view;
        //private readonly DataSourcesModel _dataSourcesModel;

        //public GCodeProcessController(ScriptController scriptController, DataSourceController dataSourceController, GCodeModel gCodeModel, DataSourcesModel dataSourcesModel)
        public GCodeProcessController(GCodeView view, IScript script, 
            GCodeModel gCodeModel, FileSystemController fileSystemController)
        {
            _script = script;
            //_dataSourceController = dataSourceController;
            _gCodeModel = gCodeModel;
            //_dataSourcesModel = dataSourcesModel;
            _view = view;
            _view.AttachController(this);
            _view.Initialize(gCodeModel, fileSystemController);
        }

        public GCodeView View => _view;

        public void Stop()
        {
            GCI.Stop(_script);
        }

        public void Run(string fileName)
        {                
            GCI.RunGcode(_script, fileName);
        }

        public void RunLine(string lineText)
        {
            try
            {
                // calling PolarisServerAPI function directly because we want to explicitly check the exception value.
                GCI.ManualDataInput(_script, lineText);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -11214:
                        _gCodeModel.NonFatalException = new ArgumentException("Failed to parse GCode line. Check the syntax of the line and try again.");
                        break;
                    default:
                        throw;
                } 
            }  
        }

        public void Step()
        {
            GCI.Step(_script);
        }

        public void Pause()
        {
            GCI.Pause(_script);
        }

        #region Data Sources
        private DataSourceMonitorZMQ DataSourceMonitor { get; set; }

        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            // skip interacting with the dataSourceController and the dataSourcesModel and just use the 
            // dataSourceMonitorDirectly paw 7feb19
            //_dataSourceController.AddDataSources(_gCodeModel.DataSources);
            //_dataSourcesModel.PropertyChanged += DataSourcePropertyChanged;

            // not clear on whether this should be here or in the model, leaning towards the model, but leaving it
            // here for now paw 
            DataSourceMonitor = dataSourceMonitor;
            DataSourceMonitor.Add(_gCodeModel.DataSources);
            DataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }

        /// <summary>
        /// Updates properties of the model when our data source gets updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DataSourceValuesUpdated":
                    UpdateGCodeStatus();
                    break;
            }
        }

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            UpdateGCodeStatus();
        }

        /// <summary>
        /// Updates the GCodeStatus based on it's data source.
        /// If the status is Aborted, this method will update the Error property of the GCodeModel.
        /// </summary>
        private void UpdateGCodeStatus()
        {
            if (_gCodeModel.State == (GCodeProcessState) _gCodeModel.GCodeEngineMode.Value)
            {
                // bail out if the state hasn't changed
                return;
            }

            _gCodeModel.State = (GCodeProcessState)_gCodeModel.GCodeEngineMode.Value;

            if (_gCodeModel.State == GCodeProcessState.Aborted && _gCodeModel.PreviousState != GCodeProcessState.Aborted)
            {
                try
                {
                    _gCodeModel.Error = GCI.GetError(_script);
                }
                catch (Exception e)
                {
                    if (e is TimeoutException || e is PolarisScriptException)
                    {
                        _gCodeModel.NonFatalException = e;
                    }
                    else { throw; }
                }
            }

            _gCodeModel.PreviousState = _gCodeModel.State;
        }

        #endregion

    }
}
