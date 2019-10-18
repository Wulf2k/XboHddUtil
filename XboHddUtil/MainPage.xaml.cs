using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XboHddUtil
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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



        public MainPage()
        {
            this.InitializeComponent();


        }

        private async void BtnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog(guids["BasicPartition"].ToString());
            await dialog.ShowAsync();
        }
    }
}
