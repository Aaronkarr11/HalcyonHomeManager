using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class WMSchedulePage : ContentPage
{
    WMScheduleViewModel _viewModel;
    public WMSchedulePage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new WMScheduleViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}