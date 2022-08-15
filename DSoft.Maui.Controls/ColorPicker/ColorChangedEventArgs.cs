using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls.ColorPicker
{
	public class ColorChangedEventArgs : EventArgs
	{

		public ColorChangedEventArgs(Color color)
		{
			this.Color = color;
		}

		public Color Color { get; private set; }
	}
}
