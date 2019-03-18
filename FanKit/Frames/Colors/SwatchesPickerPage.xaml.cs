﻿using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace FanKit.Frames.Colors
{
    public sealed partial class SwatchesPickerPage : Page
    {
        public SwatchesPickerPage()
        {
            this.InitializeComponent();
            this.Loaded += async (sender, e) =>
            {
                this.SwatchesPicker.Color = Color.FromArgb(255, 0, 187, 255);

                this.MarkdownText1.Text = await FanKit.Sample.File.GetFile("ms-appx:///TXT/Colors/SwatchesPickerPage.xaml.txt");
                this.MarkdownText2.Text = await FanKit.Sample.File.GetFile("ms-appx:///TXT/Colors/SwatchesPicker.xaml.txt");
                this.MarkdownText3.Text = await FanKit.Sample.File.GetFile("ms-appx:///TXT/Colors/SwatchesPicker.cs.txt");

            };
        } 

        private void SwatchesPicker_ColorChange(object sender, Windows.UI.Color value)=>   this.PaletteSolidBrush.Color = value;
     
    }
}
