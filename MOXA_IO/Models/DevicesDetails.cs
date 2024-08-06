using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;

namespace MOXA_IO.Models
{
    public class DevicesDetails : BindableBase
    {
        public string DeviceName { get; set; }

        private bool _deviceConnectivity;
        public bool DeviceConnectivity
        {
            get { return _deviceConnectivity; }
            set
            {
                _deviceConnectivity = value;

                if (value)
                {
                    BackgroundColor = Brushes.YellowGreen;
                    Img_Wrong_Visibility = Visibility.Collapsed;
                }
                else
                {
                    BackgroundColor = Brushes.Red;
                    Img_Wrong_Visibility = Visibility.Visible;
                }
            }
        }

        private Brush _backgroundColor = Brushes.Red;
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private Visibility _img_Wrong_Visibility = Visibility.Visible;
        public Visibility Img_Wrong_Visibility
        {
            get { return _img_Wrong_Visibility; }
            set { SetProperty(ref _img_Wrong_Visibility, value); }
        }


    }
}
