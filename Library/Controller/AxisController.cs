using System;
using System.Collections.Generic;
using System.Configuration;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;

namespace PolarisDemonstration.Controller
{
    /// <summary>
    /// Sets up methods for changing and updating the state and position of Axes on the remote Polaris Server.
    /// </summary>
    class AxisController
    {
        // todo - this handles all the axes, so axis controller is a bit of a misnomer
        // todo - give it a better name? paw 14feb19
        // todo - after all the changes this isn't a controller anymore, there are no views
        // todo - that depend on it, all it does is initialize the axis models paw 12mar19
        // todo - figure out how to remove this whole thing, which means sorting out how much 
        // todo - behaviour the model has, is it just a dumb repository or does it communicate 
        // todo - with the server, or does the controller communicate with the server and 
        // todo - update the model paw 13mar19
        private readonly IScript _script;
        private readonly IList<AxisModel> _axisModelList;

// ReSharper disable ConvertToConstant.Local
        private static readonly int AmpEnableBit = 0x08;
// ReSharper restore ConvertToConstant.Local

        public event EventHandler AxesInitialized;

        public AxisController(IScript script, IList<AxisModel> axisModelList)
        {
            _script = script;
            _axisModelList = axisModelList;
        }

        // todo - move this construction out of here - inject the dependencies paw 11mar19
        /// <summary>
        /// Gets the Axis names from the configuration file and instantiates AxisModel objects for each of them.
        /// </summary>
        internal void InstantiateAxes()
        {
            _axisModelList.Add(new AxisModel(ConfigurationManager.AppSettings["Axis1DeviceName"]));
            _axisModelList.Add(new AxisModel(ConfigurationManager.AppSettings["Axis2DeviceName"]));
            _axisModelList.Add(new AxisModel(ConfigurationManager.AppSettings["Axis3DeviceName"]));
        }

        /// <summary>
        /// Gets the Axis numbers and units for each Axis, as well as the current speed and acceleration.
        /// Updates the AxesInitialized property upon successful initialization.
        /// 
        /// Any exceptions with script calls here are treated as fatal due to them being required for initialization.
        /// </summary> 
        internal void InitializeAxes()
        {
            try
            {
                foreach (var axis in _axisModelList)
                {
                    axis.Number = GetAxisNumberFromDeviceName(axis.Name);
                    axis.Units = GetAxisUnits(axis.Number);
                    axis.Status = GetAxisStatus(axis.Name);

                    axis.Speed = GetAxisSpeed(axis.Number);
                    axis.Acceleration = GetAxisAcceleration(axis.Number);
           
                    //GetAxisHomingSpeed(axis);
                    //GetAxisHomingAcceleration(axis);
                }

                // fire event to let MainForm know that the axes are initialized
                OnAllAxesInitialized();
            }
            catch (Exception e)
            {
                // use the first axis in the _axisModelList to raise a fatal exception,
                // we don't know which axis fails in the loop above
                _axisModelList[0].FatalException = e;
            }
        }

        // simple event to let subscribers know that all axes in the _axisModelList
        // are initialized
        private void OnAllAxesInitialized()
        {           
            AxesInitialized?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the axis number of a given axis (defined by its device name).
        /// </summary>
        /// <param name="name">The name of the device to retrieve an axis number for.</param>
        private int GetAxisNumberFromDeviceName(string name)
        {
            try
            {
                return PSU.GetAxisNumberFromDeviceName(_script, name);
            }
            catch (PolarisScriptException)
            {
                throw new ArgumentException("Failed to get axis number for device " + name +
                    " as specified in the configuration file app.config. Ensure that it matches a device name in the PolarisServer configuration file.");
            }        
        }

        /// <summary>
        /// Gets the units specified in the polaris server configuration for a given axis.
        /// </summary>
        /// <param name="axisNumber">The number of the axis to get units for.</param>
        private string GetAxisUnits(int axisNumber)
        {
            try
            {
                return MCI.GetAxisUnits(_script, axisNumber);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10003:
                        throw new ArgumentException("Unexpected error. The axis number " + axisNumber + " is invalid and the system cannot continue.");
                    case -10402:
                        throw new ArgumentException("Failed to get the units specification for axis " + axisNumber +
                            ". Ensure that this device has a unit type defined in the PolarisServer configuration file.");
                    default:
                        throw;
                } 
            }          
        }

        /// <summary>
        /// Gets the status of a given axis on the polaris controller.
        /// </summary>
        /// <param name="axisName">The name of the axis to get the status for</param>
        private string GetAxisStatus(string axisName)
        {
            try
            {
                return PSU.GetDeviceVersionNumber(_script, axisName);
            }
            catch (PolarisScriptException)
            {
                throw new ArgumentException(
                    "Unexpected error. A Null pointer was passed into function psuGetDeviceVersion. Cannot setup Axes.");
            }
        }

        /// <summary>
        /// Gets the current speed of a given axis.
        /// </summary>
        /// <param name="axisNumber"></param>
        private double GetAxisSpeed(int axisNumber)
        {
            try
            {
                return MCI.GetSpeed(_script, axisNumber);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10002:
                        throw new ArgumentException(
                            "Unexpected error. A Null pointer was passed into function mciSpeedGetOneAxis. Cannot setup Axes.");
                    case -10003:
                        throw new ArgumentException(
                            "Unexpected error. The axis number " + axisNumber + 
                            " is invalid and the system cannot continue.");
                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Gets the current acceleration of a given axis
        /// </summary>
        /// <param name="axisNumber"></param>
        private double GetAxisAcceleration(int axisNumber)
        {
            try
            {
                return MCI.GetAcceleration(_script, axisNumber);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10002:
                        throw new ArgumentException(
                            "Unexpected error. A Null pointer was passed into function mciAccelGetOneAxis. Cannot setup Axes.");
                    case -10003:
                        throw new ArgumentException(
                            "Unexpected error. The axis number " + axisNumber +
                            " is invalid and the system cannot continue.");
                    default:
                        throw;
                }
            }
        }


        ///// <summary>
        ///// Sets the speed of an individual Axis and updates the AxisModel.
        ///// </summary>
        ///// <param name="axis">the axis who's speed is to be set.</param>
        ///// <param name="speed">value representing the speed.</param>
        //internal void SetAxisSpeed(AxisModel axis, double speed)
        //{
        //    MCI.SetSpeed(_script, axis.Number, speed);
        //    axis.Speed = speed;
        //}

        ///// <summary>
        ///// Enables the Axis if it is disabled; disables it if it is enabled.
        ///// Also attempts to clear axis errors prior to enabling.
        ///// </summary>
        ///// <param name="axis">the axis to enable or disable.</param>
        //internal void EnableOrDisableAxis(AxisModel axis)
        //{
        //    if (axis.Enabled)
        //    {
        //        MCI.DisableAxis(_script, axis.Number);
        //    }
        //    else
        //    {
        //        if (axis.InError)
        //        {
        //            ClearErrors(axis);
        //        }
        //        // we may still be in error at this point, if this is the case this call will
        //        // fail and throw an unhandled exception
        //        MCI.EnableAxis(_script, axis.Number);
        //    }
        //}

        //// todo - remove this once nothing depends on it
        ///// <summary>
        ///// Performs an absolute move on the specified Axis.
        ///// </summary>
        ///// <param name="axis">the axis to move.</param>
        ///// <param name="destination">commanded position for the axis.</param>
        //internal void Move(AxisModel axis, double destination)
        //{
        //    try
        //    {
        //        MCI.MoveAbsolute(_script, axis.Number, destination);
        //    }
        //    catch (PolarisScriptException e)
        //    {
        //        switch (e.ErrorCode)
        //        {
        //            case -10212:
        //                axis.NonFatalException = new ApplicationException("The axis (" + axis.Number +
        //                                                                  ") cannot be moved because it is in the aborted state. Try shutting down the GCode Engine or resetting the system.");
        //                break;
        //            default:
        //                throw;
        //        }
        //    }
        //    catch (TimeoutException e)
        //    {
        //        axis.NonFatalException = e;
        //    }
        //}

        ///// <summary>
        ///// Clears the errors on a specified axis
        ///// </summary>
        ///// <param name="axis">the axis to clear the errors on</param>
        //private void ClearErrors(AxisModel axis)
        //{
        //    MCI.PathStop(_script, axis.Number);
        //    MCI.PathUnStop(_script, axis.Number);

        //    try
        //    {
        //        MCI.ClearErrors(_script, axis.Number);
        //    }
        //    catch (PolarisScriptException e)
        //    {
        //        switch (e.ErrorCode)
        //        {
        //            case -10211:
        //                axis.NonFatalException = new ApplicationException("The axis (" + axis.Number +
        //                    ") cannot be cleared of errors because it is moving. The operation has been cancelled.");
        //                break;
        //            default:
        //                throw;
        //        }
        //    }
        //    catch (TimeoutException e)
        //    {
        //        axis.NonFatalException = e;
        //    }
        //}

        #region Data Sources
        
        private DataSourceMonitorZMQ DataSourceMonitor { get; set; }

        /// <summary>
        /// Gets this controller to add its associated data sources to the data source controller.
        /// </summary>
        //internal void AddDataSources()
        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            var dataSources = CreateDataSources();

            DataSourceMonitor = dataSourceMonitor;
            DataSourceMonitor.Add(dataSources);
            //DataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }
        
        /// <summary>
        /// Instantiate various data source registers (of different types) pointed at values on the desired axis.
        /// </summary>
        private IEnumerable<IDataSource> CreateDataSources()
        {
            var dataSources = new List<IDataSource>();

            // this assumes that all axes in the model are auto phase-able
            foreach (AxisModel axis in _axisModelList)
            {
                // these registers cannot be initialized in the model due to the need for the axis number
                
                // todo - moved to autophase controller - done
                // auto phase is not universal, need to query the server to see if an axis is autophase-able
                //var status = MCI.GetAutoPhaseStatus(_script, axis.Number);
                //if (status >= 0)
                //{
                //    axis.AutoPhaseState = new RegisterInt(
                //        $"AutophaseState_{axis.Number}",
                //        axis.Name,
                //        DataSourceType.PolarisServerVariable);

                //    axis.DataSources.Add(axis.AutoPhaseState);
                //    axis.SupportsAutophase = true;
                //}
                //else
                //{
                //    axis.SupportsAutophase = false;
                //}

                // todo - move this to the homingController - done
                //axis.HomingState = new RegisterInt($"homeStatus_{axis.Number}", axis.Name, DataSourceType.PolarisServerVariable);
                //axis.DataSources.Add(axis.HomingState);

                // this is where we add the registers already initialized in the model
                dataSources.AddRange(axis.DataSources);
            }

            return dataSources;
        }

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            UpdateAxes();
        }

        /// <summary>
        /// Updates various properties of the Axes, based on the current values of the associated data sources.
        /// </summary>
        private void UpdateAxes()
        {
            foreach (AxisModel axis in _axisModelList)
            {
                UpdateAxisPosition(axis);
                UpdateAxisInError(axis);
                UpdateAxisEnabled(axis);
                //if (axis.SupportsAutophase)
                //{
                //    UpdateAxisAutoPhaseState(axis);
                //}
                //UpdateAxisHomingState(axis);
            }
        }

        /// <summary>
        /// Updates the axis position based on the data source.
        /// </summary>
        /// <param name="axis">The AxisModel to update.</param>
        private static void UpdateAxisPosition(AxisModel axis)
        {
            axis.Position = axis.EncoderPosition.Value;
        }

        /// <summary>
        /// Updates the Axis' InError state based on the data source.
        /// </summary>
        /// <param name="axis">The AxisModel to update.</param>
        private static void UpdateAxisInError(AxisModel axis)
        {
            // check all of the error flags, we're interested in any flag that's set
            axis.InError = axis.ErrorStatus.Value != 0;
        }

        /// <summary>
        /// Set the Axis' Enabled property based on the data source
        /// </summary>
        /// <param name="axis">The AxisModel to update.</param>
        private static void UpdateAxisEnabled(AxisModel axis)
        {
            axis.Enabled = axis.DigitalOut.TestWithBitMask(AmpEnableBit);
        }

        ///// <summary>
        ///// Set the Axis' Auto Phase status based on the data source
        ///// </summary>
        ///// <param name="axis">The AxisModel to update.</param>
        //private static void UpdateAxisAutoPhaseState(AxisModel axis)
        //{
        //    axis.AutoPhaseStatus = (AutophaseAxisStatus)axis.AutoPhaseState.Value;
        //}

        ///// <summary>
        ///// Set the Axis' Homing status based on the data source
        ///// </summary>
        ///// <param name="axis">The AxisModel to update.</param>
        //private static void UpdateAxisHomingState(AxisModel axis)
        //{
        //    axis.HomingStatus = (HomingAxisStatus)axis.HomingState.Value;
        //}

        #endregion


    }
}
