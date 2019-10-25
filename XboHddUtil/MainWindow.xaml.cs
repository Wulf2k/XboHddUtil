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
    class crc
    {
        public static uint crc32(Byte[] input)
        {
            //This code shamelessly lifted from some guy who shamelessly lifted it from...
            //...eventually I'm told it traces back to the PHP code.

            //Modified to take byte array as input instead of string.
            //Alright, so that literally just involved me changing "string" to "byte"


            var table = new uint[]{
                0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419, 0x706AF48F,
                0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4, 0xE0D5E91E, 0x97D2D988,
                0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 0x90BF1D91, 0x1DB71064, 0x6AB020F2,
                0xF3B97148, 0x84BE41DE, 0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7,
                0x136C9856, 0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9,
                0xFA0F3D63, 0x8D080DF5, 0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172,
                0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B, 0x35B5A8FA, 0x42B2986C,
                0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 0x45DF5C75, 0xDCD60DCF, 0xABD13D59,
                0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423,
                0xCFBA9599, 0xB8BDA50F, 0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924,
                0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D, 0x76DC4190, 0x01DB7106,
                0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
                0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818, 0x7F6A0DBB, 0x086D3D2D,
                0x91646C97, 0xE6635C01, 0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E,
                0x6C0695ED, 0x1B01A57B, 0x8208F4C1, 0xF50FC457, 0x65B0D9C6, 0x12B7E950,
                0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65,
                0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 0x4ADFA541, 0x3DD895D7,
                0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A, 0x346ED9FC, 0xAD678846, 0xDA60B8D0,
                0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9, 0x5005713C, 0x270241AA,
                0xBE0B1010, 0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
                0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 0x2EB40D81,
                0xB7BD5C3B, 0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A,
                0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683, 0xE3630B12, 0x94643B84,
                0x0D6D6A3E, 0x7A6A5AA8, 0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1,
                0xF00F9344, 0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB,
                0x196C3671, 0x6E6B06E7, 0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC,
                0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5, 0xD6D6A3E8, 0xA1D1937E,
                0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 0xA6BC5767, 0x3FB506DD, 0x48B2364B,
                0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55,
                0x316E8EEF, 0x4669BE79, 0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236,
                0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F, 0xC5BA3BBE, 0xB2BD0B28,
                0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31, 0x2CD99E8B, 0x5BDEAE1D,
                0x9B64C2B0, 0xEC63F226, 0x756AA39C, 0x026D930A, 0x9C0906A9, 0xEB0E363F,
                0x72076785, 0x05005713, 0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38,
                0x92D28E9B, 0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21, 0x86D3D2D4, 0xF1D4E242,
                0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 0x18B74777,
                0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C, 0x8F659EFF, 0xF862AE69,
                0x616BFFD3, 0x166CCF45, 0xA00AE278, 0xD70DD2EE, 0x4E048354, 0x3903B3C2,
                0xA7672661, 0xD06016F7, 0x4969474D, 0x3E6E77DB, 0xAED16A4A, 0xD9D65ADC,
                0x40DF0B66, 0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
                0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 0xCDD70693,
                0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8, 0x5D681B02, 0x2A6F2B94,
                0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D
            };

            unchecked
            {
                uint crc = (uint)(((uint)0) ^ (-1));
                var len = input.Length;
                for (var i = 0; i < len; i++)
                {
                    crc = (crc >> 8) ^ table[
                        (crc ^ input[i]) & 0xFF
                ];
                }
                crc = (uint)(crc ^ (-1));

                if (crc < 0)
                {
                    crc += (uint)4294967296;
                }

                return crc;
            }
        }

    }
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
    class MBR
    {
        // Below link used as reference
        //    https://thestarman.pcministry.com/asm/mbr/GPT.htm

        public UInt32 NtDiskSignature = 0;
        public Byte LegacyNoBoot = 0;
        public Byte[] StartCHS = new byte[] { 0x00, 0x02, 0x00 };
        public Byte[] EndCHS = new byte[] { 0xFF, 0xFF, 0xFF };
        public Byte PartType = 0xEE;
        public UInt32 RelativeSectors = 1;
        public UInt32 TotalSectors = 0xFFFFFFFF;
        public UInt16 DriveType = 0xAA55;

        public MBR()
        {

        }

        public Byte[] ToBytes()
        {
            Byte[] bytes = new byte[0x200];     //Fix later, if DriveType is hardcoded at 0x1FE or last 2 bytes of sector

            bytes.WUInt32(0x1B8, NtDiskSignature);
            bytes.WUInt8(0x1BE, LegacyNoBoot);
            bytes.WBytes(0x1BF, StartCHS);
            bytes.WUInt8(0x1C2, PartType);
            bytes.WBytes(0x1C3, EndCHS);
            bytes.WUInt32(0x1C6, RelativeSectors);
            bytes.WUInt32(0x1CA, TotalSectors);
            bytes.WUInt16(0x1FE, DriveType);

            return bytes;
        }
    }
    class PartHeader
    {
        public string Signature = "EFI PART";
        public UInt32 Revision = 0x00010000;
        public Int32 HeaderSize = 0x5C;
        public UInt32 HeaderCRC = 0;
        public UInt64 MyLBA = 1;
        public UInt64 AltLBA = 1;
        public UInt64 FirstLBA = 0x22;
        public UInt64 LastLBA = 0;
        public Guid DiskGuid = new Guid();
        public UInt64 PartEntryLBA = 2;
        public UInt32 NumPartEntries = 0x80;
        public UInt32 SizePartEntries = 0x80;
        public UInt32 PartEntriesCRC = 0;
        

        public PartHeader()
        {

        }

        public Byte[] ToBytes()
        {
            Byte[] bytes = new byte[HeaderSize];

            bytes.WAString(0, Signature);
            bytes.WUInt32(8, Revision);
            bytes.WInt32(0xC, HeaderSize);
            bytes.WUInt32(0x10, HeaderCRC);
            bytes.WUInt64(0x18, MyLBA);
            bytes.WUInt64(0x20, AltLBA);
            bytes.WUInt64(0x28, FirstLBA);
            bytes.WUInt64(0x30, LastLBA);
            bytes.WBytes(0x38, DiskGuid.ToByteArray());
            bytes.WUInt64(0x48, PartEntryLBA);
            bytes.WUInt32(0x50, NumPartEntries);
            bytes.WUInt32(0x54, SizePartEntries);
            bytes.WUInt32(0x58, PartEntriesCRC);

            return bytes;
        }
    }
    class PartEntry
    {
        public Guid PartTypeGuid = new Guid();
        public Guid PartUniqueGuid = new Guid();
        public UInt64 FirstLBA = 0;
        public UInt64 LastLBA = 0;
        public UInt64 Flags = 0;
        public string PartName = "";
        public Int32 PartSize = 0x80;

        public PartEntry()
        {

        }
        public PartEntry(Guid PartTypeGuid, Guid PartUniqueGuid, UInt64 FirstLBA, UInt64 LastLBA, string PartName)
        {
            this.PartTypeGuid = PartTypeGuid;
            this.PartUniqueGuid = PartUniqueGuid;
            this.FirstLBA = FirstLBA;
            this.LastLBA = LastLBA;
            this.PartName = PartName;
        }

        public Byte[] ToBytes()
        {
            Byte[] bytes = new byte[PartSize];
            bytes.WBytes(0, PartTypeGuid.ToByteArray());
            bytes.WBytes(0x10, PartUniqueGuid.ToByteArray());
            bytes.WUInt64(0x20, FirstLBA);
            bytes.WUInt64(0x28, LastLBA);
            bytes.WUInt64(0x30, Flags);
            bytes.WUString(0x38, PartName);

            return bytes;
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

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetVolumeLabel(string lpRootPathName, string lpVolumeName);

        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }


        Dictionary<string, Guid> guids = new Dictionary<string, Guid>
        {
            //User GUID pulled from personal drive =  "869bb5e0-3356-4be6-85f7-29323a675cc7"
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


        ManagementObject SourceDisk = null;
        ManagementObject TargetDisk = null;

        MBR ProtectiveMBR = new MBR();
        PartHeader GPTHeader = new PartHeader();
        List<PartEntry> PartEntries = new List<PartEntry>();


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
                int bps = int.Parse(drive["BytesPerSector"].ToString());
                Byte[] bytes = new byte[bps];
                int read = 0;

                SetFilePointer(handle, bps, 0, 0);
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
                int bps = int.Parse(drive["BytesPerSector"].ToString());
                
                Byte[] bytes = new byte[bps];
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
                int bps = int.Parse(drive["BytesPerSector"].ToString());
                
                Byte[] bytes = new byte[bps];
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
                TargetDisk = drive;
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
                SourceDisk = drive;
            }
            catch { }
        }

        private void BtnAutoPrep_Click(object sender, RoutedEventArgs e)
        {
            if (TargetDisk == null)
            {
                MessageBox.Show("Please select a valid Target disk.");
            }
            else
            {
                IntPtr handle = CreateFile(TargetDisk["DeviceID"].ToString(), 0xC0000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
                if (handle.ToInt64() == -1)
                {
                    MessageBox.Show("Low Level Access Denied.");
                }
                else
                {
                    UInt64 ts = UInt64.Parse(TargetDisk["TotalSectors"].ToString());
                    int bps = int.Parse(TargetDisk["BytesPerSector"].ToString());
                    int wrote = 0;

                    ProtectiveMBR = new MBR();
                    GPTHeader = new PartHeader();
                    PartEntries = new List<PartEntry>();

                    uint size = GPTHeader.SizePartEntries * GPTHeader.NumPartEntries;
                    if ((size % bps) > 0)
                    {
                        //Round partition array size up to sector boundary
                        size = (size / (uint)bps) + 1;
                        size = size * (uint)bps;
                    }
                        
                    //Byte array size is MBR sector + GPT Header sector + Partition array sectors
                    Byte[] bytes = new byte[size + 2 * bps];

                    //PartEntries
                    /* Standard sizes
                    PartEntry TempContent = new PartEntry(guids["BasicPartition"], guids["Temp"], 0x800, 0x52007FF, "Temp Content");
                    PartEntry UserContent = new PartEntry(guids["BasicPartition"], guids["User500GB"], 0x5200800, 0x66c007ff, "User Content");
                    PartEntry SystemSupport = new PartEntry(guids["BasicPartition"], guids["SystemSupport"], 0x66c00800, 0x6bc007ff, "System Support");
                    PartEntry SystemUpdate = new PartEntry(guids["BasicPartition"], guids["SystemUpdate"], 0x6bc00800, 0x6d4007ff, "System Update");
                    PartEntry SystemUpdate2 = new PartEntry(guids["BasicPartition"], guids["SystemUpdate2"], 0x6d400800, 0x6e2007ff, "System Update 2");
                     */

                    //Below values for testing, does not create a usable XB disk.
                    PartEntry TempContent = new PartEntry(guids["BasicPartition"], guids["Temp"], 0x800, 0x117FF, "Temp Content");
                    PartEntry UserContent = new PartEntry(guids["BasicPartition"], guids["User500GB"], 0x11800, 0x227ff, "User Content");
                    PartEntry SystemSupport = new PartEntry(guids["BasicPartition"], guids["SystemSupport"], 0x22800, 0x327ff, "System Support");
                    PartEntry SystemUpdate = new PartEntry(guids["BasicPartition"], guids["SystemUpdate"], 0x33800, 0x437ff, "System Update");
                    PartEntry SystemUpdate2 = new PartEntry(guids["BasicPartition"], guids["SystemUpdate2"], 0x43800, 0x537ff, "System Update");




                    PartEntries.Add(TempContent);
                    PartEntries.Add(UserContent);
                    PartEntries.Add(SystemSupport);
                    PartEntries.Add(SystemUpdate);
                    PartEntries.Add(SystemUpdate2);

                    //CRC is calculated on numparts * size, not the entirety of the sectors
                    Byte[] parts = new byte[GPTHeader.NumPartEntries * GPTHeader.SizePartEntries];

                    for (int i = 0; i < PartEntries.Count; i++)
                    {
                        parts.WBytes((int)(GPTHeader.SizePartEntries * i), PartEntries[i].ToBytes());
                    }
                    bytes.WBytes(bps * 2, parts);

                    //GPTHeader
                    //Guid diskGuid = Guid.Parse("25e8a1b2-0b2a-4474-93fa-35b847d97ee5");                     //Set to something, fix later.
                    Guid diskGuid = Guid.Parse("15e8a1b2-0b2a-4474-93fa-35b847d97ee5");                     //Set to something, fix later.

                    GPTHeader.DiskGuid = diskGuid;
                    GPTHeader.LastLBA = ts - 1;
                    GPTHeader.PartEntriesCRC = crc.crc32(parts);
                    GPTHeader.HeaderCRC = crc.crc32(GPTHeader.ToBytes());
                    bytes.WBytes(bps, GPTHeader.ToBytes());

                    //ProtectiveMBR
                    if (ts < 0x100000000) { ProtectiveMBR.TotalSectors = (uint)ts; }
                    bytes.WBytes(0, ProtectiveMBR.ToBytes());

                    SetFilePointer(handle, 0, 0, 0);
                    WriteFile(handle, bytes, bytes.Length, wrote, IntPtr.Zero);
                    CloseHandle(handle);


                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Volume");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        try
                        {
                           
                            if (queryObj["DeviceID"].ToString().Contains(guids["Temp"].ToString())) { SetVolumeLabel(queryObj["DriveLetter"].ToString(), "Temp Content"); }
                            if (queryObj["DeviceID"].ToString().Contains(guids["User500GB"].ToString())) { SetVolumeLabel(queryObj["DriveLetter"].ToString(), "User Content"); }
                            if (queryObj["DeviceID"].ToString().Contains(guids["SystemSupport"].ToString())) { SetVolumeLabel(queryObj["DriveLetter"].ToString(), "System Support"); }
                            if (queryObj["DeviceID"].ToString().Contains(guids["SystemUpdate"].ToString())) { SetVolumeLabel(queryObj["DriveLetter"].ToString(), "System Update"); }
                            if (queryObj["DeviceID"].ToString().Contains(guids["SystemUpdate2"].ToString())) { SetVolumeLabel(queryObj["DriveLetter"].ToString(), "System Update 2"); }

                        }
                        catch (Exception ex)
                        {
                             //MessageBox.Show(ex.Message);
                        }
                    }


                    //CreateSymbolicLink("\\\\?, fileName, SymbolicLink.File);


                    //TODO:  Investigate this method instead of searching - https://stackoverflow.com/questions/14479992/how-to-get-return-value-of-the-format-method-of-the-win32-volume-class-using-voi

                    /*
                    ManagementScope Scope;
                    Scope = new ManagementScope("\\\\.\\root\\CIMV2", null);

                    Scope.Connect();
                    ObjectGetOptions Options = new ObjectGetOptions();
                    ManagementPath Path = new ManagementPath("Win32_Volume.DeviceID=\"\\\\\\\\?\\\\Volume{{" + guids["Temp"].ToString() + "}\\\\\"");
                    ManagementObject ClassInstance = new ManagementObject(Scope, Path, Options);
                    ManagementBaseObject inParams = ClassInstance.GetMethodParameters("Format");


                    ManagementBaseObject outParams = ClassInstance.InvokeMethod("Format", inParams, null);
                    */






                    MessageBox.Show("MBR/Header/Partition Entries written.");
                }
            }
        }
    }
}
