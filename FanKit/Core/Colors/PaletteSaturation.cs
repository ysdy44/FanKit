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
    /// <summary> Saturation </summary>
    public class PaletteSaturation : PaletteBase
    {
        public CanvasGradientStop[] BackgroundStops = new CanvasGradientStop[]
        {
            new CanvasGradientStop { Position = 0.0f, Color =  Windows.UI.Colors.Red },
            new CanvasGradientStop { Position = 0.16666667f, Color =Windows.UI.Colors.Yellow},
            new CanvasGradientStop { Position = 0.33333333f, Color = Color.FromArgb(255,0,255,0) },
            new CanvasGradientStop { Position = 0.5f, Color = Windows.UI.Colors.Cyan },
            new CanvasGradientStop { Position = 0.66666667f, Color = Windows.UI.Colors.Blue},
            new CanvasGradientStop { Position = 0.83333333f, Color =  Windows.UI.Colors.Magenta },
            new CanvasGradientStop { Position = 1.0f, Color =  Windows.UI.Colors.Red },
        };
        public CanvasGradientStop[] ForegroundStops = new CanvasGradientStop[]
        {
            new CanvasGradientStop { Position = 0.0f, Color = Windows.UI.Colors.Transparent },
            new CanvasGradientStop { Position = 1.0f, Color = Windows.UI.Colors.Black }
        };

        public PaletteSaturation()
        {
            this.Name = "Saturation";
            this.Unit = "%";
            this.Minimum = 0;
            this.Maximum = 100;
        }

        public override HSL GetHSL(HSL HSL, int value) => new HSL(HSL.A, HSL.H, value, HSL.L);
        public override int GetValue(HSL HSL) => (int)HSL.S;
        public override GradientStopCollection GetSliderBrush(HSL HSL)
        {
            byte A = HSL.A;
            double H = HSL.H;
            double S = HSL.S;
            double L = HSL.L;

            return new GradientStopCollection()
            {
                new GradientStop()
                {
                    Offset = 0,
                    Color = HSL.HSLtoRGB(A, H, 0.0d, L)
                },
               new GradientStop()
                {
                    Offset = 1,
                    Color =HSL.HSLtoRGB(A, H, 100.0d, L)
                },
            };
        }

        public override void Draw(CanvasControl CanvasControl, CanvasDrawingSession ds, HSL HSL, Vector2 Center, float SquareHalfWidth, float SquareHalfHeight)
        {
            //Palette
            Rect rect = new Rect(Center.X - SquareHalfWidth, Center.Y - SquareHalfHeight, SquareHalfWidth * 2, SquareHalfHeight * 2);
            using (CanvasLinearGradientBrush rainbow = new CanvasLinearGradientBrush(CanvasControl, this.BackgroundStops))
            {
                rainbow.StartPoint = new Vector2(Center.X - SquareHalfWidth, Center.Y);
                rainbow.EndPoint = new Vector2(Center.X + SquareHalfWidth, Center.Y);
                ds.FillRoundedRectangle(rect, 4, 4, rainbow);
            }
            using (CanvasLinearGradientBrush brush = new CanvasLinearGradientBrush(CanvasControl, this.ForegroundStops))
            {
                brush.StartPoint = new Vector2(Center.X, Center.Y - SquareHalfHeight);
                brush.EndPoint = new Vector2(Center.X, Center.Y + SquareHalfHeight);
                ds.FillRoundedRectangle(rect, 4, 4, brush);
            }
            ds.DrawRoundedRectangle(rect, 4, 4, Windows.UI.Colors.Gray);

            //Thumb 
            float px = ((float)HSL.H - 180) * SquareHalfWidth / 180 + Center.X;
            float py = ((float)(50 - HSL.L)) * SquareHalfHeight / 50 + Center.Y;
            ds.DrawCircle(px, py, 8, Windows.UI.Colors.Black, 4);
            ds.DrawCircle(px, py, 8, Windows.UI.Colors.White, 2);
        }
        public override HSL Delta(HSL HSL, Vector2 v, float SquareHalfWidth, float SquareHalfHeight)
        {
            double H = v.X * 180 / SquareHalfWidth + 180;
            double L = 50 - v.Y * 50 / SquareHalfHeight;
            return new HSL(HSL.A, H, HSL.S, L);
        }
    }

}
