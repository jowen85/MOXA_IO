using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Core.Facilities
{
    public class GlobalFunction
    {
        public static void Input_DigitsOnly(object sender)
        {
            TextBox textbox = (TextBox)sender;
            string MatchPattern = @"[^0-9]+";
            if (Regex.IsMatch(textbox.Text, MatchPattern))
            {
                int currentCursorPos = Regex.Match(textbox.Text, MatchPattern).Index;
                textbox.Text = Regex.Replace(textbox.Text, MatchPattern, "");
                textbox.CaretIndex = currentCursorPos; //set cursor to current pos
            }
        }

        public static void Input_Minus_DigitsOnly(object sender)
        {
            TextBox textbox = (TextBox)sender;
            string MatchPattern = @"[^0-9-]+";
            if (Regex.IsMatch(textbox.Text, MatchPattern))
            {
                int currentCursorPos = Regex.Match(textbox.Text, MatchPattern).Index;
                textbox.Text = Regex.Replace(textbox.Text, MatchPattern, "");
                textbox.CaretIndex = currentCursorPos; //set cursor to current pos
            }
        }

        public static void Input_Dot_DigitsOnly(object sender)
        {
            TextBox textbox = (TextBox)sender;
            string MatchPattern = @"[^0-9.]+";
            if (Regex.IsMatch(textbox.Text, MatchPattern))
            {
                int currentCursorPos = Regex.Match(textbox.Text, MatchPattern).Index;
                textbox.Text = Regex.Replace(textbox.Text, MatchPattern, "");
                textbox.CaretIndex = currentCursorPos; //set cursor to current pos
            }
        }

        public static void Input_Dot_Minus_DigitsOnly(object sender)
        {
            TextBox textbox = (TextBox)sender;
            string MatchPattern = @"[^0-9.-]+";
            if (Regex.IsMatch(textbox.Text, MatchPattern))
            {
                int currentCursorPos = Regex.Match(textbox.Text, MatchPattern).Index;
                textbox.Text = Regex.Replace(textbox.Text, MatchPattern, "");
                textbox.CaretIndex = currentCursorPos; //set cursor to current pos
            }
        }

        public static string GetIPAddress(string NetworkInterfaceName)
        {
            string localIP = "";
            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netif.Name == NetworkInterfaceName)
                {
                    IPInterfaceProperties properties = netif.GetIPProperties();
                    foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                    {
                        localIP = unicast.Address.ToString();
                        // break;
                    }
                    break;
                }
            }
            return localIP;
        }

        public static string GetNetworkName(string IPAddress)
        {
            string networkName = "";
            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties properties = netif.GetIPProperties();
                foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                {
                    if (unicast.Address.ToString() == IPAddress)
                    {
                        networkName = netif.Name;
                        break;
                    }
                }
                // break;
            }
            return networkName;
        }

        public static ObservableCollection<string> GetIPNetworkList()
        {
            ObservableCollection<string> NetworkList = new ObservableCollection<string>();
            NetworkInterface[] ifaceList = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface iface in ifaceList)
            {
                if (iface.OperationalStatus == OperationalStatus.Up && iface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    NetworkList.Add(iface.Name);
                }
            }
            return NetworkList;
        }

    }
}
