using HalcyonHomeManager.Interfaces;
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
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = _viewModel = new WorkItemManagmentViewModel(service);
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

        private void MyPicker_Focused(object sender, FocusEventArgs e)
        {
            if (BindingContext is WorkItemManagmentViewModel vm && sender is Picker picker)
            {
                vm.GetSelectedProjectsCommand?.Execute(picker);

            }
        }

    }
}