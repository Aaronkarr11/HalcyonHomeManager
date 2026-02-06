using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ErrorLogPage : ContentPage
{
    ErrorLogViewModel _viewModel;
    public ErrorLogPage()
    {
        InitializeComponent();
        var service = DependencyService.Get<ITransactionManager>();
        BindingContext = _viewModel = new ErrorLogViewModel(service);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}