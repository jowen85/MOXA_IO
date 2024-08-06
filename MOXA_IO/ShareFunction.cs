using Core.Variables;
using MOXA_IO.CustomControls;
using System.Windows;
using System.Windows.Controls;

namespace MOXA_IO
{
    public class ShareFunction
    {
        public static void Show_VirtualKeyboard(object sender)// , Window owner)
        {
            if (GlobalVar.systemCFG.Setting.EnableVirtualKeyboard)
            {
                VirtualKeyboard m_VirtualKeyboard = new VirtualKeyboard();

                if (sender is TextBox)
                {
                    m_VirtualKeyboard.TextBox_sender = sender as TextBox;
                }
                else if (sender is PasswordBox)
                {
                    m_VirtualKeyboard.PasswordBox_sender = sender as PasswordBox;
                }
                else
                    return;

                Control objname = sender as Control;
                objname.Focus();//focus on clicked textbox/passwordbox
                m_VirtualKeyboard.WindowStartupLocation = WindowStartupLocation.Manual;

                var desktopWorkingArea = SystemParameters.WorkArea;
                m_VirtualKeyboard.Left = desktopWorkingArea.Left + (desktopWorkingArea.Width - m_VirtualKeyboard.Width) / 2;
                //m_VirtualKeyboard.Left = m_VirtualKeyboard.Owner.Left + (m_VirtualKeyboard.Owner.Width - m_VirtualKeyboard.Width) / 2;
                m_VirtualKeyboard.Top = desktopWorkingArea.Bottom - m_VirtualKeyboard.Height;
                m_VirtualKeyboard.ShowDialog();
            }
        }

        public static void Show_VirtualKeypad(object sender)//, Window owner)
        {
            if (GlobalVar.systemCFG.Setting.EnableVirtualKeypad)
            {
                VirtualKeypad m_VirtualKeypad = new VirtualKeypad();

                if (sender is TextBox)
                {
                    m_VirtualKeypad.TextBox_sender = sender as TextBox;
                }
                else if (sender is PasswordBox)
                {
                    m_VirtualKeypad.PasswordBox_sender = sender as PasswordBox;
                }
                else
                    return;

                Control objname = sender as Control;
                objname.Focus();//focus on clicked textbox/passwordbox
                m_VirtualKeypad.WindowStartupLocation = WindowStartupLocation.Manual;

                var desktopWorkingArea = SystemParameters.WorkArea;
                m_VirtualKeypad.Left = desktopWorkingArea.Left + (desktopWorkingArea.Width - m_VirtualKeypad.Width) / 2;
                //m_VirtualKeypad.Left = m_VirtualKeypad.Owner.Left + (m_VirtualKeypad.Owner.Width - m_VirtualKeypad.Width) / 2;
                m_VirtualKeypad.Top = desktopWorkingArea.Bottom - m_VirtualKeypad.Height;
                m_VirtualKeypad.ShowDialog();
            }
        }
    }
}
