using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
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
    class PropVal
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public PropVal(string prop, string val)
        {
            this.Property = prop;
            this.Value = val;
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SetFilePointer(IntPtr hFile, int lDistanceToMove, [Out] int lpDistanceToMoveHigh, int dwMoveMethod);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32", SetLastError = true)]
        public static extern int ReadFile(IntPtr handle, byte[] bytes, int numBytesToRead, [Out] int numBytesRead, IntPtr overlapped_MustBeZero);

        [DllImport("kernel32", SetLastError = true)]
        public static extern int WriteFile(IntPtr handle, byte[] bytes, int numBytesToWrite, [Out] int numBytesWritten, IntPtr overlapped_MustBeZero);

        [DllImport("kernel32", SetLastError = true)]
        static extern int CloseHandle(IntPtr handle);
        Dictionary<string, Guid> guids = new Dictionary<string, Guid>
        {
            { "BasicPartition", new Guid("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7") },
            { "Temp", new Guid("B3727DA5-A3AC-4B3D-9FD6-2EA54441011B") },
            { "User500GB", new Guid("A2344BDB-D6DE-4766-9EB5-4109A12228E5") },
            { "User1TB", new Guid("25E8A1B2-0B2A-4474-93FA-35B847D97EE5") },
            { "User2TB", new Guid("5B114955-4A1C-45C4-86DC-D95070008139") },
            { "SystemSupport", new Guid("C90D7A47-CCB9-4CBA-8C66-0459F6B85724") },
            { "SystemUpdate", new Guid("9A056AD7-32ED-4141-AEB1-AFB9BD5565DC") },
            { "SystemUpdate2", new Guid("24B2197C-9D01-45F9-A8E1-DBBCFA161EB2") }
        };

        private ObservableCollection<PropVal> HDDs;





        public MainWindow()
        {
            InitializeComponent();

            

            HDDs = new ObservableCollection<PropVal>();
            dgHDDs.ItemsSource = HDDs;

            ICollectionView cvHDDs = CollectionViewSource.GetDefaultView(dgHDDs.ItemsSource);
            cvHDDs.SortDescriptions.Clear();
            cvHDDs.SortDescriptions.Add(new SortDescription("Value", ListSortDirection.Ascending));


            List<ManagementObject> drives = GetDrives();
            foreach (ManagementObject drive in drives)
            {
                HDDs.Add(new PropVal(drive["DeviceID"].ToString(), drive["Index"].ToString()));
            }
        }

        List<ManagementObject> GetDrives()
        {
            List<ManagementObject> drives = new List<ManagementObject>() { };
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            try
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    drives.Add(queryObj);
                }
                return drives;
            }
            catch { }
            return null;
        }

        ManagementObject GetDrive(int drive)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

            try
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (int.Parse(queryObj["Index"].ToString()) == drive)
                    {
                        return queryObj;
                    }
                }
            }
            catch { }
            return null;
        }

        private void BtnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            ManagementObject drive = GetDrive(0);

        }

        private void DgHDDs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
