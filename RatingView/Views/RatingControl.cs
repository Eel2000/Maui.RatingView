using Microsoft.Maui.Controls.Shapes;
using RatingView.Enums;
using RatingView.Models;
using RatingView.Shared;
using RatingView.Utils;
using RatingView.Utils.Constants;
using System.Windows.Input;

namespace RatingView.Views;

public class RatingControl : BaseTemplateView<Grid>
{
    #region Private Properties

    private Microsoft.Maui.Controls.Shapes.Path[] shapes;

    private string shape;

    private PathFigureCollection converted;

    private int touchedTime = 0;

    #endregion

    #region Bindable Properties
    //Rating value bindable property
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(double),
            typeof(RatingControl),
            defaultValue: 0.0,
            propertyChanged: OnBindablePropertyChanged);


    //Maximum rating value Bindable property
    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(
            nameof(Maximum),
            typeof(int),
            typeof(RatingControl),
            defaultValue: 5,
            propertyChanged: OnBindablePropertyChanged);


    //Star size Bindable property
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(
            nameof(Size),
            typeof(double),
            typeof(RatingControl),
            defaultValue: 20.0,
            propertyChanged: OnBindablePropertyChanged);


    //Star Color Bindable property
    public static readonly BindableProperty FillProperty =
        BindableProperty.Create(
            nameof(Fill),
            typeof(Color),
            typeof(RatingControl),
            defaultValue: Colors.Yellow,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty EmptyColorProperty =
        BindableProperty.Create(
            nameof(EmptyColor),
            typeof(Color),
            typeof(RatingControl),
            defaultValue: Colors.White,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty StrokeColorProperty =
        BindableProperty.Create(
            nameof(StrokeColor),
            typeof(Color),
            typeof(RatingControl),
            defaultValue: Colors.Gray,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(
            nameof(StrokeThickness),
            typeof(double),
            typeof(RatingControl),
            defaultValue: 7.0,
            propertyChanged: OnBindablePropertyChanged);

    //Star Spacing Between Bindable Property
    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(RatingControl),
            defaultValue: 10.0,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty AllowRatingProperty =
        BindableProperty.Create(
            nameof(AllowRating),
            typeof(bool),
            typeof(RatingControl),
            defaultValue: false,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(RatingControl),
            defaultValue: null,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(RatingControl),
            defaultValue: null,
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty BindControlProperty =
        BindableProperty.Create(
            nameof(BindControl),
            typeof(object),
            typeof(RatingControl),
            propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty AnimateProperty =
        BindableProperty.Create(
            nameof(Animate),
            typeof(bool),
            typeof(RatingControl),
            defaultValue: true,
            propertyChanged: OnBindablePropertyChanged);

    public readonly BindableProperty ShapeProperty =
       BindableProperty.Create(
           nameof(Shape),
           typeof(RatingShapes),
           typeof(RatingControl),
           propertyChanged: OnShapePropertyChanged);

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

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public Color Fill
    {
        get => (Color)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public Color EmptyColor
    {
        get => (Color)GetValue(EmptyColorProperty);
        set => SetValue(EmptyColorProperty, value);
    }

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public bool AllowRating
    {
        get => (bool)GetValue(AllowRatingProperty);
        set => SetValue(AllowRatingProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandProperty, value);
    }

    public object BindControl
    {
        get => GetValue(BindControlProperty);
        set => SetValue(BindControlProperty, value);
    }

    public bool Animate
    {
        get => (bool)GetValue(AnimateProperty);
        set => SetValue(AnimateProperty, value);
    }

    public RatingShapes Shape
    {
        get => (RatingShapes)GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }
    #endregion

    public RatingControl()
    {
        shapes = new Microsoft.Maui.Controls.Shapes.Path[Maximum];

        HorizontalOptions = LayoutOptions.CenterAndExpand;

        this.Control.ColumnSpacing = Spacing;

        InitializeShape();

        DrawBase();
    }

    #region Events

    private static void OnBindablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        //re-draw forms.
        ((RatingControl)bindable).ReDraw();
    }

    private static void OnShapePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((RatingControl)bindable).InitializeShape();
        ((RatingControl)bindable).ReDraw();
    }

    private void OnShapeTapped(object sender, TappedEventArgs e)
    {
        var tappedShape = sender as Microsoft.Maui.Controls.Shapes.Path;


        if (tappedShape is null) return;

        var columnIndex = Control.GetColumn(tappedShape);


        if (Maximum > 1)
        {
            Value = columnIndex + 1;
        }
        else if (Maximum is 1 or 0)
        {
            if (BindControl is null)
            {
                Value = Value is 1 ? 0 : 1;
            }
            else if (BindControl is RatingControl)
            {
                touchedTime++;
                if (touchedTime >= 1)
                {
                    Value = Value == 1 ? 0 : 1;
                    ((RatingControl)BindControl).Value = 0;
                    touchedTime = 0;
                }
                else
                {
                    Value = Value == 1 ? 0 : 1;
                    ((RatingControl)BindControl).Value = Value == 1 ? 0 : 1;
                }
            }
        }

        var data = new Rating
        {
            Value = Value,
            Parameter = CommandParameter
        };

        if (Command is not null && Command.CanExecute(data))
        {
            Command.Execute(data);
        }
    }

    #endregion

    #region Methods
    private void DrawBase()
    {
        for (int i = 0; i < Maximum; i++)
        {

            Control.ColumnDefinitions.Add(new ColumnDefinition { Width = Size });
            Microsoft.Maui.Controls.Shapes.Path image = new();
            if (i <= Value)
            {
                image.Data = new PathGeometry(converted);

                image.Fill = Fill;
                image.Stroke = Fill;
                image.Aspect = Stretch.Uniform;
                image.HeightRequest = Size;
                image.WidthRequest = Size;

            }
            else if (i > Value)
            {
                image.Data = new PathGeometry(converted);

                image.Fill = EmptyColor;
                image.Stroke = StrokeColor;

                image.StrokeLineJoin = PenLineJoin.Round;
                image.StrokeThickness = StrokeThickness;

                image.Aspect = Stretch.Uniform;
                image.HeightRequest = Size;
                image.WidthRequest = Size;
            }


            if (AllowRating)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnShapeTapped;
                image.GestureRecognizers.Add(tapGestureRecognizer);
            }

            Control.Children.Add(image);
            this.Control.SetColumn(image, i);

            if (Animate)
                shapes[i] = ApplyCustomStyle(image);
            else
                shapes[i] = image;

        }

        UpdateDraw();
    }

    private void ReDraw()
    {
        Control.Children.Clear();

        Control.ColumnDefinitions.Clear();

        shapes = new Microsoft.Maui.Controls.Shapes.Path[Maximum];

        for (int i = 0; i < Maximum; i++)
        {
            Control.ColumnDefinitions.Add(new ColumnDefinition { Width = Size });

            Microsoft.Maui.Controls.Shapes.Path image = new();
            if (i <= Value)
            {
                var c = PathConverter.ConvertStringPathToGeo(shape);
                image.Data = new PathGeometry((PathFigureCollection)c);

                image.Fill = Fill;
                image.Stroke = Fill;
                image.Aspect = Stretch.Uniform;
                image.HeightRequest = Size;
                image.WidthRequest = Size;

            }
            else if (i > Value)
            {
                var c = PathConverter.ConvertStringPathToGeo(shape);
                image.Data = new PathGeometry((PathFigureCollection)c);

                image.Fill = EmptyColor;
                image.Stroke = StrokeColor;

                image.StrokeLineJoin = PenLineJoin.Round;
                image.StrokeLineCap = PenLineCap.Round;
                image.StrokeThickness = StrokeThickness;

                image.Aspect = Stretch.Uniform;
                image.HeightRequest = Size;
                image.WidthRequest = Size;
            }


            if (AllowRating)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnShapeTapped;
                image.GestureRecognizers.Add(tapGestureRecognizer);
            }

            Control.Children.Add(image);
            this.Control.SetColumn(image, i);

            if (Animate)
                shapes[i] = ApplyCustomStyle(image);
            else
                shapes[i] = image;
        }

        UpdateDraw();
    }

    private void UpdateDraw()
    {
        for (int i = 0; i < Maximum; i++)
        {
            var image = shapes[i];

            if (Value >= i + 1)
            {
                image.HeightRequest = Size;
                image.WidthRequest = Size;
                image.StrokeLineJoin = PenLineJoin.Round;
                image.StrokeThickness = StrokeThickness;
                image.Stroke = Fill;
            }
            else
            {
                if (Value % 1 == 0)
                {
                    image.Fill = EmptyColor;
                    image.Stroke = StrokeColor;
                    image.StrokeThickness = StrokeThickness;
                    image.StrokeLineJoin = PenLineJoin.Round;
                }
                else
                {
                    var fraction = Value - Math.Floor(Value);
                    var element = shapes[(int)(Value - fraction)];
                    if (element != null)
                    {
                        var colors = new GradientStopCollection
                        {
                            new GradientStop(Fill, (float)fraction),
                            new GradientStop(EmptyColor, (float)fraction)
                        };

                        element.Fill = new LinearGradientBrush(colors, new Point(0, 0), new Point(1, 0));
                        element.StrokeThickness = StrokeThickness;
                        element.StrokeLineJoin = PenLineJoin.Round;
                        element.Stroke = StrokeColor;
                    }
                }
            }
        }
    }

    private PathFigureCollection InitializeShape()
    {
        switch (Shape)
        {
            case RatingShapes.Star:
                shape = PathShapes.Star;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Star) as PathFigureCollection;
            case RatingShapes.Heart:
                shape = PathShapes.Heart;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Heart) as PathFigureCollection;
            case RatingShapes.Like:
                shape = PathShapes.Like;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Like) as PathFigureCollection;
            case RatingShapes.Dislike:
                shape = PathShapes.Dislike;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Dislike) as PathFigureCollection;
            default:
                shape = PathShapes.Star;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Star) as PathFigureCollection;
        }

    }

    private Microsoft.Maui.Controls.Shapes.Path ApplyCustomStyle(Microsoft.Maui.Controls.Shapes.Path image)
    {
        Style imageStyle = new(typeof(Microsoft.Maui.Controls.Shapes.Path));

        VisualStateGroup commonStatesGroup = new() { Name = "CommonStates" };

        VisualState normalState = new() { Name = "Normal" };

        VisualState pointerOverState = new() { Name = "PointerOver" };
        Setter pointerOverSetter = new()
        {
            Property = Microsoft.Maui.Controls.Shapes.Path.ScaleProperty,
            Value = 1.05,
        };
        pointerOverState.Setters.Add(pointerOverSetter);

        VisualState clickedState = new() { Name = "Touched" };
        Setter scaleState = new()
        {
            Property = Microsoft.Maui.Controls.Shapes.Path.ScaleProperty,
            Value = 0.1
        };
        clickedState.Setters.Add(scaleState);

        commonStatesGroup.States.Add(normalState);
        commonStatesGroup.States.Add(clickedState);
        commonStatesGroup.States.Add(pointerOverState);

        VisualStateManager.GetVisualStateGroups(image).Add(commonStatesGroup);

        image.Style = imageStyle;

        return image;
    }

    #endregion

    protected override void OnControlInitialized(Grid control)
    {
        shapes = new Microsoft.Maui.Controls.Shapes.Path[Maximum];

        converted = InitializeShape();

        HorizontalOptions = LayoutOptions.CenterAndExpand;

        this.Control.ColumnSpacing = Spacing;

        DrawBase();
    }
}
