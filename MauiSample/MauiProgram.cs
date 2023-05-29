using DSoft.Maui.Controls.TouchTracking;

namespace MauiSample
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.ConfigureEffects(effects =>
				{
					effects.Add<TouchEffect, TouchPlatformEffect>();

                });

			return builder.Build();
		}
	}
}