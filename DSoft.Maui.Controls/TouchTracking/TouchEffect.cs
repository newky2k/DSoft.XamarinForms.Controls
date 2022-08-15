using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls.TouchTracking
{
	public class TouchEffect : RoutingEffect
	{
		private const string effectId = "DSoft.Maui.Controls.TouchEffect";

		public event TouchActionEventHandler TouchAction;

		public TouchEffect() : base(effectId) { }

		public bool Capture { set; get; }

		public void OnTouchAction(Element element, TouchActionEventArgs args)
		{
			TouchAction?.Invoke(element, args);
		}
	}
}
