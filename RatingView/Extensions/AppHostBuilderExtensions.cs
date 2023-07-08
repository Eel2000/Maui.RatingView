namespace RatingView.Extensions
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder ConfigureRatingView(this MauiAppBuilder app)
        {

            app.ConfigureFonts(fonts =>
            {
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FontRegular");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontSolid");
            });

            return app;
        }
    }
}
