﻿using System.ComponentModel;
using Windows.UI.Xaml;


namespace FanKit.Frames.Control
{
    public class AdaptiveSize : DependencyObject, INotifyPropertyChanged
    {

        #region DependencyProperty


        //DependencyProperty
        public FrameworkElement PanelElement
        {
            get { return (FrameworkElement)GetValue(PanelElementProperty); }
            set { SetValue(PanelElementProperty, value); }
        }
        public static DependencyProperty PanelElementProperty = DependencyProperty.Register(nameof(PanelElement), typeof(FrameworkElement), typeof(AdaptiveSize), new PropertyMetadata(null, OnPanelElementChanged));
        private static void OnPanelElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AdaptiveSize con && e.NewValue != e.OldValue)
            {
                con. PanelElement.SizeChanged += (s2, e2) => con.SizeWidth = con.GetSizeWidth();
                con.SizeWidth = con.GetSizeWidth();
            }
        }


        #endregion



        //Design Width
        public double designWidth=120;
        public double DesignWidth
        {
            get => designWidth;
            set
            {
                designWidth = value;
                OnPropertyChanged("DesignWidth");
            }
        }

        //Size Width
        public double sizeWidth;
        public double SizeWidth
        {
            get => sizeWidth; 
            set
            {
                sizeWidth = value;
                OnPropertyChanged("SizeWidth");
            }
        }
   
        //Get Width
        private double GetSizeWidth()
        {
            double width = this.PanelElement.ActualWidth;
            if (this.PanelElement != null && width > 100)
            {
                int count = ((int)(width / DesignWidth));//count of transverse
                return (PanelElement.ActualWidth - 4) / count;//Aliquot width
            }
            else
                return DesignWidth;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)=>  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}