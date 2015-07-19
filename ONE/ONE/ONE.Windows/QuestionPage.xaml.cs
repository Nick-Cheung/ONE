using One;
using ONE.Common;
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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace ONE
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class QuestionPage : Page
    {
        ViewModel _viewmodel;
        string questioncontent;

        double CT_WIDTH = Window.Current.Bounds.Width / 2.5; //文本块的宽度
        double CT_HEIGHT = Window.Current.Bounds.Height - 180; //文本块的高度
        const double CT_MARGIN = 30d; //文本块的边距

        public QuestionPage()
        {
            this.InitializeComponent();

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewmodel = new ViewModel();
            _viewmodel = (ViewModel)e.Parameter;
            pageRoot.DataContext = _viewmodel;
            questioncontent = _viewmodel.one.strQuestionTitle + "\n\n" + _viewmodel.one.strQuestionContent + "\n\n" + _viewmodel.one.strAnswerTitle + "\n\n" + _viewmodel.one.strAnswerContent;
            loadquestion();


        }

        void loadquestion()
        {
            stPanel.Children.Clear();
            // 为了支持文本分块，使用RichTextBlock
            RichTextBlock tbContent = new RichTextBlock();

            tbContent.Width = CT_WIDTH;
            tbContent.Height = CT_HEIGHT;
            tbContent.TextWrapping = TextWrapping.Wrap;
            tbContent.Margin = new Thickness(CT_MARGIN, 0, CT_MARGIN, CT_MARGIN + 20);
            Paragraph ph = new Paragraph();

            Run txtRun = new Windows.UI.Xaml.Documents.Run();

            txtRun.Text = questioncontent;
            ph.Inlines.Add(txtRun);
            tbContent.Blocks.Add(ph);
            if (Window.Current.Bounds.Height == 1080)
                tbContent.FontSize = Convert.ToDouble(32);
            else if (Window.Current.Bounds.Height == 1440)
                tbContent.FontSize = Convert.ToDouble(38);
            else
                tbContent.FontSize = Convert.ToDouble(20);
            stPanel.Children.Add(tbContent);
            // 更新一下状态，方便获取是否有溢出的文本
            tbContent.UpdateLayout();
            bool isflow = tbContent.HasOverflowContent;
            // 因为除了第一个文本块是RichTextBlock，
            // 后面的都是RichTextBlockOverflow一个一个接起来的
            // 所以我们需要两个变量
            RichTextBlockOverflow oldFlow = null, newFlow = null;
            if (isflow)
            {
                oldFlow = new RichTextBlockOverflow();
                oldFlow.Width = CT_WIDTH;
                oldFlow.Height = CT_HEIGHT;
                oldFlow.Margin = new Thickness(CT_MARGIN, 0, CT_MARGIN, CT_MARGIN + 20);
                tbContent.OverflowContentTarget = oldFlow;
                stPanel.Children.Add(oldFlow);
                oldFlow.UpdateLayout();
                // 继续判断是否还有溢出
                isflow = oldFlow.HasOverflowContent;
            }
            while (isflow)
            {
                newFlow = new RichTextBlockOverflow();
                newFlow.Height = CT_HEIGHT;
                newFlow.Width = CT_WIDTH;
                newFlow.Margin = new Thickness(CT_MARGIN, 0, CT_MARGIN, CT_MARGIN + 20);
                oldFlow.OverflowContentTarget = newFlow;
                stPanel.Children.Add(newFlow);
                newFlow.UpdateLayout();
                // 继续判断是否还有溢出的文本
                isflow = newFlow.HasOverflowContent;
                // 当枪一个变量填充了文本后，
                // 把第一个变量的引用指向当前RichTextBlockOverflow
                // 确保OverflowContentTarget属性可以前后相接
                oldFlow = newFlow;
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            pageRoot.Frame.GoBack();
        }
    }
}
