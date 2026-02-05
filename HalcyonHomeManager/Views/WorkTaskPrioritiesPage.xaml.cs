using HalcyonHomeManager.Interfaces;
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
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = _viewModel = new WorkTaskPrioritiesViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}