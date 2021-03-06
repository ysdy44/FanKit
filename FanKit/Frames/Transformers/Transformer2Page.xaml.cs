﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Frames.Transformers
{
    /// <summary>
    /// Page of <see cref="FanKit.Transformers.Transformer">.
    /// </summary>
    public sealed partial class Transformer2Page : Page
    {
        TransformerMode _mode;
        Vector2 _startingPoint;
        Transformer _startingTransformer;
        Layer _layer;

        class Layer
        {
            public CanvasBitmap Image;
            public TransformerMatrix TransformerMatrix;
        }

        #region DependencyProperty


        /// <summary> CanvasTransformer </summary>
        public CanvasTransformer _canvasTransformer
        {
            set
            {
                this.WidthRun.Text = string.Format("{0}", (int)value.Width);
                this.HeightRun.Text = string.Format("{0}", (int)value.Height);
                this.ScaleRun.Text = string.Format("{0}%", (int)(value.Scale * 100.0f));
                this.PositionRun.Text = string.Format("({0}, {1})", (int)value.Position.X, (int)value.Position.Y);
                this.RadianRun.Text = string.Format("{0}º", (int)(value.Radian * 180.0f / FanKit.Math.Pi));
            }
        }


        /// <summary> CanvasTransformer's radian. </summary>
        public double Radian
        {
            get { return (double)GetValue(RadianProperty); }
            set { SetValue(RadianProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Transformer2Page.Radian" /> dependency property. </summary>
        public static readonly DependencyProperty RadianProperty = DependencyProperty.Register(nameof(Radian), typeof(double), typeof(Transformer2Page), new PropertyMetadata(0.0d, (sender, e) =>
        {
            Transformer2Page con = (Transformer2Page)sender;

            if (e.NewValue is double value)
            {
                float radian = ((float)value) * FanKit.Math.Pi / 180.0f;
                con.CanvasTransformer.Radian = radian;

                con.CanvasTransformer.ReloadMatrix();
                con.CanvasControl.Invalidate();//Invalidate

                //DependencyProperty
                con._canvasTransformer = con.CanvasTransformer;
            }
        }));



        /// <summary> Transformer. </summary>
        public Transformer Transformer
        {
            get { return (Transformer)GetValue(TransformerProperty); }
            set { SetValue(TransformerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Transformer2Page.IsRatio" /> dependency property. </summary>
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(Transformer2Page), new PropertyMetadata(new Transformer(), (sender, e) =>
        {
            Transformer2Page con = (Transformer2Page)sender;

            if (e.NewValue is Transformer value)
            {
                con.LeftTopTextBlock.Text = string.Format("LeftTop: ({0},{1})", (int)value.LeftTop.X, (int)value.LeftTop.Y);
                con.RightTopTextBlock.Text = string.Format("RightTop: ({0},{1})", (int)value.RightTop.X, (int)value.RightTop.Y);
                con.RightBottomTextBlock.Text = string.Format("RightBottom: ({0},{1})", (int)value.RightBottom.X, (int)value.RightBottom.Y);
                con.LeftBottomTextBlock.Text = string.Format("LeftBottom: ({0},{1})", (int)value.LeftBottom.X, (int)value.LeftBottom.Y);
            }
        }));


        /// <summary> Maintain a ratio when scaling. </summary>
        public bool IsRatio
        {
            get { return (bool)GetValue(IsRatioProperty); }
            set { SetValue(IsRatioProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Transformer2Page.IsRatio" /> dependency property. </summary>
        public static readonly DependencyProperty IsRatioProperty = DependencyProperty.Register(nameof(IsRatio), typeof(bool), typeof(Transformer2Page), new PropertyMetadata(false));


        /// <summary> Scaling around the center. </summary>
        public bool IsCenter
        {
            get { return (bool)GetValue(IsCenterProperty); }
            set { SetValue(IsCenterProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Transformer2Page.IsCenter" /> dependency property. </summary>
        public static readonly DependencyProperty IsCenterProperty = DependencyProperty.Register(nameof(IsCenter), typeof(bool), typeof(Transformer2Page), new PropertyMetadata(false));


        /// <summary> Step Frequency when spinning. </summary>
        public bool IsStepFrequency
        {
            get { return (bool)GetValue(IsStepFrequencyProperty); }
            set { SetValue(IsStepFrequencyProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Transformer2Page.IsStepFrequency" /> dependency property. </summary>
        public static readonly DependencyProperty IsStepFrequencyProperty = DependencyProperty.Register(nameof(IsStepFrequency), typeof(bool), typeof(Transformer2Page), new PropertyMetadata(false));


        #endregion


        //@Construct
        public Transformer2Page()
        {
            this.InitializeComponent();
            this.Loaded += async (s2, e2) =>
            {
                this.MarkdownText1.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Transformers/Transformer2Page.xaml.txt");
                this.MarkdownText1.LinkClicked += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/FanKit-UWP/blob/master/FanKit/Frames/Transformers/Transformer2Page.xaml"));
                this.MarkdownText2.Text = await FanKit.Samples.File.GetFile("ms-appx:///TXT/Transformers/Transformer2Page.xaml.cs.txt");
                this.MarkdownText2.LinkClicked += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/FanKit-UWP/blob/master/FanKit/Frames/Transformers/Transformer2Page.xaml.cs"));
            };

            this.ResetButton.Tapped += (s, e) =>
            {
                Size size = new Size(this.CanvasControl.ActualWidth, this.CanvasControl.ActualHeight);

                this.CanvasTransformer.Position = new Vector2((float)size.Width / 2, (float)size.Height / 2);
                this.CanvasTransformer.Scale = 1;
                this.CanvasTransformer.Radian = 0;

                this.CanvasTransformer.ReloadMatrix();

                //DependencyProperty
                this._canvasTransformer = this.CanvasTransformer;


                Transformer transformer = this.Reset(this._layer.Image.SizeInPixels.Width, this._layer.Image.SizeInPixels.Height, this.CanvasTransformer.Width, this.CanvasTransformer.Height);
                this._layer.TransformerMatrix.Destination = transformer;

                //DependencyProperty
                this.Transformer = transformer;


                this.CanvasControl.Invalidate();//Invalidate
            };


            //Canvas
            this.CanvasControl.CreateResources += (sender, args) => args.TrackAsyncAction(this.CreateResourcesAsync(sender).AsAsyncAction());
            this.CanvasControl.Draw += (sender, args) =>
            {
                //CanvasTransformer
                Matrix3x2 matrix = this.CanvasTransformer.GetMatrix();
                Matrix3x2 canvasToVirtualMatrix = this.CanvasTransformer.GetMatrix(MatrixTransformerMode.CanvasToVirtual);

                //Transformer
                ICanvasImage source = this._layer.Image;
                Matrix3x2 transformMatrix = this._layer.TransformerMatrix.GetMatrix();
                Transformer transformer = this._layer.TransformerMatrix.Destination;

                //Draw
                args.DrawingSession.DrawCrad(new CompositeEffect
                {
                    Sources =
                    {
                        new ColorSourceEffect{ Color = Windows.UI.Colors.White },
                        new Transform2DEffect
                        {
                            Source = source,
                            TransformMatrix = transformMatrix * canvasToVirtualMatrix
                        }
                    }
                }, this.CanvasTransformer);
                args.DrawingSession.DrawBoundNodes(transformer, matrix);
            };


            //Single
            this.CanvasOperator.Single_Start += (point) =>
            {
                this._startingPoint = point;

                //Controller      
                this._layer.TransformerMatrix.CacheTransform();

                Transformer transformer = this._layer.TransformerMatrix.Destination;
                this._startingTransformer = transformer;

                Matrix3x2 matrix = this.CanvasTransformer.GetMatrix();
                this._mode = Transformer.ContainsNodeMode(point, transformer, matrix);

                this.CanvasControl.Invalidate();//Invalidate
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                bool isRatio = this.IsRatio;
                bool isCenter = this.IsCenter;
                bool isStepFrequency = this.IsStepFrequency;

                //Controller
                Matrix3x2 inverseMatrix = this.CanvasTransformer.GetInverseMatrix();
                Transformer transformer = Transformer.Controller(this._mode, _startingPoint, point, this._startingTransformer, inverseMatrix, isRatio, isCenter, isStepFrequency);

                this._layer.TransformerMatrix.Destination = transformer;

                //DependencyProperty
                this.Transformer = transformer;

                this.CanvasControl.Invalidate();//Invalidate
            };
            this.CanvasOperator.Single_Complete += (point) =>
            {
                this.CanvasControl.Invalidate();//Invalidate
            };

            //Right
            this.CanvasOperator.Right_Start += (point) =>
            {
                this.CanvasTransformer.CacheMove(point);
            };
            this.CanvasOperator.Right_Delta += (point) =>
            {
                this.CanvasTransformer.Move(point);
                this.CanvasControl.Invalidate();
                //DependencyProperty
                this._canvasTransformer = this.CanvasTransformer;
            };
            this.CanvasOperator.Right_Complete += (point) =>
            {
                this.CanvasTransformer.Move(point);
                this.CanvasControl.Invalidate();
            };

            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this.CanvasTransformer.CachePinch(center, space);
                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.CanvasTransformer.Pinch(center, space);
                this.CanvasControl.Invalidate();
                //DependencyProperty
                this._canvasTransformer = this.CanvasTransformer;
            };
            this.CanvasOperator.Double_Complete += (center, space) =>
            {
                this.CanvasControl.Invalidate();
            };

            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
            {
                if (space > 0)
                    this.CanvasTransformer.ZoomIn(point);
                else
                    this.CanvasTransformer.ZoomOut(point);

                this.CanvasControl.Invalidate();
                //DependencyProperty
                this._canvasTransformer = this.CanvasTransformer;
            };
        }

        private async Task CreateResourcesAsync(CanvasControl sender)
        {
            Size size = new Size(sender.ActualWidth, sender.ActualHeight);

            this.CanvasTransformer.Position = new Vector2((float)size.Width / 2, (float)size.Height / 2);
            this.CanvasTransformer.Scale = 1;
            this.CanvasTransformer.Radian = 0;

            this.CanvasTransformer.ReloadMatrix();

            //DependencyProperty
            this._canvasTransformer = this.CanvasTransformer;


            //Bitmap
            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(sender, "Icon/Avatar.jpg");
            TransformerMatrix transformerMatrix = new TransformerMatrix
            {
                Source = new Transformer(bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height, Vector2.Zero),
                Destination = this.Reset(bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height, this.CanvasTransformer.Width, this.CanvasTransformer.Height),
            };

            //DependencyProperty
            this.Transformer = transformerMatrix.Destination;

            //Layer
            this._layer = new Layer
            {
                TransformerMatrix = transformerMatrix,
                Image = bitmap,
            };
        }

        private Transformer Reset(float bitmapWidth, float bitmapHeight, float controlWidth, float controlHeight)
        {
            Vector2 center = new Vector2(controlWidth, controlHeight) / 2.0f;
            float scale = System.Math.Min(controlWidth / bitmapWidth, controlHeight / bitmapHeight);
            float width = scale * bitmapWidth / 3.0f / 2.0f;
            float height = scale * bitmapHeight / 3.0f / 2.0f;

            Transformer destination = new Transformer
            {
                LeftTop = center + new Vector2(-width, -height),
                RightTop = center + new Vector2(+width, -height),
                RightBottom = center + new Vector2(+width, +height),
                LeftBottom = center + new Vector2(-width, +height),
            };
            return destination;
        }

    }
}