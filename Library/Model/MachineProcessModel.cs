using System;
using System.Collections.Generic;
using System.ComponentModel;
using PolarisCommunication;
using PolarisDemonstration.Interfaces;

namespace PolarisDemonstration.Model
{
    /// <summary>
    /// State of some machine process. Implements INotifyPropertyChanged such that the view can be updated of changed properties.
    /// </summary>
    public class MachineProcessModel : ModelBase
    {
        private readonly string[] _states = {"idle", "loading", "done", "error"};

        public MachineProcessModel()
        {
            StateCounter = new RegisterInt("LoadingState", "Controller", DataSourceType.PolarisServerVariable);

            DataSources = new List<IDataSource>()
            {
                StateCounter
            };
        }

        public RegisterInt StateCounter { get; private set; }

        public List<IDataSource> DataSources { get; private set; }

        #region PropertyChanged Event Properties

        private int _state;
        public int State
        {
            get => _state;
            set
            {
                _state = value;
                FirePropertyChanged();
            }
        }

        public string StateString => _states[State];

        #endregion

    }
}
