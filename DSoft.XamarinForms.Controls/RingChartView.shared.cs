using System;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DSoft.XamarinForms.Controls
{
	public class RingChartView : ContentView
	{
        #region fields and Properties

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

        }
    }
}
