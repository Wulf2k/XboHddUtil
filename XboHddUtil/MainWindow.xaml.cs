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
        public static string RAString(this Byte[] ba, int pos)
        {
            int length = 0x200;
            if (pos + length > ba.Length)
            {
                length = ba.Length - pos;
            }
            ASCIIEncoding asc = new ASCIIEncoding();
            string str = "";
            try
            {
                str = asc.GetString(ba, pos, length).Split((Char)0)[0];
            }
            catch { }
            return str;
        }
        public static string RUString(this Byte[] ba, int pos)
        {
            int length = 0x200;
            if (pos + length > ba.Length)
            {
                length = ba.Length - pos;
            }
            UnicodeEncoding uni = new UnicodeEncoding();
            string str = "";
            try
            {
                str = uni.GetString(ba, pos, length).Split((Char)0)[0];
            }
            catch { }
            return str;
        }

        public static void WBytes(this Byte[] ba, int pos, Byte[] bytes)
        {
            Buffer.BlockCopy(bytes, 0, ba, pos, bytes.Length);
        }
        public static void WInt8(this Byte[] ba, int pos, SByte val)
        {
            ba[pos] = (Byte)val;
        }
        public static void WInt16(this Byte[] ba, int pos, Int16 val)
        {
            ba.WBytes(pos, BitConverter.GetBytes(val));
        }
        public static void WInt32(this Byte[] ba, int pos, Int32 val)
        {
            ba.WBytes(pos, BitConverter.GetBytes(val));
        }
        public static void WInt64(this Byte[] ba, int pos, Int64 val)
        {
            ba.WBytes(pos, BitConverter.GetBytes(val));
        }
        public static void WUInt8(this Byte[] ba, int pos, Byte val)
        {
            ba[pos] = val;
        }
        public static void WUInt16(this Byte[] ba, int pos, UInt16 val)
        {
            ba.WBytes(pos, BitConverter.GetBytes(val));
        }
        public static void WUInt32(this Byte[] ba, int pos, UInt32 val)
        {
            ba.WBytes(pos, BitConverter.GetBytes(val));
        }
        public static void WUInt64(this Byte[] ba, int pos, UInt64 val)
        {
            ba.WBytes(pos, BitConverter.GetBytes(val));
        }
        public static void WAString(this Byte[] ba, int pos, string val)
        {
            ASCIIEncoding asc = new ASCIIEncoding();
            ba.WBytes(pos, asc.GetBytes(val));
        }
        public static void WUString(this Byte[] ba, int pos, string val)
        {
            UnicodeEncoding uni = new UnicodeEncoding();
            ba.WBytes(pos, uni.GetBytes(val));
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

        private ObservableCollection<PropVal> HDDProps;
        private ObservableCollection<PropVal> HDDTarget;
        private ObservableCollection<PropVal> HDDSource;




        public MainWindow()
        {
            InitializeComponent();

            cbHDDs.Items.Clear();

            HDDProps = new ObservableCollection<PropVal>();
            dgHDDProps.ItemsSource = HDDProps;

            HDDTarget = new ObservableCollection<PropVal>();
            dgHDDTarget.ItemsSource = HDDTarget;
            HDDSource = new ObservableCollection<PropVal>();
            dgHDDSource.ItemsSource = HDDSource;

            List<ManagementObject> drives = GetDrives();
            foreach (ManagementObject drive in drives)
            {
                cbHDDs.Items.Add(drive["DeviceID"]);
            }

            
        }

        private void CbHDDs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HDDProps.Clear();

            string selected = cbHDDs.SelectedItem.ToString();

            ManagementObject drive = GetDrive(selected);

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
                btnSetStandard.IsEnabled = false;
                btnSetXbExt.IsEnabled = false;

                HDDProps.Add(new PropVal("Low Level Access", "Denied"));
                
            }
            else
            {
                btnSetStandard.IsEnabled = true;
                btnSetXbExt.IsEnabled = true;

                Byte[] bytes = new byte[0x200];
                int read = 0;

                SetFilePointer(handle, 0x200, 0, 0);
                ReadFile(handle, bytes, bytes.Length, read, IntPtr.Zero);

                Guid diskGuid = new Guid(bytes.RBytes(0x38, 0x10));
                HDDProps.Add(new PropVal("Disk GUID", diskGuid.ToString()));

                SetFilePointer(handle, 0, 0, 0);
                ReadFile(handle, bytes, bytes.Length, read, IntPtr.Zero);

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
                CloseHandle(handle);
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
        ManagementObject GetDrive(string DeviceID)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

            try
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["DeviceID"].ToString() == DeviceID)
                    {
                        return queryObj;
                    }
                }
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

        private void BtnSetStandard_Click(object sender, RoutedEventArgs e)
        {

            string selected = cbHDDs.SelectedItem.ToString();
            ManagementObject drive = GetDrive(selected);

            IntPtr handle = CreateFile(drive["DeviceID"].ToString(), 0xC0000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (handle.ToInt64() == -1)
            {
                MessageBox.Show("Failed to generate handle to drive.\nRelaunch with admin rights?");
            }
            else
            {
                Byte[] bytes = new byte[0x200];
                int read = 0;
                int wrote = 0;

                SetFilePointer(handle, 0, 0, 0);
                ReadFile(handle, bytes, bytes.Length, read, IntPtr.Zero);

                SetFilePointer(handle, 0, 0, 0);
                bytes.WUInt16(0x1FE, 0xAA55);
                WriteFile(handle, bytes, bytes.Length, wrote, IntPtr.Zero);

                CloseHandle(handle);
                MessageBox.Show("Drive set to Standard.");
            }
        }

        private void BtnSetXbExt_Click(object sender, RoutedEventArgs e)
        {
            string selected = cbHDDs.SelectedItem.ToString();
            ManagementObject drive = GetDrive(selected);

            IntPtr handle = CreateFile(drive["DeviceID"].ToString(), 0xC0000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (handle.ToInt64() == -1)
            {
                MessageBox.Show("Failed to generate handle to drive.\nRelaunch with admin rights?");
            }
            else
            {
                Byte[] bytes = new byte[0x200];
                int read = 0;
                int wrote = 0;

                SetFilePointer(handle, 0, 0, 0);
                ReadFile(handle, bytes, bytes.Length, read, IntPtr.Zero);

                SetFilePointer(handle, 0, 0, 0);
                bytes.WUInt16(0x1FE, 0xCC99);
                WriteFile(handle, bytes, bytes.Length, wrote, IntPtr.Zero);

                CloseHandle(handle);
                MessageBox.Show("Drive set to xbExt.");
            }
        }

        private void BtnMarkTarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HDDTarget.Clear();

                string selected = cbHDDs.SelectedItem.ToString();
                ManagementObject drive = GetDrive(selected);

                HDDTarget.Add(new PropVal("Target HDD", drive["Index"].ToString()));
                HDDTarget.Add(new PropVal("Caption", drive["Caption"].ToString()));
                HDDTarget.Add(new PropVal("DeviceID", drive["DeviceID"].ToString()));
            }
            catch { }

            
        }

        private void BtnMarkSource_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HDDSource.Clear();

                string selected = cbHDDs.SelectedItem.ToString();
                ManagementObject drive = GetDrive(selected);

                HDDSource.Add(new PropVal("Source HDD", drive["Index"].ToString()));
                HDDSource.Add(new PropVal("Caption", drive["Caption"].ToString()));
                HDDSource.Add(new PropVal("DeviceID", drive["DeviceID"].ToString()));
            }
            catch { }
        }
    }
}
