using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RatingView.Models;
using RatingView.Sample.Models;

namespace RatingView.Sample.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private double _ratingValue;

    [ObservableProperty] private Entity _data;

    [ObservableProperty] private double _liked;
    [ObservableProperty] private double _unLiked;

    public MainPageViewModel()
    {
        Data = new()
        {
            Name = "Eliezer"
        };
    }

    [RelayCommand]
    Task ShowRating()
    {

        Shell.Current.DisplayAlert("Rating", "Thank you for your feedback", "Ok");

        return Task.CompletedTask;
    }

    [RelayCommand]
    Task LikeDislike(Rating like)
    {
        if(Liked == 1 && UnLiked == 0)
        {
            Liked = 0;
            UnLiked = 1;
        }
        else
        {
            Liked = 1;
            UnLiked = 0;
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    Task Rating(Rating rating)
    {
        RatingValue = rating.Value;
        var param = rating.Parameter as Entity;
        Shell.Current.DisplayAlert("Rating", param.Name + " ,Your vote is " + rating.Value, "Ok");

        return Task.CompletedTask;
    }
}
