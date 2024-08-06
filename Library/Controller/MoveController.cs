using System;
using System.Collections.Generic;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;
using PolarisDemonstration.View;

namespace PolarisDemonstration.Controller
{
    public class MoveController
    {

        private readonly IScript _script;
        // todo - we're just passing this through to the view, seems like the view is doing too
        // todo - much in this case
        private readonly IList<AxisModel> _axisModelList;

        public MoveController(MoveView view, IScript script, IList<AxisModel> axisModelList)
        {
            _script = script;
            _axisModelList = axisModelList;
            View = view;
            View.AttachController(this);
        }

        /// <summary>
        /// Performs an absolute move on the specified Axis.
        /// </summary>
        /// <param name="axis">the axis to move.</param>
        /// <param name="destination">commanded position for the axis.</param>
        internal void Move(AxisModel axis, double destination)
        {
            try
            {
                MCI.MoveAbsolute(_script, axis.Number, destination);
            }
            catch (PolarisScriptException e)
            {
                switch (e.ErrorCode)
                {
                    case -10212:
                        axis.NonFatalException = new ApplicationException("The axis (" + axis.Number +
                                                                          ") cannot be moved because it is in the aborted state. Try shutting down the GCode Engine or resetting the system.");
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

        public void InitializeAxes()
        {
            View.Initialize(_axisModelList);
        }

        public MoveView View { get; set; }
    }
}