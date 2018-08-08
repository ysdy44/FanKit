﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace FanKit.Frames.Colors
{
    /// <summary> 
    /// Palette Hue
    /// </summary>
    public class PaletteHue : PaletteBase
    {
        public LinearGradientBrush SliderBrush = new LinearGradientBrush(new GradientStopCollection()
        {
            new GradientStop() { Offset = 0 },
             new GradientStop() { Offset = 0.16666667 },
             new GradientStop() { Offset = 0.33333333 },
             new GradientStop() { Offset = 0.5 },
             new GradientStop() { Offset = 0.66666667 },
             new GradientStop() { Offset = 0.83333333 },
             new GradientStop() { Offset = 1 },
        }, 0)
        {
            StartPoint = new Point(0, 0.5),
            EndPoint = new Point(1, 0.5),
        };



        public PaletteHue()
        {
            this.Name = "Hue";
            this.Unit = "º";
            this.Minimum = 0;
            this.Maximum = 360;
        }

        public override HSL GetHSL(HSL HSL, int value) => new HSL(HSL.A, value, HSL.S, HSL.L);
        public override int GetValue(HSL HSL) => (int)HSL.H;

        public override Brush GetSliderBrush(HSL HSL)
        {
            byte A = HSL.A;
            double H = HSL.H;
            double S = HSL.S;
            double L = HSL.L;

            this.SliderBrush.GradientStops[6].Color =
            this.SliderBrush.GradientStops[0].Color = HSLtoRGB(A, 0, S, L);
            this.SliderBrush.GradientStops[1].Color = HSLtoRGB(A, 60, S, L);
            this.SliderBrush.GradientStops[2].Color = HSLtoRGB(A, 120, S, L);
            this.SliderBrush.GradientStops[3].Color = HSLtoRGB(A, 180, S, L);
            this.SliderBrush.GradientStops[4].Color = HSLtoRGB(A, 240, S, L);
            this.SliderBrush.GradientStops[5].Color = HSLtoRGB(A, 300, S, L);

            return SliderBrush;
        }

        public override void Draw(CanvasControl CanvasControl, CanvasDrawingSession ds, HSL HSL, Vector2 Center, float SquareHalfWidth, float SquareHalfHeight)
        {
            //Palette
            Rect rect = new Rect(Center.X - SquareHalfWidth, Center.Y - SquareHalfHeight, SquareHalfWidth * 2, SquareHalfHeight * 2);
            ds.FillRoundedRectangle(rect, 4, 4, new CanvasLinearGradientBrush(CanvasControl, Windows.UI.Colors.White, HSLtoRGB(HSL.H)) { StartPoint = new Vector2(Center.X - SquareHalfWidth, Center.Y), EndPoint = new Vector2(Center.X + SquareHalfWidth, Center.Y) });
            ds.FillRoundedRectangle(rect, 4, 4, new CanvasLinearGradientBrush(CanvasControl, Windows.UI.Colors.Transparent, Windows.UI.Colors.Black) { StartPoint = new Vector2(Center.X, Center.Y - SquareHalfHeight), EndPoint = new Vector2(Center.X, Center.Y + SquareHalfHeight) });
            ds.DrawRoundedRectangle(rect, 4, 4, Windows.UI.Colors.Gray);

            //Thumb 
            float px = ((float)HSL.S - 50) * SquareHalfWidth / 50 + Center.X;
            float py = (50 - (float)HSL.L) * SquareHalfHeight / 50 + Center.Y;
            ds.DrawCircle(px, py, 8, Windows.UI.Colors.Black, 4);
            ds.DrawCircle(px, py, 8, Windows.UI.Colors.White, 2);
        }
        public override HSL Delta(HSL HSL, Vector2 v, float SquareHalfWidth, float SquareHalfHeight)
        {
            double S = 50 + v.X * 50 / SquareHalfWidth;
            double L = 50 - v.Y * 50 / SquareHalfHeight;

            return new HSL(HSL.A, HSL.H, S, L);
        }
    }
}