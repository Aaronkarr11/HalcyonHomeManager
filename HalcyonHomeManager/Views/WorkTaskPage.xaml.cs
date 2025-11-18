using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class WorkTaskPage : ContentPage
    {
        public WorkTaskPage()
        {
            InitializeComponent();
            BindingContext = new WorkTaskViewModel(); 
        }

    }
}