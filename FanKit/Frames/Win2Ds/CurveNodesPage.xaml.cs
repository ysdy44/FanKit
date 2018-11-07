﻿using FanKit.Library.Win2Ds;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FanKit.Frames.Win2Ds
{
    public sealed partial class CurveNodesPage : Page
    {
        //CurveNodes
        CurveNodes CurveNode = new CurveNodes();

        public CurveNodesPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.MarkdownText1.Text = await FanKit.Library.File.GetFile("ms-appx:///TXT/Win2Ds/CurveNodesPage.xaml.txt");
            this.MarkdownText2.Text = await FanKit.Library.File.GetFile("ms-appx:///TXT/Win2Ds/CurveNodesPage.xaml.cs.txt");
            this.MarkdownText3.Text = await FanKit.Library.File.GetFile("ms-appx:///TXT/Win2Ds/CurveNodes.cs.txt");
        }

        #region UI

        //PenMode & NodeMode
        private void PenModeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.NodeModeButton.IsChecked = false;
            this.CurveNode.EditMode = NodeEditMode.Add;
        }
        private void NodeModeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.PenModeButton.IsChecked = false;
            this.CurveNode.EditMode = NodeEditMode.EditMove;
        }

        //Button
        private void Remove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurveNode.Remove();
            this.CanvasControl.Invalidate();
        }
        private void Add_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurveNode.Interpolation();
            this.CanvasControl.Invalidate();
        }
        private void Sharp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurveNode.Sharp();
            this.CanvasControl.Invalidate();
        }
        private void Smooth_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurveNode.Smooth();
            this.CanvasControl.Invalidate();
        }

        //Radio
        private void Mirrored_Tapped(object sender, TappedRoutedEventArgs e) => this.CurveNode.ControlMode = NodeControlMode.Mirrored;
        private void Disconnected_Tapped(object sender, TappedRoutedEventArgs e) => this.CurveNode.ControlMode = NodeControlMode.Disconnected;
        private void Asymmetric_Tapped(object sender, TappedRoutedEventArgs e) => this.CurveNode.ControlMode = NodeControlMode.Asymmetric;


        #endregion

        #region Canvas


        private void CanvasControl_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            Vector2 center = new Vector2((float)(sender.ActualWidth / 2), (float)(sender.ActualHeight / 2));
            this.CurveNode.Nodes.Add(new Node(new Vector2(-144.6047f, -138.5997f) + center, new Vector2(-144.6047f, -138.5997f) + center, new Vector2(-144.6047f, -138.5997f) + center));
            this.CurveNode.Nodes.Add(new Node(new Vector2(13.37953f, -95.3983f) + center, new Vector2(35.38268f, -57.79712f) + center, new Vector2(-8.623611f, -132.9995f) + center, NodeChooseMode.Vector));
            this.CurveNode.Nodes.Add(new Node(new Vector2(-12.58583f, 87.00745f) + center, new Vector2(9.285034f, 126.0071f) + center, new Vector2(-34.4567f, 48.00778f) + center, NodeChooseMode.Vector));
            this.CurveNode.Nodes.Add(new Node(new Vector2(144.6047f, 138.5997f) + center, new Vector2(144.6047f, 138.5997f) + center, new Vector2(144.6047f, 138.5997f) + center));
        }
        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            this.CurveNode.Draw(sender, args.DrawingSession);
        }


        #endregion
                     
        #region Pointer


        bool IsMove;
        private void CanvasControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.IsMove = true;

            this.CurveNode.Operator_Start(e.GetCurrentPoint(this.CanvasControl).Position.ToVector2());
            this.CanvasControl.Invalidate();
        }
        private void CanvasControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.IsMove)
            {
                this.CurveNode.Operator_Delta(e.GetCurrentPoint(this.CanvasControl).Position.ToVector2());
                this.CanvasControl.Invalidate();
            }
        }
        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (this.IsMove)
            {
                this.IsMove = false;

                this.CurveNode.Operator_Complete(e.GetCurrentPoint(this.CanvasControl).Position.ToVector2());
                this.CanvasControl.Invalidate();
            }

            bool IsEnabled = this.CurveNode.IsAnyChoose;
            this.Remove.IsEnabled = IsEnabled;
            this.Add.IsEnabled = IsEnabled;
            this.Sharp.IsEnabled = IsEnabled;
            this.Smooth.IsEnabled = IsEnabled;
        }



        #endregion
                              
    }
}
