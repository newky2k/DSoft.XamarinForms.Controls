using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace DSoft.XamarinForms.Controls
{
    public class BubbleView : ContentView
    {
        #region Fields

        private Frame _background;
        private Label _label;

        #endregion

        #region BubbleColor

        public static readonly BindableProperty BubbleColorProperty = BindableProperty.Create(
           nameof(BubbleColor), typeof(Color), typeof(BubbleView), Color.Red, propertyChanged: RedrawCanvas);

        public Color BubbleColor
        {
            get => (Color)GetValue(BubbleColorProperty);
            set => SetValue(BubbleColorProperty, value);
        }

        #endregion

        #region HasShadow

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(
           nameof(HasShadow), typeof(bool), typeof(BubbleView), true, propertyChanged: RedrawCanvas);

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        #endregion


        #region BorderColor

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
           nameof(BorderColor), typeof(Color), typeof(BubbleView), Color.Transparent, propertyChanged: RedrawCanvas);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        #endregion

        #region TextColor

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
           nameof(TextColor), typeof(Color), typeof(BubbleView), Color.White, propertyChanged: RedrawCanvas);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        #endregion

        #region Text

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
           nameof(Text), typeof(string), typeof(BubbleView), "0", propertyChanged: RedrawCanvas);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        public BubbleView()
        {
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            Padding = 0;

            _background = new Frame()
            {
                BackgroundColor = Color.Red,
            };

            _label = new Label()
            {
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            _background.Content = _label;

            this.Content = _background;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (!(height < 0))
            {
                var widthThing = width / 2;
                _background.CornerRadius = (float)height / 2;
                _background.Padding = new Thickness(0, 0, 0, 0);

                if (_label.Width < 15)
                {
                    _background.WidthRequest = 20;
                }
                else
                {
                    _background.WidthRequest = _label.Width + 8;
                }

            }


        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            var self = bindable as BubbleView;


            self.UpdateContent();

        }

        private void UpdateContent()
        {
            _background.BackgroundColor = BubbleColor;
            _label.Text = Text;
            _label.TextColor = TextColor;

            _background.BorderColor = BorderColor;
            _background.HasShadow = HasShadow;

        }
    }
}
