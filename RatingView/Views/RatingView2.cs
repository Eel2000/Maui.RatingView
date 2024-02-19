using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using RatingView.Enums;
using RatingView.Models;
using RatingView.Shared;
using RatingView.Utils;
using RatingView.Utils.Constants;

namespace RatingView.Views;

public sealed class RatingView2 : Border
{
    #region Private Properties

    private Microsoft.Maui.Controls.Shapes.Path[] _shapes;

    private string _shape;

    private PathFigureCollection _converted;

    //private readonly List<IView> _myChildren = new List<IView>();
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

    public RatingShapes Shape
    {
        get => (RatingShapes)GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    //public new IReadOnlyList<IView> Children => _myChildren;

    #endregion

    Grid content = new();

    public new View Content { get => base.Content; private set => base.Content = value; }

    public RatingView2()
    {
        _shapes = new Microsoft.Maui.Controls.Shapes.Path[Maximum];


        HorizontalOptions = LayoutOptions.CenterAndExpand;

        this.content.ColumnSpacing = Spacing;

        InitializeShape();

        DrawBase();

        //Content = content;

        this.Stroke = Colors.Transparent;
    }

    #region Events

    private static void OnBindablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        //re-draw forms.
        ((RatingView2)bindable).ReDraw();
    }

    private static void OnShapePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((RatingView2)bindable).InitializeShape();
        ((RatingView2)bindable).ReDraw();
    }

    private void OnShapeTapped(object sender, TappedEventArgs e)
    {
        var tappedShape = sender as Microsoft.Maui.Controls.Shapes.Path;


        if (tappedShape is null) return;

        var columnIndex = this.content.GetColumn(tappedShape);


        if (Maximum > 1)
        {
            Value = columnIndex + 1;
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
            this.content.ColumnDefinitions.Add(new ColumnDefinition { Width = Size });
            Microsoft.Maui.Controls.Shapes.Path image = new();
            if (i <= Value)
            {
                image.Data = new PathGeometry(_converted);

                image.Fill = Fill;
                image.Stroke = Fill;
                image.Aspect = Stretch.Uniform;
                image.HeightRequest = Size;
                image.WidthRequest = Size;
            }
            else if (i > Value)
            {
                image.Data = new PathGeometry(_converted);

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

            this.content.Children.Add(image);
            this.content.SetColumn(image, i);

            _shapes[i] = image;
        }

        UpdateDraw();
    }

    private void ReDraw()
    {
        this.content.Children.Clear();

        this.content.ColumnDefinitions.Clear();

        _shapes = new Microsoft.Maui.Controls.Shapes.Path[Maximum];

        for (int i = 0; i < Maximum; i++)
        {
            this.content.ColumnDefinitions.Add(new ColumnDefinition { Width = Size });

            Microsoft.Maui.Controls.Shapes.Path image = new();
            if (i <= Value)
            {
                var c = PathConverter.ConvertStringPathToGeo(_shape);
                image.Data = new PathGeometry((PathFigureCollection)c);

                image.Fill = Fill;
                image.Stroke = Fill;
                image.Aspect = Stretch.Uniform;
                image.HeightRequest = Size;
                image.WidthRequest = Size;
            }
            else if (i > Value)
            {
                var c = PathConverter.ConvertStringPathToGeo(_shape);
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

            this.content.Children.Add(image);
            this.content.SetColumn(image, i);

            _shapes[i] = image;
        }

        UpdateDraw();
    }

    private void UpdateDraw()
    {
        for (int i = 0; i < Maximum; i++)
        {
            var image = _shapes[i];

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
                    var element = _shapes[(int)(Value - fraction)];
                    if (element != null)
                    {
                        var colors = new GradientStopCollection
                        {
                            new(Fill, (float)fraction),
                            new(EmptyColor, (float)fraction)
                        };

                        element.Fill =
                            new LinearGradientBrush(colors, new Point(0, 0), new Point(1, 0));
                        element.StrokeThickness = StrokeThickness;
                        element.StrokeLineJoin = PenLineJoin.Round;
                        element.Stroke = StrokeColor;
                    }
                }
            }
        }

        Content = content;
    }

    private PathFigureCollection InitializeShape()
    {
        switch (Shape)
        {
            case RatingShapes.Star:
                _shape = PathShapes.Star;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Star) as PathFigureCollection;
            case RatingShapes.Heart:
                _shape = PathShapes.Heart;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Heart) as PathFigureCollection;
            case RatingShapes.Like:
                _shape = PathShapes.Like;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Like) as PathFigureCollection;
            case RatingShapes.Dislike:
                _shape = PathShapes.Dislike;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Dislike) as PathFigureCollection;
            default:
                _shape = PathShapes.Star;
                return PathConverter.ConvertStringPathToGeo(PathShapes.Star) as PathFigureCollection;
        }
    }

    #endregion

}