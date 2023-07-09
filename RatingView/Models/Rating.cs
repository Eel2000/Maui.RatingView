namespace RatingView.Models;

#nullable enable
public class Rating
{
    /// <summary>
    /// The value of the rating
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// The additionnal data/ command param passed through
    /// </summary>
    public object? Parameter { get; set; }
}
