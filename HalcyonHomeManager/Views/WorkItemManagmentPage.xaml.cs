using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkItemManagmentPage : ContentPage
    {
        WorkItemManagmentViewModel _viewModel;
        public WorkItemManagmentPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new WorkItemManagmentViewModel();
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
    }
}