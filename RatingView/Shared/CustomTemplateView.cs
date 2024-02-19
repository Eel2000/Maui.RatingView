using Microsoft.Maui.Controls;

namespace RatingView.Shared
{
    public abstract class CustomTemplateView<TControl> : Layout, IContentView
    {
        protected TControl Control { get; private set; }

        public object Content => Control;

        public IView PresentedContent => Content as IView;

        public CustomTemplateView()
        {

        }

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    if (Control != null)
        //        Control.BindingContext = BindingContext;
        //}

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
}
