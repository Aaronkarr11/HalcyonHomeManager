using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = new NewItemViewModel(service);
        }
    }
}