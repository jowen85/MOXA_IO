using Core.Enums;
using Core.Events;
using Core.Variables;
using MOXA_IO.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows.Media;

namespace MOXA_IO.ViewModels
{
    public class TopPanelViewModel : BindableBase, INotifyPropertyChanged
    {
        #region Variables
        private Timer _TmrMachine;
        private Device _device = new Device();
        #endregion Variables

        #region Constructors
        public TopPanelViewModel()
        {
            WindowLoaded = new DelegateCommand(Loaded);
            DevicesList = new ObservableCollection<DevicesDetails>();

            GlobalVar.Publisher_DeviceConnectivity.GetEvent<Device>().Subscribe(AddDevice);
            GlobalVar.Publisher_MachineStatus.GetEvent<MachineStatus>().Subscribe(machineStatus);

        }

        #endregion Constructors

        #region Methods
        private void Loaded()
        {
            _TmrMachine = new Timer();
            _TmrMachine.Interval = 1000;
            _TmrMachine.Elapsed += _TmrMachine_Elapsed;
            _TmrMachine.Start();
        }
        #endregion Methods

        #region Events
        private void AddDevice(Device _device)
        {
            DevicesList.Add(new DevicesDetails()
            {
                DeviceName = _device.DeviceName,
                DeviceConnectivity = _device.DeviceConnectivity,
            });
        }

        private void UpdateDeviceConnectivity()
        {
            try //try catch to handle error when UI loaded but devicesList still null
            {
                //TODO : add/remove the device connetivity
                DevicesList[0].DeviceConnectivity = GlobalVar.IIO.IsConnected;
                //DevicesList[2].DeviceConnectivity = GlobalVar.ACS.IsConnected;
            }
            catch { }
        }

        public void machineStatus(Machine_Status Status)
        {
            LblMachineStatus = Status.ToString();

            if (Status == Machine_Status.Idle)
            {
                BgStatus = Brushes.Blue;
            }
            else if (Status == Machine_Status.Initializing)
            {
                BgStatus = Brushes.Blue;
            }
            else if (Status == Machine_Status.Running)
            {
                BgStatus = Brushes.YellowGreen;
            }
            else if (Status == Machine_Status.Stop)
            {
                BgStatus = Brushes.OrangeRed;
            }
            else if (Status == Machine_Status.Error)
            {
                BgStatus = Brushes.OrangeRed;
            }
            else if (Status == Machine_Status.Ending_Lot)
            {
                BgStatus = Brushes.YellowGreen;
            }
        }

        private void _TmrMachine_Elapsed(object sender, ElapsedEventArgs e)
        {
            Lblcurrenttime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            UpdateDeviceConnectivity();
        }
        #endregion Events

        #region Properties
        private ObservableCollection<DevicesDetails> _devicesList;
        public ObservableCollection<DevicesDetails> DevicesList
        {
            get { return _devicesList; }
            set { SetProperty(ref _devicesList, value); }
        }

        private Brush _bgStatus = Brushes.Blue;
        public Brush BgStatus
        {
            get { return _bgStatus; }
            set { SetProperty(ref _bgStatus, value); }
        }

        private string _lblMachineStatus = "None";
        public string LblMachineStatus
        {
            get { return _lblMachineStatus; }
            set { SetProperty(ref _lblMachineStatus, value); }
        }

        private string _lblcurrenttime = "00:00:00";
        public string Lblcurrenttime
        {
            get { return _lblcurrenttime; }
            set { SetProperty(ref _lblcurrenttime, value); }
        }

        public DelegateCommand WindowLoaded { get; private set; }

        #endregion Properties
    }
}
