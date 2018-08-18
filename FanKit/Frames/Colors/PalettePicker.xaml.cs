﻿using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace FanKit.Frames.Colors
{
    public partial class PalettePicker : UserControl
    {
        //Delegate
        public delegate void ColorChangeHandler(object sender, Color value);
        public event ColorChangeHandler ColorChange = null;

        //Value
        public Vector2 Center = new Vector2(50, 50);
        public float SquareWidth = 100;
        public float SquareHeight = 100;
        public float SquareHalfWidth => this.SquareWidth / 2;
        public float SquareHalfHeight => this.SquareHeight / 2;
        public float StrokePadding = 12;

        #region DependencyProperty


        protected Color color = Color.FromArgb(255, 255, 255, 255);
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                this.HSL =HSL.RGBtoHSL(value);
            }
        }


        public HSL HSL
        {
            get { return (HSL)GetValue(HSLProperty); }
            set { SetValue(HSLProperty, value); }
        }
        public static readonly DependencyProperty HSLProperty = DependencyProperty.Register(nameof(HSL), typeof(HSL), typeof(PalettePicker), new PropertyMetadata(new HSL(255, 360, 100, 100), new PropertyChangedCallback(HSLOnChanged)));
        private static void HSLOnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PalettePicker con = (PalettePicker)sender;

            if (e.NewValue is HSL NewValue) con.HSLChanged(NewValue);
        }
        public virtual void HSLChanged(HSL value)
        {
            byte A = HSL.A;
            double H = HSL.H;
            double S = HSL.S;
            double L = HSL.L;

            this.Slider.Value = this.Picker.Value = this.PaletteBase.GetValue(value);
            this.Slider.SliderBackground = this.PaletteBase.GetSliderBrush(value);

            this.CanvasControl.Invalidate();

            this.color = HSL.HSLtoRGB(A,H,S,L);
            this.ColorChange?.Invoke(this, this.Color);
        }


        #endregion


        private PaletteBase paletteBase;
        public PaletteBase PaletteBase
        {
            get => paletteBase;
            set
            {
                if (value != null)
                {
                    this.Picker.Unit = value.Unit;
                    this.Slider.Minimum = this.Picker.Minimum = value.Minimum;
                    this.Slider.Maximum = this.Picker.Maximum = value.Maximum;

                    this.Slider.Value = this.Picker.Value = value.GetValue(this.HSL);
                    this.Slider.SliderBackground = value.GetSliderBrush(this.HSL);

                    this.CanvasControl.Invalidate();

                    this.paletteBase = value;
                }
            }
        }
        public List<PaletteBase> PaletteBases = new List<PaletteBase>
        {
            new PaletteHue(),
            new PaletteSaturation(),
            new PaletteLightness(),
        };

        public PalettePicker()
        {
            this.InitializeComponent();

            this.ComboBox.ItemsSource = this.PaletteBases;
            this.ComboBox.SelectionChanged += (s, e) => this.PaletteBase = this.PaletteBases[this.ComboBox.SelectedIndex];
            this.ComboBox.Loaded += (s, e) => this.ComboBox.SelectedIndex = 0;
        }


        private void Picker_ValueChange(object sender, int Value)
        {
            if (this.PaletteBase!=null) this.HSL = this.PaletteBase.GetHSL(this.HSL, Value);
        }
        private void Slider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.PaletteBase != null) this.HSL = this.PaletteBase.GetHSL(this.HSL, (int)e.NewValue);
        }


        private void CanvasControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Center = e.NewSize.ToVector2() / 2;

            this.SquareWidth = (float)e.NewSize.Width - this.StrokePadding * 2;
            this.SquareHeight = (float)e.NewSize.Height - this.StrokePadding * 2;
        }
        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args) =>this.PaletteBase.Draw(this.CanvasControl, args.DrawingSession,this.HSL,this.Center,this.SquareHalfWidth,this.SquareHalfHeight);


        protected bool IsPalette = false;
        protected Vector2 v;
        private void CanvasControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            v = e.Position.ToVector2() - this.Center;

            this.IsPalette = Math.Abs(v.X) < this.SquareWidth && Math.Abs(v.Y) < this.SquareHeight;
        }
        private void CanvasControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (this.IsPalette)
            {
                v += e.Delta.Translation.ToVector2();

                this.HSL = this.PaletteBase.Delta(this.HSL, v, this.SquareHalfWidth, this.SquareHalfHeight);
            }
        }
        private void CanvasControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) => this.IsPalette = false;
        
    }
}















