using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RatingView.Extensions;
using RatingView.Sample.ViewModels;

namespace RatingView.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureRatingView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            return builder.Build();
        }
    }
}