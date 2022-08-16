using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls
{
	public class GradiantFrame : Frame
	{

		#region Private Members

		private readonly SKCanvasView _canvasView = new SKCanvasView();

		#endregion

		#region Bindable Properties

		#region FromColor

		public static readonly BindableProperty FromColorProperty = BindableProperty.Create(
		   nameof(FromColor), typeof(Color), typeof(GradiantFrame), new Color(-1.0f, -1.0f, -1.0f, -1.0f), propertyChanged: RedrawCanvas);

		public Color FromColor
		{
			get => (Color)GetValue(FromColorProperty);
			set => SetValue(FromColorProperty, value);
		}

		#endregion

		#region ToColorHex

		public static readonly BindableProperty ToColorProperty = BindableProperty.Create(
			nameof(ToColor), typeof(Color), typeof(GradiantFrame), new Color(-1.0f, -1.0f, -1.0f, -1.0f), propertyChanged: RedrawCanvas);

		public Color ToColor
		{
			get => (Color)GetValue(ToColorProperty);
			set => SetValue(ToColorProperty, value);
		}

		#endregion

		#endregion

		#region Constructors
		public GradiantFrame()
		{
			Padding = new Thickness(0);
			BackgroundColor = Colors.Transparent;
			HasShadow = false;
			_canvasView.PaintSurface += OnCanvasViewPaintSurface;
			Content = _canvasView;
		}
		#endregion

		#region Methods

		private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
		{
			GradiantFrame svgIcon = bindable as GradiantFrame;
			svgIcon?._canvasView.InvalidateSurface();
		}

		void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			using (SKPaint paint = new SKPaint())
			{
				// Create 300-pixel square centered rectangle
				SKRect rect = info.Rect;

				// Create linear gradient from upper-left to lower-right
				paint.Shader = SKShader.CreateLinearGradient(
									new SKPoint(rect.Right, rect.Top),
									new SKPoint(rect.Left, rect.Bottom),
									new SKColor[] { SKColor.Parse(FromColor.ToHex()), SKColor.Parse(ToColor.ToHex()) },
									new float[] { 0, 1 },
									SKShaderTileMode.Repeat);

				// Draw the gradient on the rectangle
				canvas.DrawRect(rect, paint);

			}
		}

		#endregion
	}
}
