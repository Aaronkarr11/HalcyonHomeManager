using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkTaskPrioritiesPage : ContentPage
    {
        WorkTaskPrioritiesViewModel _viewModel;
        public WorkTaskPrioritiesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new WorkTaskPrioritiesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}