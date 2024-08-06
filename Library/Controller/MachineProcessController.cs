using System;
using PolarisCommunication;
using PolarisDemonstration.Model;

namespace PolarisDemonstration.Controller
{
    /// <summary>
    /// Extendable class used to demonstrate simple control of a machine process using a
    /// custom script with a data source.
    /// </summary>
    class MachineProcessController
    {
        private readonly IScript _script;
        private readonly MachineProcessModel _machineProcessModel;

        public MachineProcessController(IScript script, MachineProcessModel machineProcessModel)
        {
            _script = script;
            _machineProcessModel = machineProcessModel;
        }

        /// <summary>
        /// Calls a custom script on the Polaris Server.
        /// Uses the Script Call method directly; this is not a PolarisServerAPI function.
        /// </summary>
        internal void Start()
        {
            try
            {
                _script.Call("startLoadProcess");
            }
            catch (Exception e)
            {
                _machineProcessModel.NonFatalException = e;
            }
        }

        /// <summary>
        /// Calls a custom script on the Polaris Server.
        /// Uses the Script Call method directly; this is not a PolarisServerAPI function.
        /// </summary>
        internal void Stop()
        {
            try
            {
                _script.Call("pdsStop");
            }
            catch (Exception e)
            {
                _machineProcessModel.NonFatalException = e;
            }
        }

        #region Data Sources

        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            dataSourceMonitor.Add(_machineProcessModel.DataSources);
            dataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            UpdateProcessState();
        }

        private void UpdateProcessState()
        {
            // no need to check if this has changed, just set it, checking just adds overhead
            if (_machineProcessModel.StateCounter != null)
            {
                _machineProcessModel.State = _machineProcessModel.StateCounter.Value;
            }
        }

        #endregion
    }
}
