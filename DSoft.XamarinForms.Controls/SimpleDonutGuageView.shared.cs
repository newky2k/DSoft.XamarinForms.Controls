using System;
namespace DSoft.XamarinForms.Controls
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
