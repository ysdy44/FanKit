﻿using System;
using System.ComponentModel;
using Windows.UI.Xaml;


namespace FanKit.Frames.Library
{
    public class DataTemplateAdaptiver : DependencyObject, INotifyPropertyChanged
    {

        #region DependencyProperty


        //DependencyProperty
        public FrameworkElement PanelElement
        {
            get { return (FrameworkElement)GetValue(PanelElementProperty); }
            set { SetValue(PanelElementProperty, value); }
        }
        public static DependencyProperty PanelElementProperty = DependencyProperty.Register(nameof(PanelElement), typeof(FrameworkElement), typeof(DataTemplateAdaptiver), new PropertyMetadata(null, OnPanelElementChanged));
        private static void OnPanelElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataTemplateAdaptiver con)
            {
                if (e.NewValue != e.OldValue)
                {
                    con.PanelElement.SizeChanged += con.OnSizeChanged;
                }
            }
        }

         

        #endregion



        //Width
        public double sizeWidth;
        public double SizeWidth
        {
            get => sizeWidth; 
            set
            {
                sizeWidth = value;
                OnPropertyChanged(nameof(SizeWidth));
            }
        }
        //Height
        public double sizeHeight;
        public double SizeHeight
        {
            get => sizeHeight;
            set
            {
                sizeHeight = value;
                OnPropertyChanged(nameof(SizeHeight));
            }
        }

        public double imageHeight;
        public double ImageHeight
        {
            get => imageHeight;
            set
            {
                imageHeight = value;
                OnPropertyChanged(nameof(ImageHeight));
            }
        }

        protected void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = this.GetSizeWidth(e.NewSize.Width);

            this.SizeWidth = width;
            this.SizeHeight = width * 1.6d;
            this.ImageHeight = width * 1.1d;
        }

        private double GetSizeWidth(double width)
        {
            double designWidth = Math.Sqrt(Math.Sqrt(width)) * 24d;//design Width

            int count = ((int)(width / designWidth));//count of transverse

            return (width - 4) / count;//Aliquot width
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)=>  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




