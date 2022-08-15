using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls.TouchTracking
{
	internal class DragInfo
	{

		public DragInfo(long id, Point pressPoint)
		{
			Id = id;
			PressPoint = pressPoint;
		}

		public long Id { private set; get; }
		public Point PressPoint { private set; get; }
	}
}
