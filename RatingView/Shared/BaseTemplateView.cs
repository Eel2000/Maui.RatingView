namespace RatingView.Shared;

public class BaseTemplateView<TControl> : TemplatedView where TControl : View, new()
{
    protected TControl? Control { get; private set; }

    public BaseTemplateView()
        => ControlTemplate = new ControlTemplate(typeof(TControl));

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (Control != null)
            Control.BindingContext = BindingContext;
    }

    protected override void OnChildAdded(Element child)
    {
        if (Control is null && child is TControl control)
        {
            Control = control;
            OnControlInitialized(Control);
        }

        base.OnChildAdded(child);
    }

    protected abstract void OnControlInitialized(TControl control);
}
