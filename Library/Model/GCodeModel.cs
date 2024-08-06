using System.Collections.Generic;
using PolarisCommunication;

namespace PolarisDemonstration.Model
{
    /// <summary>
    /// State and Errors related to the GCode engine. Implements INotifyPropertyChanged such that the view can be updated of changed properties.
    /// </summary>
    public class GCodeModel : ModelBase
    {
        public GCodeModel()
        {
            // this initial setting is updated as soon as the g-code status data source is started
            PreviousState = GCodeProcessState.Stopped; 
            GCodeEngineMode = new RegisterInt("GCodeEngineMode", "Controller", DataSourceType.PolarisServerVariable);

            DataSources = new List<IDataSource> { GCodeEngineMode };
        }

        public RegisterInt GCodeEngineMode { get; }
        public List<IDataSource> DataSources { get; }

        internal GCodeProcessState PreviousState { get; set; }

        #region PropertyChanged Event Properties

        private GCodeProcessState _state;
        public GCodeProcessState State
        {
            get => _state;
            set
            {
                _state = value;
                FirePropertyChanged();
            }
        }

        private PolarisCommunication.PolarisServerAPI.GCI.GCIError _error;
        public PolarisCommunication.PolarisServerAPI.GCI.GCIError Error
        {
            get => _error;
            set
            {
                _error = value;
                FirePropertyChanged();
            }
        }

        #endregion
    }

    public enum GCodeProcessState
    {
        Stopped,
        Ready,
        Running,
        Paused,
        StepMode,
        SteppingPaused,
        Aborted,
        Pausing,
        RunningMDI
    }
}
