using Prism.Regions;
using System.Windows;
using System.Windows.Input;

namespace MOXA_IO.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        private readonly IRegionManager _regionManager;

        public MainWindowView(IRegionManager regionManager)
        {
            InitializeComponent();

            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("TopContentRegion", typeof(TopPanelView));
            _regionManager.RegisterViewWithRegion("MenuContentRegion", typeof(MenuView));
            _regionManager.RegisterViewWithRegion("MainContentRegion", typeof(IOView));
        }

        private void Title_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }

                DragMove();
            }
        }

      
    }
}
