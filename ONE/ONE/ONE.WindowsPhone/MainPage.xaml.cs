using ONE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using One;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

using System.IO;


// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ONE
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        string date;
        ViewModel viewModel;
        public MainPage()
        {
            this.InitializeComponent();
            isPicSaved = false;



        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            date = (string)e.Parameter;
            //date = "2015-07-17";

            viewModel = new ViewModel(date);

            bool flag = await viewModel.LoadData();

            if(flag == true)
            {
                pageRoot.DataContext = viewModel;
            }
            else
            {
                MessageDialog dialog = new MessageDialog("无网络连接");
                await dialog.ShowAsync();
            }

        }

        private void itemClick(object sender, ItemClickEventArgs e)
        {
            string date = e.ClickedItem.ToString();
            Frame.Navigate(typeof(MainPage), date);  
        }

        private bool isPicSaved;

        private async void hp_image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!isPicSaved)
            {
                string fileName = viewModel.one.date + ".jpg";
                StorageFolder folder = KnownFolders.PicturesLibrary;

                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);

                if (file != null)
                {
                    viewModel.stream.Seek(0);
                    IRandomAccessStream input = viewModel.stream;
                    Stream inputstream = WindowsRuntimeStreamExtensions.AsStreamForRead(input);

                    IRandomAccessStream ouput = await file.OpenAsync(FileAccessMode.ReadWrite);
                    Stream ouputstream = WindowsRuntimeStreamExtensions.AsStreamForWrite(ouput);

                    await inputstream.CopyToAsync(ouputstream);
                    ouputstream.Dispose();
                    inputstream.Dispose();

                    MessageDialog dialog = new MessageDialog("保存图片成功");
                    await dialog.ShowAsync();
                    isPicSaved = true;
                }
            }
            
        }


    }
}
