using System;
using System.Collections.Generic;
using System.Threading;
using PolarisCommunication;
using PolarisCommunication.CustomExceptions;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;
using PolarisDemonstration.View;

namespace PolarisDemonstration.Controller
{
    public class AsyncMessagingController
    {
        private readonly IScript _script;
        private IAsyncMessagingMonitor _asyncMessagingMonitor;

        private readonly AsyncMessagingModel _asyncMessagingModel;
        private readonly IList<AxisModel> _axisModelList;

        public AsyncMessagingController(AsyncMessagingView view, IScript script, 
            AsyncMessagingModel asyncMessagingModel, IList<AxisModel> axisModelList)
        {
            _script = script;
            _asyncMessagingModel = asyncMessagingModel;
            _axisModelList = axisModelList;
            View = view;
            View.AttachController(this);
        }

        internal void ConnectAndStart(SynchronizationContext context)
        {
            DisconnectAsyncMessagingMonitor();
            ConnectAsyncMessagingMonitor(context);
            StartAsyncMessagingMonitor();
        }
        
        private void DisconnectAsyncMessagingMonitor()
        {
            _asyncMessagingMonitor?.Stop();
            _asyncMessagingMonitor?.Disconnect();
        }

        // todo - move the dependency out of here paw 11mar19
        private void ConnectAsyncMessagingMonitor(SynchronizationContext context)
        {
            _asyncMessagingMonitor = new AsyncMessagingMonitor();
            _asyncMessagingMonitor.Connect(_script, context);

            // Attach method to update event
            _asyncMessagingMonitor.MotionDoneReceived += MotionDoneReceivedModel;
            _asyncMessagingMonitor.GenericMessageReceived += GenericMessageReceivedModel;
            _asyncMessagingMonitor.ErrorMessageReceived += ErrorMessageReceivedModel;
        }

        private void StartAsyncMessagingMonitor()
        {
            _asyncMessagingMonitor.Start();
            _asyncMessagingModel.AsyncMessagingStarted = true;
        }
        
        // move and wait
        internal void MotionDoneWaitAsyncMsg(AxisModel axis, double destination)
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
            MCI.MotionDoneWaitAsync(_script, axis.Number,-1);
        }

        private void MotionDoneReceivedModel(object sender, EventArgs e)
        {
            var msg = (AsyncMessageEventArgs)e;
            _asyncMessagingModel.MotionDoneReceived = msg.data;
        }

        private void GenericMessageReceivedModel(object sender, EventArgs e)
        {
            var msg = (AsyncMessageEventArgs) e;
            _asyncMessagingModel.GenericMessageReceived = msg.data;
        }

        private void ErrorMessageReceivedModel(object sender, EventArgs e)
        {
            var msg = (AsyncMessageEventArgs)e;
            _asyncMessagingModel.ErrorReceived = msg;
        }

        public AsyncMessagingView View { get; set; }

        public void InitializeAxes()
        {
            View.Initialize(_asyncMessagingModel, _axisModelList);
        }
    }
}
