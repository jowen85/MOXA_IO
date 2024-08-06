using System;
using System.Collections.Generic;
using PolarisCommunication;
using PolarisCommunication.PolarisServerAPI;
using PolarisDemonstration.Model;
using PolarisDemonstration.View;
using static PolarisCommunication.PolarisServerAPI.PSU;

namespace PolarisDemonstration.Controller
{
    public class PositionJogController
    {
        private readonly IList<AxisModel> _axisModelList;
        private readonly IScript _script;

        public PositionJogController(PositionJogView positionJogView, IScript script, IList<AxisModel> axisModelList)
        {
            _axisModelList = axisModelList;
            _script = script;
            View = positionJogView;
            View.AttachController(this);
            //InitializeAxes();
        }

        public void InitializeAxes()
        {
            View.Initialize(_axisModelList);
        }

        /// <summary>
        /// Performs a Jog on the specified Axis, in the positive direction.
        /// </summary>
        /// <param name="axis">the axis to jog.</param>
        internal void JogPlus(AxisModel axis)
        {
            MCI.MoveVelocity(_script, axis.Number, axis.Speed);
        }

        /// <summary>
        /// Performs a Jog on the specified Axis, in the negative direction.
        /// </summary>
        /// <param name="axis">the axis to jog.</param>
        internal void JogMinus(AxisModel axis)
        {
            MCI.MoveVelocity(_script, axis.Number, axis.Speed * -1);
        }

        /// <summary>
        /// Stops the Jogging on the specified Axis.
        /// </summary>
        /// <param name="axis">the axis to stop.</param>
        internal void JogStop(AxisModel axis)
        {
            MCI.PathStop(_script, axis.Number);
        }

        internal void SetDigitalOutputBit(string dev, int bitNum)
        {
            PSU.SetDigitalOutputBit(_script, dev, bitNum);
        }
        internal int GetDigitalOutputBitMask(int devNum, string displayName)
        {
            return PSU.GetDigitalOutputBitMask(_script, devNum, displayName);
        }
        internal int GetDigitalOutputBitName(string dev, string displayName)
        {
            return PSU.GetDigitalOutputBitName(_script, dev, displayName);
        }
        internal void SetDigitalOutputBitName(string dev, string displayName)
        {
            PSU.SetDigitalOutputBitName(_script, dev, displayName);
        }

        internal void ClearDigitalOutputBitName(string dev, string displayName)
        {
            PSU.ClearDigitalOutputBitName(_script, dev, displayName);
        }

        internal void ClearDigitalOutputBit(string dev, int bitNum)
        {
            PSU.ClearDigitalOutputBit(_script, dev, bitNum);
        }


        internal int GetDigitalOutputs(string dev)
        {
            return PSU.GetDigitalOutputs(_script, dev);
        }

        internal IList<DigitalIODisplayInfo> GetDigitalInputDisplayInfo(string deviceName)
        {
            return PSU.GetDigitalInputDisplayInfo(_script, deviceName);
        }

        internal IList<DigitalIODisplayInfo> GetDigitalOutputDisplayInfo(string deviceName)
        {
            return PSU.GetDigitalOutputDisplayInfo(_script, deviceName);
        }


        // todo - this needs a new name
        internal void AddDataSources(DataSourceMonitorZMQ dataSourceMonitor)
        {
            dataSourceMonitor.DataSourceUpdated += HandleDataSourceUpdate;
        }

        private void HandleDataSourceUpdate(object sender, EventArgs e)
        {
            foreach (var axis in _axisModelList)
            {
                axis.Position = axis.EncoderPosition.Value;
            }
        }

        public PositionJogView View { get; }


    }
}