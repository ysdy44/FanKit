﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FanKit.Frames.Colors
{
    public sealed partial class SwatchesPicker : UserControl
    {
        //Delegate
        public delegate void ColorChangeHandler(object sender, Color value);
        public event ColorChangeHandler ColorChange = null;

        private bool isMultiSelect;
        public bool IsMultiSelect
        {
            get => isMultiSelect;
            set
            {
                if (value)
                {
                    this.AddButton.IsEnabled = false;

                    this.GridView.CanReorderItems = true;
                    this.GridView.IsSwipeEnabled = true;
                    this.GridView.AllowDrop = true;

                    this.GridView.IsItemClickEnabled = false;
                    this.GridView.SelectionMode = ListViewSelectionMode.Multiple;
                }
                else
                {
                    this.AddButton.IsEnabled = true;

                    this.GridView.CanReorderItems = false;
                    this.GridView.IsSwipeEnabled = false;
                    this.GridView.AllowDrop = false;

                    this.GridView.IsItemClickEnabled = true;
                    this.GridView.SelectionMode = ListViewSelectionMode.Single;
                }
                isMultiSelect = value;
            }
        }

        ObservableCollection<Color> Collection = new ObservableCollection<Color>
        {
            Color.FromArgb(255,0,0,0),Color.FromArgb(255,31,31,31),Color.FromArgb(255,63,63,63),Color.FromArgb(255,95,95,95),Color.FromArgb(255,127,127,127),Color.FromArgb(255,159,159,159),Color.FromArgb(255,191,191,191), Color.FromArgb(255,223,223,223), Color.FromArgb(255,255,255,255),
            Color.FromArgb(255, 255, 192, 203),Color.FromArgb(255, 220, 20, 60),Color.FromArgb(255, 255, 240, 245),Color.FromArgb(255, 219, 112, 147),Color.FromArgb(255, 255, 105, 180),Color.FromArgb(255, 199, 21, 133),Color.FromArgb(255, 218, 112, 214),Color.FromArgb(255, 216, 191, 216),
            Color.FromArgb(255, 221, 160, 221),Color.FromArgb(255, 238, 130, 238),Color.FromArgb(255, 255, 0, 255),Color.FromArgb(255, 139, 0, 139),Color.FromArgb(255, 128, 0, 128),Color.FromArgb(255, 186, 85, 211),Color.FromArgb(255, 148, 0, 211),Color.FromArgb(255, 75, 0, 130),
            Color.FromArgb(255, 138, 43, 226),Color.FromArgb(255, 147, 112, 219),Color.FromArgb(255, 123, 104, 238),Color.FromArgb(255, 106, 90, 205),Color.FromArgb(255, 72, 61, 139),Color.FromArgb(255, 230, 230, 250),Color.FromArgb(255, 0, 0, 205),Color.FromArgb(255, 25, 25, 112),
            Color.FromArgb(255, 0, 0, 139),Color.FromArgb(255, 0, 0, 128),Color.FromArgb(255, 65, 105, 225),Color.FromArgb(255, 100, 149, 237),Color.FromArgb(255, 119, 136, 153),Color.FromArgb(255, 112, 128, 144),Color.FromArgb(255, 30, 144, 255),Color.FromArgb(255, 240, 248, 255),
            Color.FromArgb(255, 70, 130, 180),Color.FromArgb(255, 135, 206, 250),Color.FromArgb(255, 135, 206, 235),Color.FromArgb(255, 0, 191, 255),Color.FromArgb(255, 173, 216, 230),Color.FromArgb(255, 176, 216, 230),Color.FromArgb(255, 95, 158, 160),Color.FromArgb(255, 240, 255, 255),
            Color.FromArgb(255, 224, 255, 255),Color.FromArgb(255, 175, 238, 238),Color.FromArgb(255, 0, 255, 255),Color.FromArgb(255, 0, 206, 209),Color.FromArgb(255, 47, 79, 79),Color.FromArgb(255, 0, 139, 139),Color.FromArgb(255, 0, 128, 128),Color.FromArgb(255, 72, 209, 204),
            Color.FromArgb(255, 32, 178, 170),Color.FromArgb(255, 64, 224, 208),Color.FromArgb(255, 127, 255, 212),Color.FromArgb(255, 102, 205, 170),Color.FromArgb(255, 0, 250, 154),Color.FromArgb(255, 245, 255, 250),Color.FromArgb(255, 0, 255, 127),Color.FromArgb(255, 60, 179, 113),
            Color.FromArgb(255, 46, 139, 87),Color.FromArgb(255, 144, 238, 144),Color.FromArgb(255, 152, 251, 152),Color.FromArgb(255, 143, 188, 143),Color.FromArgb(255, 50, 205, 50),Color.FromArgb(255, 0, 255, 0),Color.FromArgb(255, 34, 139, 34),Color.FromArgb(255, 127, 255, 0),
            Color.FromArgb(255, 124, 252, 0),Color.FromArgb(255, 173, 255, 47),Color.FromArgb(255, 85, 107, 47),Color.FromArgb(255, 154, 205, 50),Color.FromArgb(255, 107, 142, 35),Color.FromArgb(255, 245, 245, 220),Color.FromArgb(255, 250, 250, 210),Color.FromArgb(255, 255, 255, 240),
            Color.FromArgb(255, 255, 255, 224),Color.FromArgb(255, 255, 255, 0),Color.FromArgb(255, 128, 128, 0),Color.FromArgb(255, 189, 183, 107),Color.FromArgb(255, 255, 250, 205),Color.FromArgb(255, 238, 232, 170),Color.FromArgb(255, 240, 230, 140),Color.FromArgb(255, 255, 215, 0),
            Color.FromArgb(255, 255, 248, 220),Color.FromArgb(255, 218, 165, 32),Color.FromArgb(255, 184, 134, 11),Color.FromArgb(255, 255, 250, 240),Color.FromArgb(255, 253, 245, 230),Color.FromArgb(255, 245, 222, 179),Color.FromArgb(255, 255, 228, 181),Color.FromArgb(255, 255, 165, 0),
            Color.FromArgb(255, 255, 239, 213),Color.FromArgb(255, 255, 235, 205),Color.FromArgb(255, 255, 222, 173),Color.FromArgb(255, 250, 235, 215),Color.FromArgb(255, 210, 180, 140),Color.FromArgb(255, 222, 184, 135),Color.FromArgb(255, 255, 228, 196),Color.FromArgb(255, 255, 140, 0),
            Color.FromArgb(255, 250, 240, 230),Color.FromArgb(255, 205, 133, 63),Color.FromArgb(255, 244, 164, 96),Color.FromArgb(255, 210, 105, 30),Color.FromArgb(255, 255, 245, 238),Color.FromArgb(255, 160, 82, 45),Color.FromArgb(255, 255, 160, 122),Color.FromArgb(255, 255, 160, 122),
            Color.FromArgb(255, 255, 69, 0),Color.FromArgb(255, 255, 99, 71),Color.FromArgb(255, 255, 228, 225),Color.FromArgb(255, 250, 128, 114),Color.FromArgb(255, 255, 250, 250),Color.FromArgb(255, 240, 128, 128),Color.FromArgb(255, 188, 143, 143),Color.FromArgb(255, 205, 92, 92),
            Color.FromArgb(255, 165, 42, 42),Color.FromArgb(255, 178, 34, 34),Color.FromArgb(255, 139, 0, 0),Color.FromArgb(255, 128, 0, 0),
        };

        #region DependencyProperty


        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(StrawPicker), new PropertyMetadata(Windows.UI.Colors.White));
         

        #endregion

        public SwatchesPicker()
        {
            this.InitializeComponent();
        }

        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Collection.RemoveAt(this.GridView.SelectedIndex==-1?0 :this.GridView.SelectedIndex);
        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e)=>  this.Collection.Insert(0,this.Color);
        private void MultiSelectToggleButton_Checked(object sender, RoutedEventArgs e) => this.IsMultiSelect = true;
        private void MultiSelectToggleButton_Unchecked(object sender, RoutedEventArgs e) => this.IsMultiSelect = false;
    

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Color item)
            {
                this.Color = item;
                this.ColorChange?.Invoke(this, item);
            }
        }
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.GridView.SelectedIndex;
            this.RemoveButton.IsEnabled = (index >= 0 && index < Collection.Count);
        }


    }
}
