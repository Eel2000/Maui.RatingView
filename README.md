# Maui.RatingView
**Maui.RatingView** - is a cross platform plugin for [MAUI](https://dotnet.microsoft.com/en-us/apps/maui) which allow you to use the rating capabilities in your application with ease and flexibility.

**Important** : this plusing is still in pre-release mode, ensure that in visual studio package manage the prerealse checkbox is check to be able to see it.

## Desktop
https://github.com/Eel2000/Maui.RatingView/assets/44249870/c764d534-39e2-446f-8d4e-eaf60ace56e4


## Mobile(Android)
https://github.com/Eel2000/Maui.RatingView/assets/44249870/c40b60e8-dbe3-40ca-b0cd-9b0b55059c35



## Getting Started with the Rating View Ccontrol

### Configuration

* Add the package reference to your project's .csproj file
*  Look for the RatingViewControl into your project's package manager 
```.NET CLI
   dotnet add package RatingViewControl --version 1.1.2-alpha
```

### Using the control

* First Add the reference to the control into your xaml file like this
```XAML
xmlns:rv="clr-namespace:RatingView.Views;assembly=RatingView"
```
```XAML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:rv="clr-namespace:RatingView.Views;assembly=RatingView"
             x:Class="MauiApp2.MainPage">

</ContentPage>
```
* Then use the control in your xaml page like shown below.
```XAML
 <rv:RatingControl Maximum="6" Size="40" Fill="Red" Value="3" />
```

### Attributes and use
|Attribute | Description|
| --- | ---|
| Shape | the shape to display. the is 4 supported types (Heart,Like,Displike,Star) |
| Maximum | the maximun rating value. the default maximum value is `5` |
| Size | the size of the shape. the default shape's size is `20px` |
| Fill | The color used when the shape is filled. the default color is `Orange` |
| Value | support double values, the current rate value |
| EmptyColor | The color of filling when the shape is empty. the default color is `White` |
| StrokeColor | The color of the shape's stroke. the default value is `Gray` |
| StrokeThickness | The thickness of the shape's stroke. the default value is `7px` |
| Spacing | defines the space between drawn shapes. By default the space defined is `10px` |
| AllowRating | A boolean value which define if the click event can be handled to change(update) the rating value |
| Command | Used when the `AllowRating` is defined to `True` this command attribute can be used to perfom a specifique action on rating value changed |
| CommandParameter | of type of object which is the parameter passed to the command when executed. Note that this parameter when handle in your code will be of type of `Rating` object which is the model containt int the library it has 2 Properties **`Value` : which is the rating value** **`Paramter` : the command parameter passed from the xaml page** |
| BindControl | This property allows to bind a specique rating view to another one, usefull in some scenario. Eg : Youtube Liking behavior |
| Animate | This property define that the Shape scale should change when Touched or Overlayed **(For Desktop App only at this time )** |

## Examples
### Liking fingers

```XAML
 <VerticalStackLayout Spacing="5">
    <Label Text="Liking" HorizontalOptions="Center"/>
    <HorizontalStackLayout Spacing="10" HorizontalOptions="CenterAndExpand">
        <rv:RatingControl x:Name="likedBtn" Maximum="1" Value="{Binding Liked}" Size="40" AllowRating="True"
               Fill="ForestGreen" Command="{Binding LikeDislikeCommand}" BindControl="{x:Reference unLikedBtn}"
                      StrokeThickness="5" Shape="Like"/>

        <rv:RatingControl x:Name="unLikedBtn" Maximum="1" Value="{Binding UnLiked}" Size="40" AllowRating="True"
                          Fill="DarkRed" Command="{Binding LikeDislikeCommand}" 
                          BindControl="{x:Reference likedBtn}"
                          StrokeThickness="5" Shape="Dislike"/>
    </HorizontalStackLayout>
</VerticalStackLayout>
```
Output

https://github.com/Eel2000/Maui.RatingView/assets/44249870/94e49c5a-15ae-4eae-91d7-d2f310ec8e14

### Linking Heart

```XAML
<rv:RatingControl Maximum="1" 
                  Size="40" 
                  Fill="Red" 
                  Shape="Heart" 
                  AllowRating="True" />
```

OutPut 

https://github.com/Eel2000/Maui.RatingView/assets/44249870/ce31a3ac-1621-44df-a224-5ed02c2b7880

## Created By: Eliezer Bwana

-   LinkedIn:  [Eliezer Bwana](https://www.linkedin.com/in/eliezer-bwana-a52747190)
-   Twitter:  [@BwanaEliezer](https://twitter.com/BwanaEliezer)

## [](https://github.com/rotorgames/Rg.Plugins.Popup#license)License

The MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:






