using System;
using System.Collections.Generic;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;

namespace PolarisDemonstration.Controller
{
    public class AxisStatusController
    {
        private readonly IList<AxisModel> _axisModelList;
        private readonly IScript _script;

        public AxisStatusController(AxisStatusConfigurationView view, IScript script, IList<AxisModel> axisModelList)
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

        /// <summary>
        /// Enables the Axis if it is disabled; disables it if it is enabled.
        /// Also attempts to clear axis errors prior to enabling.
        /// </summary>
        /// <param name="axis">the axis to enable or disable.</param>
        internal bool IsEnableCheck(AxisModel axis)
        {
            bool chk = MCI.EnableCheck(_script, axis.Number);
            return chk;
        }


        /// <summary>
        /// Enables the Axis if it is disabled; disables it if it is enabled.
        /// Also attempts to clear axis errors prior to enabling.
        /// </summary>
        /// <param name="axis">the axis to enable or disable.</param>
        internal void EnableOrDisableAxis(AxisModel axis)
        {
            bool chk = MCI.EnableCheck(_script, axis.Number);
            if (axis.Enabled)
            {
                MCI.DisableAxis(_script, axis.Number);
            }
            else
            {
                if (axis.InError)
                {
                    ClearErrors(axis);
                }
                // we may still be in error at this point, if this is the case this call will
                // fail and throw an unhandled exception
                MCI.EnableAxis(_script, axis.Number);
            }
        }

        /// <summary>
        /// Clears the errors on a specified axis
        /// </summary>
        /// <param name="axis">the axis to clear the errors on</param>
        private void ClearErrors(AxisModel axis)
        {
            MCI.PathStop(_script, axis.Number);
            MCI.PathUnStop(_script, axis.Number);

            try
            {
                MCI.ClearErrors(_script, axis.Number);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10211:
                        axis.NonFatalException = new ApplicationException("The axis (" + axis.Number +
                                                                          ") cannot be cleared of errors because it is moving. The operation has been cancelled.");
                        break;
                    default:
                        throw;
                }
            }
            catch (TimeoutException e)
            {
                axis.NonFatalException = e;
            }
        }

        /// <summary>
        /// Sets the speed of an individual Axis and updates the AxisModel.
        /// </summary>
        /// <param name="axis">the axis who's speed is to be set.</param>
        /// <param name="speed">value representing the speed.</param>
        internal void SetAxisSpeed(AxisModel axis, double speed)
        {
            MCI.SetSpeed(_script, axis.Number, speed);
            axis.Speed = speed;
        }

        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            dataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }

        // ReSharper disable ConvertToConstant.Local
        private static readonly int AmpEnableBit = 0x08;
        // ReSharper restore ConvertToConstant.Local

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            foreach (var axis in _axisModelList)
            {
                axis.InError = axis.ErrorStatus.Value != 0;
                axis.Enabled = axis.DigitalOut.TestWithBitMask(AmpEnableBit);
            }
        }

        public AxisStatusConfigurationView View { get; }
    }
}