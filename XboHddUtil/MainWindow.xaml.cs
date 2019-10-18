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


    public static class ExtBA
    {
        public static Byte[] RBytes(this Byte[]ba, int pos, int length)
        {
            Byte[] bytes = new byte[length];
            Buffer.BlockCopy(ba, pos, bytes, 0, length);
            return bytes;
        }
        public static SByte RInt8(this Byte[] ba, int pos)
        {
            return (SByte)ba[pos];
        }
        public static Int16 RInt16(this Byte[] ba, int pos)
        {
            return BitConverter.ToInt16(ba, pos);
        }
        public static Int32 RInt32(this Byte[] ba, int pos)
        {
            return BitConverter.ToInt32(ba, pos);
        }
        public static Int64 RInt64(this Byte[] ba, int pos)
        {
            return BitConverter.ToInt64(ba, pos);
        }
        public static Byte RUInt8(this Byte[] ba, int pos)
        {
            return (Byte)ba[pos];
        }
        public static UInt16 RUInt16(this Byte[] ba, int pos)
        {
            return BitConverter.ToUInt16(ba, pos);
        }
        public static UInt32 RUInt32(this Byte[] ba, int pos)
        {
            return BitConverter.ToUInt32(ba, pos);
        }
        public static UInt64 RUInt64(this Byte[] ba, int pos)
        {
            return BitConverter.ToUInt64(ba, pos);
        }


        public static void WInt8(this Byte[] ba, int pos, SByte val)
        {
            ba[pos] = (Byte)val;
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
        private ObservableCollection<PropVal> HDDProps;

        


        public MainWindow()
        {
            InitializeComponent();

            HDDs = new ObservableCollection<PropVal>();
            HDDProps = new ObservableCollection<PropVal>();
            dgHDDs.ItemsSource = HDDs;
            dgHDDProps.ItemsSource = HDDProps;

            ICollectionView cvHDDs = CollectionViewSource.GetDefaultView(dgHDDs.ItemsSource);
            cvHDDs.SortDescriptions.Clear();
            cvHDDs.SortDescriptions.Add(new SortDescription("Value", ListSortDirection.Ascending));


            List<ManagementObject> drives = GetDrives();
            foreach (ManagementObject drive in drives)
            {
                HDDs.Add(new PropVal(drive["DeviceID"].ToString(), drive["Index"].ToString()));
            }
        }

        private void DgHDDs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HDDProps.Clear();

            PropVal selected;
            selected = (PropVal)dgHDDs.SelectedItem;

            ManagementObject drive = GetDrive(int.Parse(selected.Value));

            HDDProps.Add(new PropVal("Caption", drive["Caption"].ToString()));
            HDDProps.Add(new PropVal("DeviceID", drive["DeviceID"].ToString()));
            HDDProps.Add(new PropVal("Index", drive["Index"].ToString()));
            HDDProps.Add(new PropVal("Partitions", drive["Partitions"].ToString()));
            HDDProps.Add(new PropVal("Size", drive["Size"].ToString()));
            HDDProps.Add(new PropVal("BytesPerSector", drive["BytesPerSector"].ToString()));
            HDDProps.Add(new PropVal("TotalSectors", drive["TotalSectors"].ToString()));



            IntPtr handle = CreateFile(drive["DeviceID"].ToString(), 0x80000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (handle.ToInt64() == -1)
            {
                HDDProps.Add(new PropVal("Low Level Access", "Denied"));
            }
            else
            {

                Byte[] bytes = new byte[0x200];
                int read = 0;

                SetFilePointer(handle, 0, 0, 0);
                ReadFile(handle, bytes, bytes.Length, read, IntPtr.Zero);
                CloseHandle(handle);

                string xbExt = "";
                switch (bytes.RUInt16(0x1FE).ToString("X4"))
                {
                    case "AA55":
                        xbExt = "Standard";
                        break;
                    case "CC99":
                        xbExt = "XB External";
                        break;
                    default:
                        xbExt = "Unknown";
                        break;
                }
                    

                


                HDDProps.Add(new PropVal("Standard/XbExt", xbExt));

                

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

        
    }
}
