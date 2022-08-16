using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls
{
	public class SimpleDonutGuageView : SimpleRadialGuageView
	{
		public SimpleDonutGuageView() : base()
		{
			StartAngle = 0;
			EndAngle = 360;
			Rotation = -90;
		}
	}
}
