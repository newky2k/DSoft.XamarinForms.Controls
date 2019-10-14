using Xamarin.Forms;

namespace DSoft.XamarinForms.Controls.TouchTracking
{

    public class TouchEffect : RoutingEffect
    {
        private const string effectId = "DSoft.XamarinForms.Controls.TouchEffect";

        /// <summary>
        /// Resolves the effect with its platform implementation
        /// </summary>
        /// <returns></returns>
        public static Effect Build()
        {
            return Effect.Resolve(effectId);
        }

        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base(effectId) { }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}