﻿using System;
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

namespace FanKit.Frames.Styles
{
    public sealed partial class FontWeightPage : Page
    {
        public FontWeightPage()
        {
            this.InitializeComponent();

            this.TopRun1.Text = "<TextBlock  FontWeight=\"";
            this.TopRun2.Text = "Normal";
            this.TopRun3.Text = "\"/>";
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double Offset = ((ScrollViewer)sender).VerticalOffset;

            //ImageButtonVisible：图片按钮可视
            FanKit.MainPage.ImageButtonVisibleChange(Offset);

            //顶栏
            this.TopGrid.Height = App.GetHeight(Offset);
        }

    }
}
