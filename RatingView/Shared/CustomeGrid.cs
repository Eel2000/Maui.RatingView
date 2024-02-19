using System.Collections.ObjectModel;

namespace RatingView.Shared
{
    public class CustomeGrid<TControl> : Border where TControl : class, new()
    {
        protected TControl Control { get; private set; }


        public CustomeGrid()
        {
            Content = Control as View;
        }

    }
}
