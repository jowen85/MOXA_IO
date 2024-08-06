using Prism.Mvvm;
using System.Windows;

namespace MOXA_IO.Models
{
    public class IODetails : BindableBase
    {
        public string Name { get; set; }
        public int Tag { get; set; }

        private bool _status = false;
        public bool Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private Visibility _img_OFF_Visibility = Visibility.Visible;
        public Visibility Img_OFF_Visibility
        {
            get { return _img_OFF_Visibility; }
            set { SetProperty(ref _img_OFF_Visibility, value); }
        }

    }
}
