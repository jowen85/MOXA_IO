using System;
using System.Collections.Generic;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;
using PolarisDemonstration.View;

namespace PolarisDemonstration.Controller
{
    public class HomingController
    {
        private readonly IList<AxisModel> _axisModelList;
        private readonly IScript _script;

        public HomingController(HomingView homingView, IScript script, IList<AxisModel> axisModelList)
        {
            _axisModelList = axisModelList;
            _script = script;
            View = homingView;
            View.AttachController(this);
        }
        
        public void Home(AxisModel axisToHome)
        {
            try
            {
                HomeAxis(axisToHome);
            }
            catch (Exception e)
            {
                if (e is TimeoutException || e is PolarisScriptException)
                {
                    axisToHome.NonFatalException = e;
                }
                else
                {
                    throw;
                }
            }
        }

        internal void SetHomingSpeed(AxisModel axis)
        {
            // set home on speed and home off speed
            _script.Call("mciHomeOnSpeedSetOneAxis", axis.Number, axis.HomingSpeed);
            _script.Call("mciHomeOffSpeedSetOneAxis", axis.Number, axis.HomingSpeed);
        }

        internal void SetHomingAccel(AxisModel axis)
        {
            // set homing acceleration & deceleration
            _script.Call("mciHomeAccelSetOneAxis", axis.Number, axis.HomingAcceleration);
            _script.Call("mciHomeDecelSetOneAxis", axis.Number, axis.HomingAcceleration);
        }

        // note - cast enum types to int for the script call, the script call
        // note - mechanism does not handle enum types
        private void HomeAxis(AxisModel axisToHome)
        {
            _script.Call(
                "demoHomingFunction",
                axisToHome.Number,
                (int)axisToHome.HomingDirection,
                (int)axisToHome.HomingType);
        }

        /// <summary>
        /// Gets axis homing speed. Axes have a home on and a home off speed, but 
        /// we are only retrieving the home on speed and using it for both.
        /// </summary>
        private void GetAxisHomingSpeed(AxisModel axis)
        {
            try
            {
                axis.HomingSpeed = MCI.GetHomeOnSpeed(_script, axis.Number);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10002:
                        throw new ArgumentException(
                            "Unexpected error. A null pointer was passed into function mciHomeOnSpeedGetOneAxis. Cannot setup Axes.");
                    case -10003:
                        throw new ArgumentException(
                            "Unexpected error. The axis number " + axis.Number + " is invalid and the system cannot continue.");
                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Gets axis homing acceleration. Axes have both a homing accel and
        /// a homing deceleration, but we are only retrieving the homing accel
        /// and using it for both.
        /// </summary>
        private void GetAxisHomingAcceleration(AxisModel axis)
        {
            try
            {
                axis.HomingAcceleration = MCI.GetHomingAcceleration(_script, axis.Number);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10002:
                        throw new ArgumentException(
                            "Unexpected error. A null pointer was passed into function mciHomeOnSpeedGetOneAxis. Cannot setup Axes.");
                    case -10003:
                        throw new ArgumentException(
                            "Unexpected error. The axis number " + axis.Number + " is invalid and the system cannot continue.");
                    default:
                        throw;
                }
            }
        }

        public void InitializeAxes()
        {
            try
            {
                foreach (var axis in _axisModelList)
                {
                    GetAxisHomingSpeed(axis);
                    GetAxisHomingAcceleration(axis);
                }
            }
            catch (Exception ex)
            {
                // use the first axis in the _axisModelList to raise a fatal exception,
                // we don't know which axis fails in the loop above
                _axisModelList[0].FatalException = ex;
            }

            // initialize the view here, because we can't initialize it until the 
            // axisModelList is initialized
            View.Initialize(_axisModelList);
        }

        public HomingView View { get; }

        private DataSourceMonitorZMQ DataSourceMonitor { get; set; }

        // todo - this could be in a base class
        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            var dataSources = CreateDataSources();

            DataSourceMonitor = dataSourceMonitor;
            DataSourceMonitor.Add(dataSources);
            DataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }

        private IEnumerable<IDataSource> CreateDataSources()
        {
            var dataSources = new List<IDataSource>();

            foreach (var axis in _axisModelList)
            {
                axis.HomingState = new RegisterInt($"homeStatus_{axis.Number}", axis.Name, DataSourceType.PolarisServerVariable);
                axis.DataSources.Add(axis.HomingState);

                dataSources.AddRange(axis.DataSources);
            }

            return dataSources;
        }

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            foreach (var axis in _axisModelList)
            {
                axis.HomingStatus = (HomingAxisStatus) axis.HomingState.Value;
            }
        }
    }
}