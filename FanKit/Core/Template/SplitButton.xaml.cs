﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FanKit.Frames.Template
{
    public sealed partial class SplitButton : UserControl
    {

        /// <summary>
        /// IconPath' geometry data
        /// </summary>
        public Geometry Data
        {
            get => this.Path.Data;
            set => this.Path.Data=value;
        }

        /// <summary>
        ///TextBlock' content string
        /// </summary>
        public string Label
        {
            get => this.TextBlock.Text;
            set => this.TextBlock.Text = value;
        }
        

        public SplitButton()
        {
            this.InitializeComponent();
        }
    }
}
