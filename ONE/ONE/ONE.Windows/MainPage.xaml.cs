using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ONE.Data;
using ONE.Common;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using One;

// “通用中心应用程序”项目模板在 http://go.microsoft.com/fwlink/?LinkID=391955 上有介绍

namespace ONE
{
    /// <summary>
    /// 显示分组的项集合的页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string date;      
        ViewModel viewModel;
     
        public MainPage()
        {
            this.InitializeComponent();
                      
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            date = (string)e.Parameter;
            
            viewModel = new ViewModel(date);
            bool flag = await viewModel.LoadData();

            if (flag == true)
            {

                pageRoot.DataContext = viewModel;   //数据绑定

                                           
            }
            else
            {
                //无网络连接弹出对话
                MessageDialog dialog = new MessageDialog("无网络连接！");
                await dialog.ShowAsync();
 
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }


        
        private void itemClick(object sender, ItemClickEventArgs e)
        {
            string date = e.ClickedItem.ToString();
            Frame.Navigate(typeof(MainPage),date);        //导航到指定日期的主页
        }

        

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassagePage), viewModel);     //导航到内容页
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(QuestionPage), viewModel);
        }

        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileSavePicker savepicker = new FileSavePicker();
            savepicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savepicker.FileTypeChoices.Add("Image", new List<string> { ".jpg" });
            savepicker.SuggestedFileName = viewModel.one.date;

            StorageFile savefile = await savepicker.PickSaveFileAsync();
            if(savefile != null)
            {
                viewModel.stream.Seek(0);
                IRandomAccessStream input = viewModel.stream;
                Stream inputstream = WindowsRuntimeStreamExtensions.AsStreamForRead(input);

                IRandomAccessStream ouput = await savefile.OpenAsync(FileAccessMode.ReadWrite);
                Stream ouputstream = WindowsRuntimeStreamExtensions.AsStreamForWrite(ouput);

                await inputstream.CopyToAsync(ouputstream);
                ouputstream.Dispose();
                inputstream.Dispose();
            }
        }
    }

        
}
