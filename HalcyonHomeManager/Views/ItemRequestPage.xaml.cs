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
            BindingContext = _viewModel = new ItemRequestViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}