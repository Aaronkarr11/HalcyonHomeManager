using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ErrorLogPage : ContentPage
{
    ErrorLogViewModel _viewModel;
    public ErrorLogPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new ErrorLogViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}