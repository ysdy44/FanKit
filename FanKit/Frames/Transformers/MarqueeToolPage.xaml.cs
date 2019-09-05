﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Frames.Transformers
{
    /// <summary>
    /// Page of <see cref="FanKit.Transformers.MarqueeTool">.
    /// </summary>
    public sealed partial class MarqueeToolPage : Page
    {
        CanvasRenderTarget CanvasRenderTarget;

        private MarqueeToolType toolType;
        public MarqueeToolType ToolType
        {
            get => this.toolType;
            set
            {
                this.RectangularToolButton.IsChecked = value == MarqueeToolType.Rectangular;
                this.EllipticalToolButton.IsChecked = value == MarqueeToolType.Elliptical;
                this.PolygonalToolButton.IsChecked = value == MarqueeToolType.Polygonal;
                this.FreeHandToolButton.IsChecked = value == MarqueeToolType.FreeHand;

                if (value != MarqueeToolType.Polygonal)
                {
                    this._marqueeTool.IsStarted = false;
                    this._marqueeTool.Points.Clear();
                    this.CanvasControl.Invalidate();
                }

                this.toolType = value;
            }
        }

        public MarqueeCompositeMode CompositeMode2;

        MarqueeTool _marqueeTool = new MarqueeTool();

        Vector2 _startingPoint = new Vector2();

        int _canvasWidth = 1000;
        int _canvasHeight = 1000;

        #region DependencyProperty

        
        /// <summary> Scaling around the center. </summary>
        public bool IsCenter
        {
            get { return (bool)GetValue(IsCenterProperty); }
            set { SetValue(IsCenterProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerPage.IsCenter" /> dependency property. </summary>
        public static readonly DependencyProperty IsCenterProperty = DependencyProperty.Register(nameof(IsCenter), typeof(bool), typeof(MarqueeToolPage), new PropertyMetadata(false));

        /// <summary> Maintain a ratio when scaling. </summary>
        public bool IsSquare
        {
            get { return (bool)GetValue(IsSquareProperty); }
            set { SetValue(IsSquareProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerPage.IsSquare" /> dependency property. </summary>
        public static readonly DependencyProperty IsSquareProperty = DependencyProperty.Register(nameof(IsSquare), typeof(bool), typeof(MarqueeToolPage), new PropertyMetadata(false));


        #endregion

        //@Construct
        public MarqueeToolPage()
        {
            this.InitializeComponent();
            this.Loaded +=async (s2, e2) =>
            {
                this.MarkdownText1.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Transformers/MarqueeToolPage.xaml.txt");
                this.MarkdownText1.LinkClicked += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/FanKit-UWP/blob/master/FanKit/Frames/Transformers/IndicatorControlPage.xaml"));
                this.MarkdownText2.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Transformers/MarqueeToolPage.xaml.cs.txt");
                this.MarkdownText2.LinkClicked += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/FanKit-UWP/blob/master/FanKit/Frames/Transformers/IndicatorControlPage.xaml.cs"));
            };

            this.ResetButton.Tapped += (s, e) =>
            {
                this.CanvasRenderTarget = new CanvasRenderTarget(this.CanvasControl, this._canvasWidth, this._canvasHeight);
                this.CanvasControl.Invalidate();
            };

            //Tool
            this.ToolType = MarqueeToolType.Rectangular;
            this.RectangularToolButton.Tapped += (s, e) => this.ToolType = MarqueeToolType.Rectangular;
            this.EllipticalToolButton.Tapped += (s, e) => this.ToolType = MarqueeToolType.Elliptical;
            this.PolygonalToolButton.Tapped += (s, e) => this.ToolType = MarqueeToolType.Polygonal;
            this.FreeHandToolButton.Tapped += (s, e) => this.ToolType = MarqueeToolType.FreeHand;

            //Composite
            this.NewRadioButton.Tapped += (s, e) => this.CompositeMode2 = MarqueeCompositeMode.New;
            this.AddRadioButton.Tapped += (s, e) => this.CompositeMode2 = MarqueeCompositeMode.Add;
            this.SubtractRadioButton.Tapped += (s, e) => this.CompositeMode2 = MarqueeCompositeMode.Subtract;
            this.IntersectRadioButton.Tapped += (s, e) => this.CompositeMode2 = MarqueeCompositeMode.Intersect;

            #region Draw


            //Canvas  
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this._canvasWidth = (int)e.NewSize.Width;
                this._canvasHeight = (int)e.NewSize.Height;
            };
            this.CanvasControl.CreateResources += (sender, args) =>
            {
                int width = 1000;
                int height = 1000;

                this._canvasWidth = width;
                this._canvasHeight = height;
                this.CanvasRenderTarget = new CanvasRenderTarget(sender, width, height);
            };
            this.CanvasControl.Draw += (sender, args) =>
            {
                var mask = this.CanvasRenderTarget;
                args.DrawingSession.DrawImage(mask);

                //MarqueeTool
                args.DrawingSession.DrawMarqueeTool(sender, this.ToolType, this._marqueeTool);
            };


            #endregion


            #region CanvasOperator


            //Single
            this.CanvasOperator.Single_Start += (point) =>
            {
                Vector2 canvasPoint = point;
                this._startingPoint = point;

                //MarqueeTool
                this._marqueeTool.Start(canvasPoint, this.ToolType, this.IsCenter, this.IsSquare);

                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                Vector2 canvasStartingPoint = this._startingPoint;
                Vector2 canvasPoint = point;

                //MarqueeTool
                this._marqueeTool.Delta(canvasStartingPoint, canvasPoint, this.ToolType, this.IsCenter, this.IsSquare);

                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Complete += (point) =>
            {
                Vector2 canvasStartingPoint = this._startingPoint;
                Vector2 canvasPoint = point;

                //MarqueeTool
                bool redraw = this._marqueeTool.Complete(canvasStartingPoint, canvasPoint, this.ToolType, this.IsCenter, this.IsSquare);

                if (redraw)
                {
                    using (CanvasDrawingSession drawingSession = this.CanvasRenderTarget.CreateDrawingSession())
                    {
                        //MarqueeTool
                        drawingSession.FillMarqueeMaskl(this.CanvasControl, this.ToolType, this._marqueeTool, this.CanvasRenderTarget.Bounds, this.CompositeMode2);
                    }
                }

                this.CanvasControl.Invalidate();
            };


            #endregion

        }

    }
}