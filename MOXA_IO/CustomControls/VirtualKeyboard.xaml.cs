using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MOXA_IO.CustomControls
{
    /// <summary>
    /// Interaction logic for VritualKeyboard.xaml
    /// </summary>
    public partial class VirtualKeyboard : Window
    {
        #region Variable
        public TextBox TextBox_sender = null;
        public PasswordBox PasswordBox_sender = null;
        #endregion

        #region Constructor
        public VirtualKeyboard()
        {
            InitializeComponent();
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

        private void button_enter_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void keyboard_button(object sender, RoutedEventArgs e)
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

        private void button_backspace_Click(object sender, RoutedEventArgs e)
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

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_sender != null)
                TextBox_sender.Text = null;
            if (PasswordBox_sender != null)
                PasswordBox_sender.Password = null;
        }

        private void button_shift_Click(object sender, RoutedEventArgs e)
        {
            if (button_shift.Foreground.ToString().Equals(Brushes.Black.ToString()))
            {
                button_shift.Foreground = Brushes.Blue;

                keyboard_button1.Content = "!";
                keyboard_button2.Content = "@";
                keyboard_button3.Content = "#";
                keyboard_button4.Content = "$";
                keyboard_button5.Content = "%";
                keyboard_button6.Content = "^";
                keyboard_button7.Content = "&";
                keyboard_button8.Content = "*";
                keyboard_button9.Content = "(";
                keyboard_button10.Content = ")";
                keyboard_button11.Content = "_";
                keyboard_button12.Content = ":";
                keyboard_button13.Content = '"';

                keyboard_button14.Content = "Q";
                keyboard_button15.Content = "W";
                keyboard_button16.Content = "E";
                keyboard_button17.Content = "R";
                keyboard_button18.Content = "T";
                keyboard_button19.Content = "Y";
                keyboard_button20.Content = "U";
                keyboard_button21.Content = "I";
                keyboard_button22.Content = "O";
                keyboard_button23.Content = "P";
                keyboard_button24.Content = "+";

                keyboard_button25.Content = "A";
                keyboard_button26.Content = "S";
                keyboard_button27.Content = "D";
                keyboard_button28.Content = "F";
                keyboard_button29.Content = "G";
                keyboard_button30.Content = "H";
                keyboard_button31.Content = "J";
                keyboard_button32.Content = "K";
                keyboard_button33.Content = "L";
                keyboard_button34.Content = "{";
                keyboard_button35.Content = "}";

                keyboard_button36.Content = "Z";
                keyboard_button37.Content = "X";
                keyboard_button38.Content = "C";
                keyboard_button39.Content = "V";
                keyboard_button40.Content = "B";
                keyboard_button41.Content = "N";
                keyboard_button42.Content = "M";
                keyboard_button43.Content = "<";
                keyboard_button44.Content = ">";
                keyboard_button45.Content = "?";
                keyboard_button46.Content = "|";
            }
            else
            {
                button_shift.Foreground = Brushes.Black;

                keyboard_button1.Content = "1";
                keyboard_button2.Content = "2";
                keyboard_button3.Content = "3";
                keyboard_button4.Content = "4";
                keyboard_button5.Content = "5";
                keyboard_button6.Content = "6";
                keyboard_button7.Content = "7";
                keyboard_button8.Content = "8";
                keyboard_button9.Content = "9";
                keyboard_button10.Content = "0";
                keyboard_button11.Content = "-";
                keyboard_button12.Content = ";";
                keyboard_button13.Content = "'";

                keyboard_button14.Content = "q";
                keyboard_button15.Content = "w";
                keyboard_button16.Content = "e";
                keyboard_button17.Content = "r";
                keyboard_button18.Content = "t";
                keyboard_button19.Content = "y";
                keyboard_button20.Content = "u";
                keyboard_button21.Content = "i";
                keyboard_button22.Content = "o";
                keyboard_button23.Content = "p";
                keyboard_button24.Content = "=";

                keyboard_button25.Content = "a";
                keyboard_button26.Content = "s";
                keyboard_button27.Content = "d";
                keyboard_button28.Content = "f";
                keyboard_button29.Content = "g";
                keyboard_button30.Content = "h";
                keyboard_button31.Content = "j";
                keyboard_button32.Content = "k";
                keyboard_button33.Content = "l";
                keyboard_button34.Content = "[";
                keyboard_button35.Content = "]";

                keyboard_button36.Content = "z";
                keyboard_button37.Content = "x";
                keyboard_button38.Content = "c";
                keyboard_button39.Content = "v";
                keyboard_button40.Content = "b";
                keyboard_button41.Content = "n";
                keyboard_button42.Content = "m";
                keyboard_button43.Content = ",";
                keyboard_button44.Content = ".";
                keyboard_button45.Content = "/";
                keyboard_button46.Content = @"\";
            }
        }

        private void button_spacing_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_sender != null)
            {
                int currentCursorPos = TextBox_sender.SelectionStart;
                int cursorPosFromEnd = TextBox_sender.Text.Length - currentCursorPos;

                TextBox_sender.Text = TextBox_sender.Text.Insert(currentCursorPos, " ");
                //calculate from End, because input text will delete once error, will affect cursor positin when calculate from beginning
                TextBox_sender.CaretIndex = TextBox_sender.Text.Length - cursorPosFromEnd;
            }
            else if (PasswordBox_sender != null)
            {
                PasswordBox_sender.Password += " ";
            }
        }
        #endregion
    }
}
