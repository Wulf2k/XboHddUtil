﻿using System;
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
            { "BasicPartition", new Guid("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7") }
        };



        public MainPage()
        {
            this.InitializeComponent();


        }

        private async void BtnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("");
            await dialog.ShowAsync();
        }
    }
}
