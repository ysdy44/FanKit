﻿using System.ComponentModel;
using Windows.UI.Xaml;

namespace FanKit.Frames.Template
{
    /// <summary>
    /// Constrain GridItem's width and height.so as to reach a most appropriate proportion.
    /// </summary>
    public class DataTemplateAdaptiver : DependencyObject, INotifyPropertyChanged
    {

        #region DependencyProperty

        /// <summary> Destination layout panel. </summary>
        public FrameworkElement PanelElement
        {
            get { return (FrameworkElement)GetValue(PanelElementProperty); }
            set { SetValue(PanelElementProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DataTemplateAdaptiver.PanelElement" /> dependency property. </summary>
        public static DependencyProperty PanelElementProperty = DependencyProperty.Register(nameof(PanelElement), typeof(FrameworkElement), typeof(DataTemplateAdaptiver), new PropertyMetadata(null, (sender, e) =>
        {
            DataTemplateAdaptiver con = (DataTemplateAdaptiver)sender;

            if (e.NewValue != e.OldValue)
            {
                con.PanelElement.SizeChanged += (s, e2) =>
                {
                    if (e2.NewSize.Width > 100 && e2.NewSize.Height > 100)
                    {
                        if (e2.NewSize == e2.PreviousSize) return;

                        con.OnSizeChanged(e2.NewSize.Width);
                    }
                };
            }
        }));

        #endregion


        /// <summary>
        /// Width
        /// </summary>
        public double SizeWidth
        {
            get => this.sizeWidth;
            set
            {
                this.sizeWidth = value;
                this.OnPropertyChanged(nameof(this.SizeWidth));//Notify
            }
        }
        public double sizeWidth;

        /// <summary>
        /// Height
        /// </summary>
        public double SizeHeight
        {
            get => this.sizeHeight;
            set
            {
                this.sizeHeight = value;
                this.OnPropertyChanged(nameof(this.SizeHeight));//Notify
            }
        }
        public double sizeHeight;


        /// <summary>
        /// Call when <see cref="DataTemplateAdaptiver.PanelElement"> size changed.
        /// </summary>
        /// <param name="width"> width </param>
        public void OnSizeChanged(double width)
        {
            double designWidth = System.Math.Sqrt(System.Math.Sqrt(width)) * 24d;//design Width
            int count = ((int)(width / designWidth));//count of transverse
            double finalwidth = (width - 4) / count;//Aliquot width

            //Size
            this.SizeWidth = finalwidth;
            this.SizeHeight = finalwidth * 1.6d;
        }


        //Notify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
