using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemRequestPage : ContentPage
    {
        ItemRequestViewModel _viewModel;
        public ItemRequestPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = _viewModel = new ItemRequestViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}