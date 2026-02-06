using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class HouseHoldManagmentPage : ContentPage
    {
        HouseHoldManagmentViewModel _viewModel;
        public HouseHoldManagmentPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = _viewModel = new HouseHoldManagmentViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}