using RatingView.Sample.ViewModels;
using RatingView.Views;

namespace RatingView.Sample
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

            var rating = new RatingView2()
            {
                Children = {  }
            };
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}