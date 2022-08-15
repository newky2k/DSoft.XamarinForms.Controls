using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls.Extensions
{
	public static class SKPointExtensions
	{
		public static SKPoint ToPixelSKPoint(this Point pt, SKCanvasView canvasView)
		{
			return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
				(float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
		}

		public static bool IsInsideCircle(this SKPoint location, SKPoint center, float radius)
		{
			if (radius < 0) return false;

			var distance = Math.Sqrt(Math.Pow((location.X - center.X), 2f) +
										Math.Pow((location.Y - center.Y), 2f));
			return distance < radius;
		}

		public static SKColor[] ToSKColors(this IEnumerable<Color> colors)
		{
			var skColors = new List<SKColor>();

			if (colors == null || !colors.Any())
				throw new ArgumentException("Colors array must not be null or empty");

			foreach (var aCol in colors)
				skColors.Add(aCol.ToSKColor());

			return skColors.ToArray();
		}
	}
}
