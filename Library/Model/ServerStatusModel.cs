using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    /// <summary>
    /// Properties related to the Polaris Controller through the StatusMonitor.
    /// Implements INotifyPropertyChanged such that the view can be updated of changed properties.
    /// </summary>
    public class ServerStatusModel : ModelBase
    {
        public ServerStatusModel()
        {
            PolarisState = PolarisState.Undefined;
        }

        public PolarisState PolarisState { get; set; }

        #region PropertyChanged Event Properties

        private bool _statusMonitorConnected;
        public bool StatusMonitorConnected
        {
            get => _statusMonitorConnected;
            set
            {
                _statusMonitorConnected = value;
                FirePropertyChanged();
            }
        }

        private double _cpuLoad;
        public double CPULoad
        {
            get => _cpuLoad;
            set
            {
                _cpuLoad = value;
                FirePropertyChanged();
            }
        }

        private int _freeDiskSpace;
        public int FreeDiskSpace
        {
            get => _freeDiskSpace;
            set
            {
                _freeDiskSpace = value;
                FirePropertyChanged();
            }
        }

        private int _freeMemory;
        public int FreeMemory
        {
            get => _freeMemory;
            set
            {
                _freeMemory = value;
                FirePropertyChanged();
            }
        }

        private int _totalMemory;
        public int TotalMemory
        {
            get => _totalMemory;
            set
            {
                _totalMemory = value;
                FirePropertyChanged();
            }
        }

        private bool _polarisServerStarted;
        public bool PolarisServerStarted
        {
            get => _polarisServerStarted;
            set
            {
                _polarisServerStarted = value;
                FirePropertyChanged();
            }
        }

        private bool _polarisServerPaused;
        public bool PolarisServerPaused
        {
            get => _polarisServerPaused;
            set
            {
                _polarisServerPaused = value;
                FirePropertyChanged();
            }
        }

        private int _polarisServerStatus;
        public int PolarisServerStatus
        {
            get => _polarisServerStatus;
            set
            {
                _polarisServerStatus = value;
                FirePropertyChanged();
            }
        }

        private string _selectedController;
        public string SelectedController
        {
            get => _selectedController;
            set
            {
                _selectedController = value;
                FirePropertyChanged();
            }
        }

        #endregion
    }

    /// <summary>
    /// Describes the current state of the Polaris Server; used by the threaded start and stop mechanisms.
    /// </summary>
    public enum PolarisState
    {
        Initialized,
        Stopped,
        Undefined
    }
}
