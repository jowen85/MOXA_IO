using System;
using System.Collections.Generic;
using PolarisCommunication;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;
using PolarisDemonstration.View;

namespace PolarisDemonstration.Controller
{
    public class AutophaseController
    {
        private readonly IList<AxisModel> _axisModelList;
        private readonly IScript _script;

        public AutophaseController(AutophaseView view, IScript script, IList<AxisModel> axisModelList)
        {
            _axisModelList = axisModelList;
            _script = script;
            View = view;
            View.AttachController(this);
        }

        public void InitializeAxes()
        {
            View.Initialize(_axisModelList);
        }

        private DataSourceMonitorZMQ DataSourceMonitor { get; set; }

        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            var dataSources = CreateDataSources();

            DataSourceMonitor = dataSourceMonitor;
            DataSourceMonitor.Add(dataSources);
            DataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }

        // add the data sources for auto-phasing
        private IEnumerable<IDataSource> CreateDataSources()
        {
            var dataSources = new List<IDataSource>();

            foreach (var axis in _axisModelList)
            {
                var status = MCI.GetAutoPhaseStatus(_script, axis.Number);
                if (status >= 0)
                {
                    axis.AutoPhaseState = new RegisterInt(
                        $"AutophaseState_{axis.Number}",
                        axis.Name,
                        DataSourceType.PolarisServerVariable);
                    axis.DataSources.Add(axis.AutoPhaseState);
                    axis.SupportsAutophase = true;
                }
                else
                {
                    axis.SupportsAutophase = false;
                }
                dataSources.AddRange(axis.DataSources);
            }

            return dataSources;
        }

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            foreach (var axis in _axisModelList)
            {
                if (axis.SupportsAutophase)
                {
                    UpdateAxisAutoPhaseState(axis);
                }
            }
        }

        private static void UpdateAxisAutoPhaseState(AxisModel axis)
        {
            axis.AutoPhaseStatus = (AutophaseAxisStatus) axis.AutoPhaseState.Value;
        }

        public void Autophase(AxisModel axis)
        {
            MCI.AutoPhase(_script, axis.Number);
        }

        public AutophaseView View { get; }
    }
}