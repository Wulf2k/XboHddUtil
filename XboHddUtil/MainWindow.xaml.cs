using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XboHddUtil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ManagementObject GetDrive(int drive)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                txtOutput.AppendText(queryObj.ToString() + Environment.NewLine);
                foreach (PropertyData prop in queryObj.Properties)
                {
                    //txtOutput.AppendText(prop.Name + ": " + prop.Value + Environment.NewLine);
                    

                }
                if (int.Parse(queryObj["Index"].ToString()) == 0)
                {
                    txtOutput.AppendText(queryObj["DeviceID"].ToString());
                }
            }
            return null;
        }

        private void BtnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            GetDrive(0);
        }
    }
}
