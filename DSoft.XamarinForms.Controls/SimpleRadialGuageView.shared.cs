using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DSoft.XamarinForms.Controls
{
    public class SimpleRadialGuageView : ContentView
    {
        #region fields and Properties

        private readonly SKCanvasView _canvasView = new SKCanvasView();
        private readonly Frame _container = new Frame()
        {
            CornerRadius = 20,
            HasShadow = false,
            BackgroundColor = Color.Transparent,
            
        };

        public double CurrentSweep => EndAngle * (Percent / 100);

        #endregion

        #region Paint objects

        private SKPaint ArcPaintBack => new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = ScaleBackgroundLineWidth,
            Color = ScaleBackgroundColor.ToSKColor(),
            IsAntialias = true,
        };

        private SKPaint ArcPaintBackRound => new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = ScaleBackgroundColor.ToSKColor(),
            IsAntialias = true,
        };

        private SKPaint ArcPaintRound => new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = ScaleForegroundColor.ToSKColor(),
            IsAntialias = true,
        };

        private SKPaint ArcPaint => new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = ScaleForegroundLineWidth,
            Color = ScaleForegroundColor.ToSKColor(),
            IsAntialias = true,
        };

        #endregion

        #region Bindable Properties

        #region ScaleBackgroundColor

        public static readonly BindableProperty ScaleBackgroundColorProperty = BindableProperty.Create(
           nameof(ScaleBackgroundColor), typeof(Color), typeof(SimpleRadialGuageView), Color.FromHex("#343c44"), propertyChanged: RedrawCanvas);

        public Color ScaleBackgroundColor
        {
            get => (Color)GetValue(ScaleBackgroundColorProperty);
            set => SetValue(ScaleBackgroundColorProperty, value);
        }

        #endregion

        #region ScaleBackgroundLineWidth

        public static readonly BindableProperty ScaleBackgroundLineWidthProperty = BindableProperty.Create(
           nameof(ScaleBackgroundLineWidth), typeof(float), typeof(SimpleRadialGuageView), 30.0f, propertyChanged: RedrawCanvas);

        public float ScaleBackgroundLineWidth
        {
            get => (float)GetValue(ScaleBackgroundLineWidthProperty);
            set => SetValue(ScaleBackgroundLineWidthProperty, value);
        }

        #endregion

        #region ScaleForegroundColor

        public static readonly BindableProperty ScaleForegroundColorProperty = BindableProperty.Create(
           nameof(ScaleForegroundColor), typeof(Color), typeof(SimpleRadialGuageView), Color.FromHex("#22b9e2"), propertyChanged: RedrawCanvas);

        public Color ScaleForegroundColor
        {
            get => (Color)GetValue(ScaleForegroundColorProperty);
            set => SetValue(ScaleForegroundColorProperty, value);
        }

        #endregion

        #region ScaleForegroundLineWidth

        public static readonly BindableProperty ScaleForegroundLineWidthProperty = BindableProperty.Create(
           nameof(ScaleForegroundLineWidth), typeof(float), typeof(SimpleRadialGuageView), 22.0f, propertyChanged: RedrawCanvas);

        public float ScaleForegroundLineWidth
        {
            get => (float)GetValue(ScaleForegroundLineWidthProperty);
            set => SetValue(ScaleForegroundLineWidthProperty, value);
        }

        #endregion

        #region Percentage

        public static readonly BindableProperty PercentProperty = BindableProperty.Create(
           nameof(Percent), typeof(double), typeof(SimpleRadialGuageView), 50.0d, propertyChanged: RedrawCanvas);

        public double Percent
        {
            get => (double)GetValue(PercentProperty);
            set => SetValue(PercentProperty, value);
        }

        #endregion

        #region StartAngle

        public static readonly BindableProperty StartAngleProperty = BindableProperty.Create(
           nameof(StartAngle), typeof(double), typeof(SimpleRadialGuageView), 120.0d, propertyChanged: RedrawCanvas);

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        #endregion

        #region EndAngle

        public static readonly BindableProperty EndAngleProperty = BindableProperty.Create(
           nameof(EndAngle), typeof(double), typeof(SimpleRadialGuageView), 300.0d, propertyChanged: RedrawCanvas);

        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        #endregion

        #region CenterContent

        public static readonly BindableProperty CenterContentProperty = BindableProperty.Create(
           nameof(CenterContent), typeof(View), typeof(SimpleRadialGuageView), null, propertyChanged: null);

        public View CenterContent
        {
            get => (View)GetValue(CenterContentProperty);
            set => SetValue(CenterContentProperty, value);
        }

        #endregion

        #endregion

        #region Constructors

        public SimpleRadialGuageView()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;

            _canvasView.PaintSurface += OnPaintSurface;

            var grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };
            grid.Children.Add(_canvasView);
            grid.Children.Add(_container);

            this.Content = grid;
        }

        #endregion

        #region Methods

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var currentPosX = this.Width / 2;
            var currentPosy = this.Height / 2;
            var newSize = this.Width - (this.Width / 3);

            _container.Content = CenterContent;
            _container.LayoutTo(new Rectangle(currentPosX - (newSize / 2), currentPosy - (newSize / 2), newSize, newSize), 0, Easing.Linear);
        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            SimpleRadialGuageView self = bindable as SimpleRadialGuageView;
            self?._canvasView.InvalidateSurface();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            //get the canvas & info
            var canvas = args.Surface.Canvas;
            int surfaceWidth = args.Info.Width;
            int surfaceHeight = args.Info.Height;

            //clear the canvas
            canvas.Clear();

            var centerx = surfaceWidth / 2;
            var centery = surfaceHeight / 2;

            var initialRadius = Math.Min(surfaceHeight, surfaceWidth) * 0.5f;



            var buffer = ScaleBackgroundLineWidth / 2;

            var backradius = initialRadius - buffer;
            var backleft = centerx - backradius;
            var backtop = centery - backradius;
            var backright = backleft + (backradius * 2);
            var backbottom = backtop + (backradius * 2);


            //back row first
            var rect = new SKRect(backleft, backtop, backright, backbottom);

            var angle = Math.PI * (StartAngle + 0) / 180.0;
            var endangle = Math.PI * (StartAngle + EndAngle) / 180.0;

            //calculate the radius and the center point of the circle
            var radius = (rect.Right - rect.Left) / 2;
            var middlePoint = new SKPoint();
            middlePoint.X = (rect.Left + radius);
            middlePoint.Y = rect.Top + radius; //top of current circle plus radius

            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)), middlePoint.Y + (float)(radius * Math.Sin(angle)), ScaleBackgroundLineWidth / 2, ArcPaintBackRound);
            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(endangle)), middlePoint.Y + (float)(radius * Math.Sin(endangle)), ScaleBackgroundLineWidth / 2, ArcPaintBackRound);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, (float)StartAngle, (float)EndAngle);
                canvas.DrawPath(path, ArcPaintBack);
            }

            var frontradius = initialRadius - buffer;
            var left = centerx - frontradius;
            var top = centery - frontradius;
            var right = left + (frontradius * 2);
            var bottom = top + (frontradius * 2);

            var rect2 = new SKRect(left, top, right, bottom);

            angle = Math.PI * (StartAngle) / 180.0;
            endangle = Math.PI * ((StartAngle + CurrentSweep)) / 180.0;


            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)), middlePoint.Y + (float)(radius * Math.Sin(angle)), ScaleForegroundLineWidth / 2, ArcPaintRound);
            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(endangle)), middlePoint.Y + (float)(radius * Math.Sin(endangle)), ScaleForegroundLineWidth / 2, ArcPaintRound);

            using (SKPath path = new SKPath())
            {

                path.AddArc(rect2, (float)StartAngle, (float)CurrentSweep);

                canvas.DrawPath(path, ArcPaint);
            }

        }

        #endregion
    }
}
