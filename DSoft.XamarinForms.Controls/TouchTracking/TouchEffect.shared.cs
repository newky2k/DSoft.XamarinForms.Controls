using Xamarin.Forms;

namespace DSoft.XamarinForms.Controls.TouchTracking
{

    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("DSoft.XamarinForms.Controls.TouchTracking.TouchEffect") { }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}