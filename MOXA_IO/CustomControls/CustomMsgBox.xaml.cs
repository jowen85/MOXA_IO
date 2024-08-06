using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MOXA_IO.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomMsgBox.xaml
    /// </summary>
    public partial class CustomMsgBox : Window
    {
        #region Variable
        string m_ReturnValue = "";
        #endregion Variable

        #region Constructor
        public CustomMsgBox()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Method

        public string ShowWindow(string[] _CustomButtonText, string _text, string _caption, System.Drawing.Icon Icon)
        {
            m_ReturnValue = "";
            txtMsg.Text = _text;
            title.Content = _caption;

            ColumnDefinition columgrid;

            System.Drawing.Icon icon = Icon;
            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            iconImg.Source = bs;

            for (int i = 0; i < _CustomButtonText.Length; i++)
            {
                Button _btn = new Button();
                _btn.SetResourceReference(Button.StyleProperty, "styGlassButton");
                _btn.Width = 120;
                _btn.FontSize = 16;
                _btn.FontWeight = FontWeights.SemiBold;
                _btn.Foreground = Brushes.Black;
                _btn.Background = Brushes.Silver;
                _btn.Content = _CustomButtonText[i];
                _btn.Click += new RoutedEventHandler(_btn_Click);

                columgrid = new ColumnDefinition();
                columgrid.Width = new GridLength(130);

                buttonGrid.ColumnDefinitions.Add(columgrid);
                Grid.SetColumn(_btn, i);
                buttonGrid.Children.Add(_btn);
            }

            ShowDialog();
            return m_ReturnValue;
        }

        void _btn_Click(object sender, RoutedEventArgs e)
        {
            Button _btn = (Button)sender;
            m_ReturnValue = _btn.Content.ToString();
            Close();
        }

        void Dispose()
        {
        }
        #endregion

        #region Destructor
        ~CustomMsgBox()
        {
            Dispose();
        }
        #endregion
    }
}
