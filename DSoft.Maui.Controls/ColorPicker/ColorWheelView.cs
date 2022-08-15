using DSoft.Maui.Controls.Extensions;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSoft.Maui.Controls.TouchTracking;
using MColor = Microsoft.Maui.Graphics.Colors;

namespace DSoft.Maui.Controls.ColorPicker
{
	public class ColorWheelView : Frame
	{
		#region Defaults

		private static IEnumerable<Color> DefaultColors
		{
			get
			{
				var results = new List<Color>();

				for (int i = 0; i < 8; i++)
				{
					results.Add(SKColor.FromHsl(i * 360f / 7, 100, 50).ToMauiColor());
				}

				results.Reverse();

				return results;
			}
		}
		#endregion


		#region Bindable Properties

		#region ColorsProperty

		public static readonly BindableProperty ColorsProperty = BindableProperty.Create(
			nameof(Colors), typeof(IEnumerable<Color>), typeof(ColorWheelView), DefaultColors, propertyChanged: RedrawCanvas);

		/// <summary>
		/// The Colors to be used by the color gradient
		/// </summary>
		public IEnumerable<Color> Colors
		{
			get => (IEnumerable<Color>)GetValue(ColorsProperty);
			set => SetValue(ColorsProperty, value);
		}


		#endregion

		#region ShowWhite

		public static readonly BindableProperty ShowWhiteProperty = BindableProperty.Create(
			nameof(Colors), typeof(bool), typeof(ColorWheelView), true, propertyChanged: RedrawCanvas);

		/// <summary>
		/// Show the white color gradient overlay at the center of the Gradient
		/// </summary>
		public bool ShowWhite
		{
			get => (bool)GetValue(ShowWhiteProperty);
			set => SetValue(ShowWhiteProperty, value);
		}


		#endregion
		#endregion

		#region Fields

		private readonly SKCanvasView _canvasView = new SKCanvasView();

		private const int _shrinkage = 50;

		private float _radius;
		private bool _colorChanged;
		private float _touchCircleRadius = 30;

		#region Paint Objects
		private readonly SKPaint _touchCircleOutline = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			Color = MColor.Gray.ToSKColor(),
			StrokeWidth = 4,
			IsAntialias = true
		};

		private readonly SKPaint _touchCircleFill = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};

		private readonly SKPaint _circlePalette = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};


		private readonly SKPaint _centerGradient = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};

		private readonly SKPaint _rectanglePalette = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};
		#endregion

		#region Points
		private SKPoint _touchLocation;
		private SKPoint _center;
		#endregion

		#region Colors

		//private List<Color> _colors = DefaultColors;

		private SKColor _selectedColor = MColor.Transparent.ToSKColor();
		#endregion

		#region Events
		public event EventHandler<ColorChangedEventArgs> ColorChanged;
		#endregion

		#endregion
		public ColorWheelView()
		{

			Padding = new Thickness(0);
			BackgroundColor = MColor.Transparent;
			HasShadow = false;
			Content = _canvasView;
			_canvasView.PaintSurface += OnPaintSurface;

			var touchEffect = new TouchEffect()
			{
				Capture = true,

			};

			touchEffect.TouchAction += OnTouchEffectAction;

			this.Effects.Add(touchEffect);
		}

		private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorWheelView colorWheel = bindable as ColorWheelView;
			colorWheel?._canvasView.InvalidateSurface();
		}

		void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{

			var colorRange = Colors.ToSKColors();

			var info = e.Info;
			var surface = e.Surface;
			var canvas = surface.Canvas;
			canvas.Clear();

			_center = new SKPoint(info.Rect.MidX, info.Rect.MidY);
			_radius = (Math.Min(info.Width, info.Height) - _shrinkage) / 2;

			_circlePalette.Shader = SKShader.CreateSweepGradient(_center, colorRange, null);
			canvas.DrawCircle(_center, _radius, _circlePalette);

			if (ShowWhite)
			{
				var innerradius = _radius * 0.75f;
				_centerGradient.Shader = SKShader.CreateRadialGradient(
					_center,
					innerradius,
					new SKColor[] { SKColors.White,
												SKColors.Transparent },
					new float[] { 0.05f, 1 },
					SKShaderTileMode.Clamp);

				canvas.DrawCircle(_center, innerradius, _centerGradient);
			}


			var rectLeft = info.Rect.MidX - _radius;
			var rectTop = 0;
			var rectRight = rectLeft + _radius * 2;
			var rectBottom = rectTop + _shrinkage;
			var rect = new SKRect(rectLeft, rectTop, rectRight, rectBottom);

			//insure touch circle in the center of color wheel
			if (_touchLocation == SKPoint.Empty)
			{
				_touchLocation = _center;
				_colorChanged = true;
			}

			if (_colorChanged)
			{
				using (var bmp = new SKBitmap(info))
				{
					IntPtr dstpixels = bmp.GetPixels();

					var succeed = surface.ReadPixels(info, dstpixels, info.RowBytes, (int)_touchLocation.X, (int)_touchLocation.Y);
					if (succeed)
					{
						_selectedColor = bmp.GetPixel(0, 0);
						_touchCircleFill.Color = _selectedColor;

						ColorChanged?.Invoke(this, new ColorChangedEventArgs(_selectedColor.ToMauiColor()));
					}
				}
			}

			canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleOutline);
			canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleFill);
		}

		private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
		{
			var skPoint = args.Location.ToPixelSKPoint(_canvasView);
			if (skPoint.IsInsideCircle(_center, _radius))
			{
				_touchLocation = skPoint;

				if (args.Type == TouchActionType.Entered ||
					args.Type == TouchActionType.Pressed ||
					args.Type == TouchActionType.Moved ||
					args.Type == TouchActionType.Released)
				{
					_colorChanged = true;
					_canvasView.InvalidateSurface();
				}
			}
			else
			{
				_colorChanged = false;
			}
		}

	}
}
