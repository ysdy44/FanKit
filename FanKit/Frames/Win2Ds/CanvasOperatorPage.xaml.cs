﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Frames.Win2Ds
{
    public sealed partial class CanvasOperatorPage : Page
    {
        //Transform
        float CanvasWidth = 100;
        float CanvasHeight = 100;
        Vector2 Position;
        float Scale = 1f;
        
        //Right
        Vector2 rightStartPoint;
        Vector2 rightStartPosition;

        //Double
        Vector2 doubleStartCenter;
        Vector2 doubleStartPosition;
        float doubleStartScale;
        float doubleStartSpace;

        CanvasTextFormat RulerTextFormat = new CanvasTextFormat() { FontSize = 12, HorizontalAlignment = CanvasHorizontalAlignment.Center, VerticalAlignment = CanvasVerticalAlignment.Center, };

        public CanvasOperatorPage()
        {
            this.InitializeComponent();
            this.Loaded += async (sender, e) =>
            {
                this.MarkdownText1.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Win2Ds/CanvasOperatorPage.xaml.txt");
                this.MarkdownText2.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Win2Ds/CanvasOperatorPage.xaml.cs.txt");
                this.MarkdownText3.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Win2Ds/CanvasOperator.cs.txt");
            };


            //Canvas
            this.CanvasControl.SizeChanged += (s, e) =>
              {
                  if (e.NewSize == e.PreviousSize) return;                  
                  this.Position.X = (float)e.NewSize.Width / 2;
                  this.Position.Y = (float)e.NewSize.Height / 2;
              };
            this.CanvasControl.Draw += (sender, args) =>
            {
                this.RulerDraw(args.DrawingSession, this.Position, this.Scale);

                args.DrawingSession.Transform = Matrix3x2.CreateTranslation(-this.CanvasWidth / 2, -this.CanvasHeight / 2) * Matrix3x2.CreateScale(this.Scale) * Matrix3x2.CreateTranslation(this.Position);
                args.DrawingSession.FillRectangle(-this.CanvasWidth / 2, -this.CanvasHeight / 2, this.CanvasWidth, this.CanvasHeight, Windows.UI.Colors.Gray);
            };


            //Single
            this.CanvasOperator.Single_Start += (point) =>
            {
                this.Position = point;
                this.Invalidate("Single_Start", Visibility.Visible);
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                this.Position = point;
                this.Invalidate("Single_Delta");
            };
            this.CanvasOperator.Single_Complete += (point) =>
            {
                this.Position = point;
                this.Invalidate("Single_Complete", Visibility.Collapsed);
            };


            //Right
            this.CanvasOperator.Right_Start += (point) =>
            {
                this.rightStartPoint = point;
                this.rightStartPosition = this.Position;
                this.Invalidate("Right_Start", Visibility.Visible);
            };
            this.CanvasOperator.Right_Delta += (point) =>
            {
                this.Position = this.rightStartPosition - this.rightStartPoint + point;

                this.Invalidate("Right_Delta");
            };
            this.CanvasOperator.Right_Complete += (point) => this.Invalidate("Right_Complete", Visibility.Collapsed);


            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this.doubleStartCenter = (center - this.Position) / this.Scale + new Vector2(this.CanvasWidth / 2, this.CanvasHeight / 2);
                this.doubleStartPosition = this.Position;

                this.doubleStartSpace = space;
                this.doubleStartScale = this.Scale;

                this.Invalidate("Double_Start", Visibility.Visible);
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.Scale = this.doubleStartScale / this.doubleStartSpace * space;

                this.Position = center - (this.doubleStartCenter - new Vector2(this.CanvasWidth / 2, this.CanvasHeight / 2)) * this.Scale;

                this.Invalidate("Double_Delta");
            };
            this.CanvasOperator.Double_Complete += (center, space) => this.Invalidate("Double_Complete", Visibility.Collapsed);


            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
            {

                if (space > 0)
                {
                    if (this.Scale < 10f)
                    {
                        this.Scale *= 1.1f;
                        this.Position = point + (this.Position - point) * 1.1f;
                    }
                }
                else
                {
                    if (this.Scale > 0.1f)
                    {
                        this.Scale /= 1.1f;
                        this.Position = point + (this.Position - point) / 1.1f;
                    }
                }

                this.Invalidate("Wheel_Changed", Visibility.Visible);
            };
        }



        public void Invalidate(string text, Visibility? visibility = null)
        {
            this.CanvasControl.Invalidate();

            this.TipTextBlock.Text = text;
            if (visibility != null) this.TipBorder.Visibility = visibility ?? Visibility.Collapsed;
        }


        private void RulerDraw(CanvasDrawingSession ds, Vector2 position, float scale)
        {
            float ActualWidth = (float)this.CanvasControl. ActualWidth;
            float ActualHeight = (float)this.CanvasControl.ActualHeight;

            //line
            ds.DrawLine(20, 20, ActualWidth, 20, Windows.UI.Colors.Gray);//Horizontal
            ds.DrawLine(20, 20, 20, ActualHeight, Windows.UI.Colors.Gray);//Vertical

            //space
            float space = (float)(10 * scale);
            while (space < 10) space *= 5; 
            while (space > 100) space /= 5;
            float spaceFive = space * 5;

            //Horizontal
            for (float X = (float)position.X; X < ActualWidth; X += space) ds.DrawLine(X, 10, X, 20, Windows.UI.Colors.Gray);
            for (float X = (float)position.X; X > 20; X -= space) ds.DrawLine(X, 10, X, 20, Windows.UI.Colors.Gray);
            //Vertical
            for (float Y = (float)position.Y; Y < ActualHeight; Y += space) ds.DrawLine(10, Y, 20, Y, Windows.UI.Colors.Gray);
            for (float Y = (float)position.Y; Y > 20; Y -= space) ds.DrawLine(10, Y, 20, Y, Windows.UI.Colors.Gray);

            //Horizontal
            for (float X = (float)position.X; X < ActualWidth; X += spaceFive) ds.DrawLine(X, 10, X, 20, Windows.UI.Colors.Gray);
            for (float X = (float)position.X; X > 20; X -= spaceFive) ds.DrawLine(X, 10, X, 20, Windows.UI.Colors.Gray);
            //Vertical
            for (float Y = (float)position.Y; Y < ActualHeight; Y += spaceFive) ds.DrawLine(10, Y, 20, Y, Windows.UI.Colors.Gray);
            for (float Y = (float)position.Y; Y > 20; Y -= spaceFive) ds.DrawLine(10, Y, 20, Y, Windows.UI.Colors.Gray);

            //Horizontal
            for (float X = (float)position.X; X < ActualWidth; X += spaceFive) ds.DrawText(((int)(Math.Round((X - position.X) / scale))).ToString(), X, 10, Windows.UI.Colors.Gray, RulerTextFormat);
            for (float X = (float)position.X; X > 20; X -= spaceFive) ds.DrawText(((int)(Math.Round((X - position.X) / scale))).ToString(), X, 10, Windows.UI.Colors.Gray, RulerTextFormat);
            //Vertical
            for (float Y = (float)position.Y; Y < ActualHeight; Y += spaceFive) ds.DrawText(((int)(Math.Round((Y - position.Y) / scale))).ToString(), 10, Y, Windows.UI.Colors.Gray, RulerTextFormat);
            for (float Y = (float)position.Y; Y > 20; Y -= spaceFive) ds.DrawText(((int)(Math.Round((Y - position.Y) / scale))).ToString(), 10, Y, Windows.UI.Colors.Gray, RulerTextFormat);
        }       
    }
}
