using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class MainPage : ContentPage
    {
        HomeViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = _viewModel = new HomeViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"HelpPage");
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Version 1.0.0; Build 1", $"Copyright {DateTime.Now.Year} - Aaron Karr - made with love <3 \r\n\r\n GitHub URL: https://github.com/Aaronkarr11/HalcyonHomeManager", "OK");
        }
    }
}