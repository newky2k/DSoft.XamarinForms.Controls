# DSoft.XamarinForms.Controls
Controls library for Xamarin.Forms

 - Color pickers
   - `ColorWheelView` and `ColorPickerView`
   - Color picker views and popups for Xamarin Forms using SkiaSharp
 - GradientFrame
   - Draw a simple to color linear gradient
 - RepeaterView
   - Simple StackLayout derivative for showing lists
 - TouchEffect


## Changes/Modifications

### Color Pickers
Both color picker repos from zhenweied09 and simonscoffins where designed around a popuppage containing a SkiaView.  The following changes have been made to both based on their code

 - Moved the code into Frames (`ColorWheelView` and `ColorPickerView`)
   - This allows the pickers to be integrated into any view visual element
 - Added bindable property for Colors
   - This allows for a `Color` enumerable to be provide to the view for the color gradient rather than just using the default colors
   - This uses Xamarin.Forms colors externally
 - Added a white gradient in the center of the `ColorWheelView`
   - This allows selection of white from the picker and allows lighter versions of colors to be picked
   - Can be disabled by setting `ShowWhite` to false
 - Changed the size of the selector indicator and made it twice the size


## Attribution

Colors pickers based on work from  https://github.com/zhenweied09/ColorPickers (@zhenweied09)  and https://github.com/simonscoffins/color-picker (@simonscoffins)

Repeater view is based on work from (@webmasterdevlin) 
https://github.com/webmasterdevlin/RepeaterViewDemo/blob/master/RepeaterViewDemo/RepeaterViewDemo/RepeaterView.cs
