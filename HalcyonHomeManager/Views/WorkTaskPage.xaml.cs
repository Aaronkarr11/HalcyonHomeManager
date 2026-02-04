using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class WorkTaskPage : ContentPage
    {
        public WorkTaskPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = new WorkTaskViewModel(service);
        }

    }
}