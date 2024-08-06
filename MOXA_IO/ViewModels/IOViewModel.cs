using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using MOXA_IO.Models;
using System.Collections.Generic;
using System;
using Core.Enums;
using System.Timers;
using IOManager;
using Core.Variables;
using System.Windows;
using System.IO;
using PolarisCommunication;

namespace MOXA_IO.ViewModels
{
    public class IOViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Variables
        private Timer tmrUpdateIO;
        private IBaseIO m_IIO;
        private string[] inputText;
        private string[] outputText;

        #endregion Variables

        #region Constructor

        public IOViewModel()
        {
            m_IIO = GlobalVar.IIO;
            IOCommand = new DelegateCommand<IODetails>(IO);
            InputList = new List<IODetails>();
            OutputList = new List<IODetails>();

            LoadIO();
            tmrUpdateIO = new Timer(200);
            tmrUpdateIO.Elapsed += TmrUpdateIO_Elapsed;
            tmrUpdateIO.Start();

        }

        #endregion Constructor

        #region Methods
        private void IO(IODetails _IOParam)
        {
            ushort bit = (ushort)_IOParam.Tag;
            ushort state = Convert.ToUInt16(!_IOParam.Status);
            m_IIO.WriteOutBit(bit, state);
        }

        private void LoadIO()
        {
            try
            {
                inputText = File.ReadAllLines(@"..\Config Section\IO\" + GlobalVar.systemCFG.DigitalIO.InputFileName);
                outputText = File.ReadAllLines(@"..\Config Section\IO\" + GlobalVar.systemCFG.DigitalIO.OutputFileName);

                // Input
                for (int i = 0; i < inputText.Length; i++)
                {
                    InputList.Add(new IODetails()
                    {
                        Name = inputText[i].Replace("_", " "),
                        Tag = i,
                    });
                }

                // Output
                for (int i = 0; i < outputText.Length; i++)
                {
                    OutputList.Add(new IODetails()
                    {
                        Name = outputText[i].Replace("_", " "),
                        Tag = i,
                    });
                }
                //for (int i = 0; i < Enum.GetNames(typeof(IO_IN)).Length; i++)
                //{
                //    InputList.Add(new IODetails()
                //    {
                //        Name = Enum.GetName(typeof(IO_IN), i).Replace("_", " "),
                //        Tag = i,
                //    });
                //}

                //for (int i = 0; i < Enum.GetNames(typeof(IO_OUT)).Length; i++)
                //{
                //    OutputList.Add(new IODetails()
                //    {
                //        Name = Enum.GetName(typeof(IO_OUT), i).Replace("_", " "),
                //        Tag = i,
                //    });
                //}
            }
            catch (Exception ex)
            {
                GlobalVar.ILog.LogData("Failed to delete log file" + "\n" + ex.Message, (int)LoggerFileList.Error);
            }
        }
        #endregion Methods

        #region Events
        private void TmrUpdateIO_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if((GlobalVar.IIO as IPolarisIO).DeviceIsConnected())
            //    (GlobalVar.IIO as IPolarisIO).StartScanIO();
            for (int i = 0; i < inputText.Length; i++)
            {
                bool _CurrentStatus = m_IIO.ReadInBit(Convert.ToUInt16(i));
                if (InputList[i].Status != _CurrentStatus)
                {
                    InputList[i].Img_OFF_Visibility = _CurrentStatus ? Visibility.Collapsed : Visibility.Visible;
                    InputList[i].Status = _CurrentStatus;
                }
            }

            for (int j = 0; j < outputText.Length; j++)
            {
                bool _CurrentStatus = m_IIO.ReadOutBit(Convert.ToUInt16(j));
                if (OutputList[j].Status != _CurrentStatus)
                {
                    OutputList[j].Img_OFF_Visibility = _CurrentStatus ? Visibility.Collapsed : Visibility.Visible;
                    OutputList[j].Status = _CurrentStatus;
                }
            }
        }

        #endregion Events

        #region Properties
        private string _title = "IO";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private List<IODetails> _inputList;
        public List<IODetails> InputList
        {
            get { return _inputList; }
            set { SetProperty(ref _inputList, value); }
        }

        private List<IODetails> _outputList;
        public List<IODetails> OutputList
        {
            get { return _outputList; }
            set { SetProperty(ref _outputList, value); }
        }

        public DelegateCommand<IODetails> IOCommand { get; private set; }

        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            tmrUpdateIO.Elapsed -= TmrUpdateIO_Elapsed;
            tmrUpdateIO.Dispose();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        #endregion
    }
}
