using System;
using System.Collections;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DSoft.XamarinForms.Controls
{
	public class RingChartView : ContentView
	{
        #region fields and Properties
        private List<DataEntryInternal> _internalData = new List<DataEntryInternal>();

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

        #region Bindable Properties

        #region ScaleBackgroundColor

        public static readonly BindableProperty RingBackgroundColorProperty = BindableProperty.Create(
           nameof(RingBackgroundColor), typeof(Color), typeof(RingChartView), Color.FromHex("#ebeced"), propertyChanged: RedrawCanvas);

        public Color RingBackgroundColor
        {
            get => (Color)GetValue(RingBackgroundColorProperty);
            set => SetValue(RingBackgroundColorProperty, value);
        }

        public static readonly BindableProperty UseShadedRingColorProperty = BindableProperty.Create(
           nameof(UseShadedRingColor), typeof(bool), typeof(RingChartView), false, propertyChanged: RedrawCanvas);

        public bool UseShadedRingColor
        {
            get => (bool)GetValue(UseShadedRingColorProperty);
            set => SetValue(UseShadedRingColorProperty, value);
        }

        #endregion

        #region CenterView

        public static readonly BindableProperty CenterViewProperty = BindableProperty.Create(
           nameof(CenterView), typeof(View), typeof(RingChartView), null, propertyChanged: null);

        public View CenterView
        {
            get => (View)GetValue(CenterViewProperty);
            set => SetValue(CenterViewProperty, value);
        }

		#endregion

		#region ItemsSource


		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IList),
            typeof(RingChartView),
            null,
            BindingMode.OneWay,
            propertyChanged: ItemsChanged);

        public IList ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as IList;
            set => SetValue(ItemsSourceProperty, value);
        }

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {


            if (!(bindable is RingChartView))
                return;

            RingChartView self = bindable as RingChartView;
            

            self?.UpdateInternalData((IList)newValue);


            self?._canvasView.InvalidateSurface();
        }

        #endregion


        #region Color Palette

        public static readonly BindableProperty ColorPaletteProperty = BindableProperty.Create(
                            nameof(ColorPalette), typeof(IList<Color>), typeof(RingChartView), InternalColorModel.DefaultColors, propertyChanged: null);

        public IList<Color> ColorPalette
        {
            get => (IList<Color>)GetValue(ColorPaletteProperty);
            set => SetValue(ColorPaletteProperty, value);
        }

        #endregion
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

            _container.Content = CenterView;
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

            var itemCount = _internalData.Count * 2;

            var maxlineWidth = 10;

            var buffer = maxlineWidth;

            var backradius = initialRadius - buffer;
            var backleft = centerx - (backradius * 0.5f);
            var backtop = centery - (backradius * 0.5f);
            var backright = backleft + backradius;
            var backbottom = backtop + backradius;

            //Main rect
            var rect = new SKRect(backleft, backtop, backright, backbottom);


            for (var loop = 0; loop < _internalData.Count; loop++)
			{
                var currenEntry = _internalData[loop];

                var foreColor = (currenEntry.Color.HasValue) ? currenEntry.Color.Value : GetColor(loop);

                var backColor = (UseShadedRingColor == true) ? Color.FromRgba(foreColor.R, foreColor.G, foreColor.B, 0.4f) : RingBackgroundColor;

                DrawRing(canvas, rect, backColor.ToSKColor(), foreColor.ToSKColor(), maxlineWidth, currenEntry.Percent);

                rect.Inflate(new SKSize(-(maxlineWidth * 2), -(maxlineWidth * 2)));
            }
            
           
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

        private void UpdateInternalData(IList data)
		{
            _internalData = new List<DataEntryInternal>();

            if (data == null || data.Count == 0)
                return;

            //work in reverse
            for (var loop = (data.Count - 1); loop > -1; loop--)
			{
                var item = data[loop];

                var percentProp = item.GetType().GetProperty("Percent");
                var valueProp = item.GetType().GetProperty("Value");
                var labelProp = item.GetType().GetProperty("Label");
                var colorProp = item.GetType().GetProperty("Color");

                double realPerValue = 0;
                double realValue = 0;
                string realLabel = string.Empty;
                Color? realColor = null;

                if (percentProp != null && percentProp.PropertyType.Equals(typeof(double)))
                {
                    var percValue = percentProp.GetValue(item);

                    realPerValue = (double)percValue;
                }

                if (valueProp != null && valueProp.PropertyType.Equals(typeof(double)))
				{
                    var valValue = valueProp.GetValue(item);

                    realValue = (double)valValue;
                }

                if (labelProp != null && labelProp.PropertyType.Equals(typeof(string)))
                {
                    var labValue = labelProp.GetValue(item);

                    realLabel = labValue as string;
                }

                if (colorProp != null && colorProp.PropertyType.Equals(typeof(Color?)))
                {
                    var labValue = colorProp.GetValue(item);

                    if (labValue != null)
                        realColor = (Color)labValue;
                }
                else if (colorProp != null && colorProp.PropertyType.Equals(typeof(Color)))
                {
                    var labValue = colorProp.GetValue(item);

                    realColor = (Color)labValue;
                }


                _internalData.Add(new DataEntryInternal()
                {
                    Percent = realPerValue,
                    Value = realValue,
                    Label = realLabel,
                    Color = realColor,
                });
            }

        }

        private Color GetColor(int index)
		{
            if (index > (ColorPalette.Count - 1))
                return Color.IndianRed;

            return ColorPalette[index];
        }
		#region Internal classes
		private class DataEntryInternal
		{
            public double Percent { get; set; }

            public double Value { get; set; }

            public string Label { get; set; }

            public Color? Color { get; set; }
        }

        private class InternalColorModel
		{
            public static List<Color> DefaultColors
			{
                get
                {
                    var colors = new List<Color>()
                                {
                                    Color.FromHex("e56590"),
                                    Color.FromHex("9686c9"),
                                    Color.FromHex("e58870"),
                                    Color.FromHex("47ba9f"),
                                     Color.FromHex("2e2157"),
                                      Color.FromHex("760f56"),

                                    Color.FromHex("8d88cb"),
                                    Color.FromHex("fdcc41"),
                                    Color.FromHex("c33716"),
                                    Color.FromHex("ff010b"),

                                    Color.FromHex("7c3f09"),
                                    Color.FromHex("739f46"),
                                    Color.FromHex("6c4874"),
                                    Color.FromHex("690102"),
                                };

                    return colors;
                }
			}
		}

		#endregion

	}
}
