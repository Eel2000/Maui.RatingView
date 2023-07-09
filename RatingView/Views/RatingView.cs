using RatingView.Models;
using RatingView.Shared;
using System.Windows.Input;

namespace RatingView.Views;

// All the code in this file is included in all platforms.
public class RatingView : BaseTemplateView<Grid>
{
    #region Bindable Properties
    //Rating value bindable property
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(double),
            typeof(RatingView),
            defaultValue: 0.0,
            propertyChanged: OnValueChanged);


    //Maximum rating value Bindable property
    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(
            nameof(Maximum),
            typeof(int),
            typeof(RatingView),
            defaultValue: 5,
            propertyChanged: OnMaximumChanged);


    //Star size Bindable property
    public static readonly BindableProperty StarSizeProperty =
        BindableProperty.Create(
            nameof(StarSize),
            typeof(double),
            typeof(RatingView),
            defaultValue: 20.0,
            propertyChanged: OnStarSizeChanged);


    //Star Color Bindable property
    public static readonly BindableProperty StarColorProperty =
        BindableProperty.Create(
            nameof(StarColor),
            typeof(Color),
            typeof(RatingView),
            defaultValue: Colors.Yellow,
            propertyChanged: OnStarColorChanged);


    //Star Spacing Between Bindable Property
    public static readonly BindableProperty StarSpacingProperty =
        BindableProperty.Create(
            nameof(StarSpacing),
            typeof(double),
            typeof(RatingView),
            defaultValue: 10.0,
            propertyChanged: OnStartSpacingChanged);

    public static readonly BindableProperty AllowClickRatingProperty =
        BindableProperty.Create(
            nameof(AllowClickRating),
            typeof(bool),
            typeof(RatingView),
            defaultValue: false,
            propertyChanged: OnAllowClickRatingChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(RatingView),
            defaultValue: null,
            propertyChanged: OnCommandChanged);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(RatingView),
            defaultValue: null,
            propertyChanged: OnCommandParameterChanged);

    public static readonly BindableProperty AnimateProperty =
        BindableProperty.Create(
            nameof(Animate),
            typeof(bool),
            typeof(RatingView),
            defaultValue: true,
            propertyChanged: OnAnimateChanged);

    #endregion

    #region Private Properties
    private Image[] stars;
    #endregion

    #region Public Properties
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public int Maximum
    {
        get => (int)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double StarSize
    {
        get => (double)GetValue(StarSizeProperty);
        set => SetValue(StarSizeProperty, value);
    }

    public Color StarColor
    {
        get => (Color)GetValue(StarColorProperty);
        set => SetValue(StarColorProperty, value);
    }

    public double StarSpacing
    {
        get => (double)GetValue(StarSpacingProperty);
        set => SetValue(StarSpacingProperty, value);
    }

    public bool AllowClickRating
    {
        get => (bool)GetValue(AllowClickRatingProperty);
        set => SetValue(AllowClickRatingProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public bool Animate
    {
        get => (bool)GetValue(AnimateProperty);
        set => SetValue(AnimateProperty, value);
    }

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion


    public RatingView()
    {
        HorizontalOptions = LayoutOptions.CenterAndExpand;

        this.Control.ColumnSpacing = StarSpacing;

        stars = new Image[Maximum];

        GenerateStars();
    }

    #region Methods
    private void UpdateStars()
    {
        for (int i = 0; i < Maximum; i++)
        {
            var image = stars[i];

            var fontImageSource = image.Source as FontImageSource;

            if (Value >= i + 1)
            {
                fontImageSource.Size = StarSize;
            }
            else
            {
                if (Value % 1 == 0)
                {
                    fontImageSource.Color = Colors.DarkGray;
                    fontImageSource.Glyph = "\uf005";
                    fontImageSource.FontFamily = "FontSolid";
                }
                else
                {
                    var fraction = Value - Math.Floor(Value);
                    var element = stars[(int)(Value - fraction)];
                    if (element != null)
                    {
                        var imageSource = element.Source as FontImageSource;
                        imageSource.Glyph = "\uf089";
                        imageSource.FontFamily = "FontSolid";
                        imageSource.Color = StarColor;
                    }
                }
            }
        }
    }

    private void GenerateStars()
    {
        for (int i = 0; i < Maximum; i++)
        {

            Control.ColumnDefinitions.Add(new ColumnDefinition { Width = StarSize });
            Image image = new();
            if (i <= Value)
            {

                image.Source = new FontImageSource()
                {
                    Glyph = "\uf005",
                    FontFamily = "FontSolid",
                    Color = StarColor,
                    Size = StarSize,
                };
                image.WidthRequest = StarSize;
                image.HeightRequest = StarSize;
                image.Aspect = Aspect.AspectFit;

            }
            else if (i > Value)
            {
                image.Source = new FontImageSource()
                {
                    Glyph = "\uf005",
                    FontFamily = "FontSolid",
                    Color = Colors.DarkGray,
                    Size = StarSize,
                };
                image.WidthRequest = StarSize;
                image.HeightRequest = StarSize;
                image.Aspect = Aspect.AspectFit;
            }


            if (AllowClickRating)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnStarTapped;
                image.GestureRecognizers.Add(tapGestureRecognizer);
            }

            Control.Children.Add(image);
            this.Control.SetColumn(image, i);


            if (Animate)
                stars[i] = ApplyCustomStyle(image);
            else
                stars[i] = image;
        }

        UpdateStars();
    }


    private void RegenerateStars()
    {
        Control.Children.Clear();

        Control.ColumnDefinitions.Clear();

        stars = new Image[Maximum];

        for (int i = 0; i < Maximum; i++)
        {
            Control.ColumnDefinitions.Add(new ColumnDefinition { Width = StarSize });

            Image image = new();
            if (i <= Value)
            {

                image.Source = new FontImageSource()
                {
                    Glyph = "\uf005",
                    FontFamily = "FontSolid",
                    Color = StarColor,
                    Size = StarSize,
                };
                image.WidthRequest = StarSize;
                image.HeightRequest = StarSize;
                image.Aspect = Aspect.AspectFit;

            }
            else if (i > Value)
            {
                image.Source = new FontImageSource()
                {
                    Glyph = "\uf005",
                    FontFamily = "FontSolid",
                    Color = Colors.DarkGray,
                    Size = StarSize,
                };
                image.WidthRequest = StarSize;
                image.HeightRequest = StarSize;
                image.Aspect = Aspect.AspectFit;
            }


            if (AllowClickRating)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnStarTapped;
                image.GestureRecognizers.Add(tapGestureRecognizer);
            }

            Control.Children.Add(image);
            this.Control.SetColumn(image, i);

            if (Animate)
                stars[i] = ApplyCustomStyle(image);
            else
                stars[i] = image;
        }

        UpdateStars();
    }

    private Image ApplyCustomStyle(Image image)
    {
        Style imageStyle = new(typeof(Image));

        VisualStateGroup commonStatesGroup = new() { Name = "CommonStates" };

        VisualState normalState = new() { Name = "Normal" };

        VisualState pointerOverState = new() { Name = "PointerOver" };
        Setter pointerOverSetter = new()
        {
            Property = Image.ScaleProperty,
            Value = 1.05,
        };
        pointerOverState.Setters.Add(pointerOverSetter);

        VisualState clickedState = new() { Name = "Clicked" };
        Setter scaleState = new()
        {
            Property = Image.ScaleProperty,
            Value = 0.9
        };
        clickedState.Setters.Add(scaleState);

        commonStatesGroup.States.Add(normalState);
        commonStatesGroup.States.Add(clickedState);
        commonStatesGroup.States.Add(pointerOverState);

        VisualStateManager.GetVisualStateGroups(image).Add(commonStatesGroup);

        image.Style = imageStyle;

        return image;
    }

    [Obsolete]
    protected override void OnControlInitialized(Grid control)
    {
        stars = new Image[Maximum];

        HorizontalOptions = LayoutOptions.CenterAndExpand;

        this.Control.ColumnSpacing = StarSpacing;

        //Generate columns
        //for (int i = 0; i < Maximum; i++)
        //{
        //    ColumnDefinition col = new();
        //    col.Width = new GridLength(StarSize, GridUnitType.Star);
        //    this.Control.ColumnDefinitions.Add(col);
        //}

        GenerateStars();
    }
    #endregion

    #region Events
    private static void OnMaximumChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnStarSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnStarColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnStartSpacingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnAllowClickRatingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnAnimateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private static void OnCommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ratingView = bindable as RatingView;

        ratingView.RegenerateStars();
    }

    private void OnStarTapped(object sender, TappedEventArgs e)
    {
        var tappedImage = sender as Image;

        if (tappedImage is null) return;

        var columnIndex = this.Control.GetColumn(tappedImage);

        Value = columnIndex + 1;

        Rating rating = new()
        {
            Value = Value,
            Parameter = CommandParameter
        };
        if (Command is not null && Command.CanExecute(rating))
        {
            Command.Execute(rating);
        }
    }
    #endregion
}