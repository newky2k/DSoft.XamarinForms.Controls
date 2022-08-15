using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls.ColorPicker.Extensions
{
	public static class ColorExtensions
	{
		internal static IEnumerable<ColorPick> ToColorPicks(this IEnumerable<Color> colors)
		{
			var result = new List<ColorPick>();

			if (colors == null || !colors.Any())
				throw new ArgumentException("Colors array must not be null or empty");

			foreach (var aCol in colors)
				result.Add(new ColorPick(aCol));

			return result;
		}
	}
}
