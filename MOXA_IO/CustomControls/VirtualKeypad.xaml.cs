using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MOXA_IO.CustomControls
{
    /// <summary>
    /// Interaction logic for VirtualKeypad.xaml
    /// </summary>
    public partial class VirtualKeypad : Window
    {
        #region Variable
        public TextBox TextBox_sender = null;
        public PasswordBox PasswordBox_sender = null;
        private string RestoreText = string.Empty;
        #endregion

        #region Constructor
        public VirtualKeypad()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TextBox_sender != null)
                RestoreText = TextBox_sender.Text;
            else if (PasswordBox_sender != null)
                RestoreText = PasswordBox_sender.Password;
        }
        #endregion

        #region FormEvent
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Numpad_button(object sender, RoutedEventArgs e)
        {
            Button _btn = sender as Button;

            try
            {
                if (TextBox_sender != null)
                {
                    int currentCursorPos = TextBox_sender.SelectionStart;
                    int cursorPosFromEnd = TextBox_sender.Text.Length - currentCursorPos;

                    TextBox_sender.Text = TextBox_sender.Text.Insert(currentCursorPos, _btn.Content.ToString());
                    //calculate from End, because input text will delete once error, will affect cursor positin when calculate from beginning
                    TextBox_sender.CaretIndex = TextBox_sender.Text.Length - cursorPosFromEnd;
                }
                else if (PasswordBox_sender != null)
                {
                    PasswordBox_sender.Password += _btn.Content.ToString();
                }
            }
            catch { }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBox_sender != null && TextBox_sender.Text.Length > 0)
                {
                    int currentCursorPos = TextBox_sender.SelectionStart;
                    TextBox_sender.Text = TextBox_sender.Text.Remove(currentCursorPos - 1, 1);
                    TextBox_sender.CaretIndex = currentCursorPos - 1;
                }
                if (PasswordBox_sender != null && PasswordBox_sender.Password.Length > 0)
                {
                    PasswordBox_sender.Password = PasswordBox_sender.Password.Remove(PasswordBox_sender.Password.Length - 1);
                }
            }
            catch { }
        }


        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_sender != null)
                TextBox_sender.Text = RestoreText;
            else if (PasswordBox_sender != null)
                PasswordBox_sender.Password = RestoreText;

            Close();
        }
        #endregion
    }
}
