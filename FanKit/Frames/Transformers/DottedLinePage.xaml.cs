﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace FanKit.Frames.Transformers
{
    public sealed partial class DottedLinePage : Page
    {
        DottedLineImage DottedLineImage;
        DottedLineBrush DottedLineBrush;

        float CanvasWidth = 1000;
        float CanvasHeight = 1000;
        Vector2 starting = new Vector2();
        TransformerRect _transformerRect;

        //@Construct
        public DottedLinePage()
        {
            this.InitializeComponent();
            this.Loaded += async (s2, e2) =>
            {
                this.MarkdownText1.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Transformers/DottedLinePage.xaml.txt");
                this.MarkdownText1.LinkClicked += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/FanKit-UWP/blob/master/FanKit/Frames/Transformers/DottedLinePage.xaml"));
                this.MarkdownText2.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Transformers/DottedLinePage.xaml.cs.txt");
                this.MarkdownText2.LinkClicked += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/FanKit-UWP/blob/master/FanKit/Frames/Transformers/DottedLinePage.xaml.cs"));
            };

            this.ResetButton.Tapped += (s, e) =>
            {
                using (var ds = this.DottedLineImage.CreateDrawingSession())
                {
                    ds.Clear(Windows.UI.Colors.Transparent);
                }
                this.DottedLineImage.Baking(this.CanvasAnimatedControl);
            };

            #region Draw


            //Canvas
            this.CanvasAnimatedControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
            };
            this.CanvasAnimatedControl.CreateResources += (sender, args) =>
            {
                CanvasRenderTarget canvasRenderTarget = new CanvasRenderTarget(sender, this.CanvasWidth, this.CanvasHeight);
                this.DottedLineImage = new DottedLineImage(canvasRenderTarget);
                this.DottedLineBrush = new DottedLineBrush(sender, 6);

                this.DottedLineImage.Baking(sender);
            };
            this.CanvasAnimatedControl.Draw += (sender, args) =>
            {
                args.DrawingSession.DrawDottedLine(sender, this.DottedLineBrush, this.DottedLineImage, this.CanvasWidth, this.CanvasHeight);
                
                Rect rect = this._transformerRect.ToRect();
                args.DrawingSession.DrawThickRectangle(rect);
            };
            this.CanvasAnimatedControl.Update += (sender, args) =>
            {
                this.DottedLineBrush.Update(1);
            };


            #endregion

            #region CanvasOperator


            //Single
            this.CanvasOperator.Single_Start += (point) =>
            {
                this.starting = point;
                this._transformerRect = new TransformerRect(point, point);
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                this._transformerRect = new TransformerRect(starting, point);
            };
            this.CanvasOperator.Single_Complete += (point) =>
            {
                using (var ds = this.DottedLineImage.CreateDrawingSession())
                {
                    ds.FillRectangle(this._transformerRect.ToRect(), Windows.UI.Colors.Gray);
                }
                this.DottedLineImage.Baking(this.CanvasAnimatedControl);

                this._transformerRect = new TransformerRect(Vector2.Zero, Vector2.Zero);
            };


            #endregion
        }
    }
}