using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class HouseHoldMemberPage : ContentPage
    {
        public HouseHoldMemberPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = new HouseHoldMemberViewModel(service);
        }
    }
}