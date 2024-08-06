using System;
using System.Windows;

namespace MOXA_IO.CustomControls
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen
    {
        #region Constructors
        public SplashScreen(string MachineName, string Version)
        {
            InitializeComponent();

            progBar.IsIndeterminate = false;
            lblMachineName.Text = MachineName;
            lblVersion.Content = Version;
            tbLoadTitle.Text = "Loading...";
        }
        #endregion Constructors

        #region Methods

        public void UpdateStatus(string LoadTitle, int Percentage)
        {
            Dispatcher.Invoke(() =>
            {
                tbLoadTitle.Text = LoadTitle;
                progBar.Value = Percentage;
            });
        }
        #endregion Methods

    }
}
