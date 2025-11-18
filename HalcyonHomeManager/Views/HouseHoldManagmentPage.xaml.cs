using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class HouseHoldManagmentPage : ContentPage
    {
        HouseHoldManagmentViewModel _viewModel;
        public HouseHoldManagmentPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HouseHoldManagmentViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}