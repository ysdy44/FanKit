﻿using FanKit.Win2Ds;
using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace FanKit.Frames.Win2Ds
{
    public sealed partial class DottedLinePage : Page
    {        
        DottedLine DottedLine;
        CanvasRenderTarget InPut;

        //Canvas
        bool IsRender = false;
        float CanvasWidth =1000;
        float CanvasHeight =1000;

        //Pointer
        bool IsMove;
        Point MoveStart;
        Point MoveEnd;

        public DottedLinePage()
        {
            this.InitializeComponent();
            this.Loaded += async (sender, e) =>
            {
                this.MarkdownText1.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Win2Ds/DottedLinePage.xaml.txt");
                this.MarkdownText2.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Win2Ds/DottedLinePage.xaml.cs.txt");
                this.MarkdownText3.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Win2Ds/DottedLine.cs.txt");
            };


            //Canvas
            this.CanvasAnimatedControl.SizeChanged += (s, e) =>
              {
                  if (e.NewSize == e.PreviousSize) return;

                  if (System.Math.Abs(e.NewSize.Width - this.CanvasWidth) > 10)
                  {
                      this.CanvasWidth = (float)e.NewSize.Width;
                      this.CanvasHeight = (float)e.NewSize.Height;

                      this.IsRender = true;
                  }
              };
            this.CanvasAnimatedControl.CreateResources += (sender, args) =>
            {
                this.DottedLine = new DottedLine(sender, 6, 1);
                this.InPut = new CanvasRenderTarget(sender, this.CanvasWidth, this.CanvasHeight);
                this.IsRender = true;
            };
            this.CanvasAnimatedControl.Draw += (sender, args) =>
            {
                this.DottedLine.Draw(sender, args.DrawingSession, new Rect(0, 0, this.CanvasWidth, this.CanvasHeight));
                if (this.IsMove) args.DrawingSession.DrawRectangle(new Rect(this.MoveStart, this.MoveEnd), Windows.UI.Colors.Gray);
            };
            this.CanvasAnimatedControl.Update += (sender, args) =>
            {
                this.DottedLine.Update();

                if (this.IsRender)
                {
                    this.IsRender = false;

                    this.InPut = new CanvasRenderTarget(this.CanvasAnimatedControl, this.CanvasWidth, this.CanvasHeight);
                    this.DottedLine.Render(sender, this.InPut);
                }
            };


            //Pointer
            this.CanvasAnimatedControl.PointerPressed += (s, e) =>
            {
                this.MoveStart = this.MoveEnd = e.GetCurrentPoint(this.CanvasAnimatedControl).Position;
                this.IsMove = true;
            };
            this.CanvasAnimatedControl.PointerMoved += (s, e) =>
            {
                if (this.IsMove)
                {
                    this.MoveEnd = e.GetCurrentPoint(this.CanvasAnimatedControl).Position;
                }
            };
            this.CanvasAnimatedControl.PointerReleased += (s, e) =>
            {
                if (this.IsMove)
                {
                    this.IsMove = false;
                    using (var ds = this.InPut.CreateDrawingSession())
                    {
                        ds.FillRectangle(new Rect(this.MoveStart, this.MoveEnd), Windows.UI.Colors.Gray);
                    }
                }
            };
        }  
    }
}