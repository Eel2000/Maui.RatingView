# Maui.RatingView
RatingView is a Dotnet MAUI control that allow to integrate the rating control into a MAUI application.

## Desktop
https://github.com/Eel2000/Maui.RatingView/assets/44249870/0c8f2fd2-73b8-4bd8-b763-5e01abc36684

## Mobile(Android)
https://github.com/Eel2000/Maui.RatingView/assets/44249870/3701a884-8d5a-4e6e-bd15-94cbac31835f

## Getting Started with the Rating View Ccontrol

### Configuration
```C#
   var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureRatingView()//Add the control to the build pipeline
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
```


