using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DSoft.XamarinForms.Controls
{
	public class RingChartView : ContentView
	{
        #region fields and Properties

        private double StartAngle = 270;
        private double EndAngle = 360;

        private readonly SKCanvasView _canvasView = new SKCanvasView();
        private readonly Frame _container = new Frame()
        {
            CornerRadius = 20,
            HasShadow = false,
            BackgroundColor = Color.Transparent,

        };

        //public double CurrentSweep => EndAngle * (Percent / 100);

        #endregion

        public RingChartView()
		{
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Fill;

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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var currentPosX = this.Width / 2;
            var currentPosy = this.Height / 2;
            var newSize = this.Width - (this.Width / 3);

            //_container.Content = CenterContent;
            _container.LayoutTo(new Rectangle(currentPosX - (newSize / 2), currentPosy - (newSize / 2), newSize, newSize), 0, Easing.Linear);
        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            RingChartView self = bindable as RingChartView;
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

            var initialRadius = Math.Min(surfaceHeight, surfaceWidth);

            //var percentages = new double[] { 62.7, 29.5, 85.2, 45.6};

            var percentages = new double[] { 45.6, 85.2, 29.5, 62.7 };

            var itemCount = percentages.Length * 2;

            var maxlineWidth = 15;//initialRadius / ((itemCount * 2));

            var buffer = maxlineWidth;

            var backradius = initialRadius - buffer;
            var backleft = centerx - (backradius * 0.5f);
            var backtop = centery - (backradius * 0.5f);
            var backright = backleft + backradius;
            var backbottom = backtop + backradius;

            //Main rect
            var rect = new SKRect(backleft, backtop, backright, backbottom);

            var mainBackColor = Color.FromHex("#ebeced");

            var colors = new List<Color>()
            {
                //Color.FromHex("#22b9e2"),
                Color.FromHex("e56590"),
                Color.FromHex("9686c9"),
                Color.FromHex("e58870"),
                Color.FromHex("47ba9f"),
                
                
                
            };

            for (var loop = 0; loop < percentages.Length; loop++)
			{
                var foreColor = colors[loop];// Color.FromHex("#22b9e2");
                var backColor = mainBackColor;// Color.FromRgba(foreColor.R, foreColor.G, foreColor.B, 0.4f);

                DrawRing(canvas, rect, backColor.ToSKColor(), foreColor.ToSKColor(), maxlineWidth, percentages[loop]);

                rect.Inflate(new SKSize(-(maxlineWidth * 2), -(maxlineWidth * 2)));
            }
            
            

            //DrawRing(canvas, rect, Color.FromHex("#343c44").ToSKColor(), Color.FromHex("#22b9e2").ToSKColor(), maxlineWidth, 67);
        }

        private void DrawRing(SKCanvas canvas, SKRect rect, SKColor backcolor, SKColor foreColor, float lineWidth, double percent)
		{
            var CurrentSweep = EndAngle * (percent / 100);

            var ArcPaintBack = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                Color = backcolor,
                IsAntialias = true,
            };

            var ArcPaintBackRound = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = backcolor,
                IsAntialias = true,
            };

            var ArcPaintRound = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = foreColor,
                IsAntialias = true,
            };

            var ArcPaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                Color = foreColor,
                IsAntialias = true,
            };

            var angle = Math.PI * (StartAngle + 0) / 180.0;
            var endangle = Math.PI * (StartAngle + EndAngle) / 180.0;

            //calculate the radius and the center point of the circle
            var radius = (rect.Right - rect.Left) / 2;
            var middlePoint = new SKPoint();
            middlePoint.X = (rect.Left + radius);
            middlePoint.Y = rect.Top + radius; //top of current circle plus radius

            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)), middlePoint.Y + (float)(radius * Math.Sin(angle)), lineWidth / 2, ArcPaintBackRound);
            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(endangle)), middlePoint.Y + (float)(radius * Math.Sin(endangle)), lineWidth / 2, ArcPaintBackRound);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, (float)StartAngle, (float)EndAngle);
                canvas.DrawPath(path, ArcPaintBack);
            }

            angle = Math.PI * (StartAngle) / 180.0;
            endangle = Math.PI * ((StartAngle + CurrentSweep)) / 180.0;


            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)), middlePoint.Y + (float)(radius * Math.Sin(angle)), lineWidth / 2, ArcPaintRound);
            canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(endangle)), middlePoint.Y + (float)(radius * Math.Sin(endangle)), lineWidth / 2, ArcPaintRound);

            using (SKPath path = new SKPath())
            {

                path.AddArc(rect, (float)StartAngle, (float)CurrentSweep);

                canvas.DrawPath(path, ArcPaint);
            }
        }
    }
}
