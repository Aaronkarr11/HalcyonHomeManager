using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class HouseHoldMemberPage : ContentPage
    {
        public HouseHoldMemberPage()
        {
            InitializeComponent();
            BindingContext = new HouseHoldMemberViewModel();
        }
    }
}