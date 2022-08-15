using DSoft.Maui.Controls.ColorPicker.Extensions;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MColor = Microsoft.Maui.Graphics.Colors;
using DSoft.Maui.Controls.TouchTracking;
using DSoft.Maui.Controls.Extensions;

namespace DSoft.Maui.Controls.ColorPicker
{
	public class ColorPickerView : Frame
	{
		#region Defaults

		private static IEnumerable<Color> DefaultColors
		{
			get
			{
				var results = new List<Color> {

					Color.FromArgb("#25c5db"),
					Color.FromArgb("#0098a6"),
					Color.FromArgb("#0e47a1"),
					Color.FromArgb("#1665c1"),
					Color.FromArgb("#039be6"),
					Color.FromArgb("#64b5f6"),
					Color.FromArgb("#ff7000"),
					Color.FromArgb("#ff9f00"),
					Color.FromArgb("#ffb200"),
					Color.FromArgb("#cf9702"),
					Color.FromArgb("#8c6e63"),
					Color.FromArgb("#6e4c42"),
					Color.FromArgb("#d52f31"),
					Color.FromArgb("#ff1643"),
					Color.FromArgb("#f44236"),
					Color.FromArgb("#ec407a"),
					Color.FromArgb("#ad1457"),
					Color.FromArgb("#6a1b9a"),
					Color.FromArgb("#ab48bf"),
					Color.FromArgb("#b968c7"),
					Color.FromArgb("#00695b"),
					Color.FromArgb("#00887a"),
					Color.FromArgb("#4cb6ac"),
					Color.FromArgb("#307c32"),
					Color.FromArgb("#43a047"),
					Color.FromArgb("#64dd16"),
					Color.FromArgb("#222222"),
					Color.FromArgb("#5f7c8c"),
					Color.FromArgb("#b1bec6"),
					Color.FromArgb("#465a65"),
					Color.FromArgb("#607d8d"),
					Color.FromArgb("#91a5ae"),
				};

				return results;

			}
		}

		#endregion

		#region Fields
		private readonly SKCanvasView _canvasView = new SKCanvasView();

		IEnumerable<ColorPick> ColorPicks;

		bool _colorPicksInitialized;
		ColorPick _pickedColor;

		const int CanvasPadding = 5;
		bool _colorChanged;

		readonly SKPaint _clrPickPaint = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};

		readonly SKPaint _clrPickPaintEdge = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			Color = MColor.Gray.ToSKColor(),
			StrokeWidth = 4,
			IsAntialias = true
		};

		readonly SKPaint _pickedClrPaint = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			StrokeWidth = 5,
			IsAntialias = true,
		};

		#endregion

		#region Properties

		#region Bindable Properties

		#region ColorsProperty

		public static readonly BindableProperty ColorsProperty = BindableProperty.Create(
			nameof(Colors), typeof(IEnumerable<Color>), typeof(ColorWheelView), DefaultColors, propertyChanged: RedrawCanvasNewColors);

		/// <summary>
		/// The Colors to be used by the color gradient
		/// </summary>
		public IEnumerable<Color> Colors
		{
			get => (IEnumerable<Color>)GetValue(ColorsProperty);
			set => SetValue(ColorsProperty, value);
		}


		#endregion

		#region ColorsPerRow

		public static readonly BindableProperty ColorsPerRowProperty = BindableProperty.Create(
			nameof(Colors), typeof(int), typeof(ColorWheelView), 5, propertyChanged: RedrawCanvas);

		/// <summary>
		/// Show the white color gradient overlay at the center of the Gradient
		/// </summary>
		public int ColorsPerRow
		{
			get => (int)GetValue(ColorsPerRowProperty);
			set => SetValue(ColorsPerRowProperty, value);
		}


		#endregion

		#endregion

		#endregion

		#region Events

		public event EventHandler<ColorChangedEventArgs> ColorChanged;

		#endregion
		#region Constructors


		public ColorPickerView()
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

			Effects.Add(touchEffect);
		}

		#endregion

		#region Methods

		private static void RedrawCanvasNewColors(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorPickerView colorPicker = bindable as ColorPickerView;

			colorPicker._colorPicksInitialized = false;
			colorPicker?._canvasView.InvalidateSurface();
		}

		private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorPickerView colorPicker = bindable as ColorPickerView;
			colorPicker?._canvasView.InvalidateSurface();
		}

		void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{

			var info = e.Info;
			var surface = e.Surface;
			var canvas = surface.Canvas;

			canvas.Clear();

			if (!_colorPicksInitialized)
			{
				InitializeColorPicks(info.Width);
			}

			// draw the colors
			foreach (var cp in ColorPicks)
			{
				_clrPickPaint.Color = cp.Color.ToSKColor();
				canvas.DrawCircle(cp.Position.X, cp.Position.Y, cp.Radius, _clrPickPaint);

				if (cp.IsWhite)
				{
					canvas.DrawCircle(cp.Position.X, cp.Position.Y, cp.Radius, _clrPickPaintEdge);
				}

			}

			// check if there is a selected color
			if (_pickedColor == null) { return; }

			// draw the highlight for the picked color
			if (_pickedColor.IsWhite)
			{
				_pickedClrPaint.Color = MColor.Gray.ToSKColor();
			}
			else
			{
				_pickedClrPaint.Color = _pickedColor.Color.ToSKColor();
			}

			canvas.DrawCircle(_pickedColor.Position.X, _pickedColor.Position.Y, _pickedColor.Radius + 10, _pickedClrPaint);
		}

		void InitializeColorPicks(int skImageWidth)
		{

			var contentWidth = skImageWidth - (CanvasPadding * 2);
			var colorWidth = contentWidth / ColorsPerRow;

			var sp = new SKPoint((colorWidth / 2) + CanvasPadding, (colorWidth / 2) + CanvasPadding);
			var col = 1;
			var row = 1;
			var radius = (colorWidth / 2) - 10;

			ColorPicks = Colors.ToColorPicks();

			foreach (var cp in ColorPicks)
			{

				if (col > ColorsPerRow)
				{
					col = 1;
					row += 1;
				}

				// calc current position
				var x = sp.X + (colorWidth * (col - 1));
				var y = sp.Y + (colorWidth * (row - 1));

				cp.Radius = radius;
				cp.Position = new SKPoint(x, y);
				col += 1;
			}

			_colorPicksInitialized = true;
		}

		private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
		{

			if (args.Type == TouchActionType.Released)
			{
				var pnt = args.Location.ToPixelSKPoint(_canvasView);

				// loop through all colors
				foreach (var cp in ColorPicks)
				{
					// check if selecting a color
					if (cp.IsTouched(pnt))
					{
						_colorChanged = true;
						_pickedColor = cp;
						break; // get out of loop
					}
				}

				_canvasView.InvalidateSurface();

				if (_colorChanged)
				{
					ColorChanged?.Invoke(this, new ColorChangedEventArgs(_pickedColor.Color));
					_colorChanged = false;
				}
			}
		}

		#endregion


	}
}
