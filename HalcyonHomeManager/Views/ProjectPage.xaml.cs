using HalcyonHomeManager.ViewModels;

namespace HalcyonHomeManager.Views
{
    public partial class ProjectPage : ContentPage
    {
        public ProjectPage()
        {
            InitializeComponent();
            BindingContext = new ProjectViewModel();
        }
    }
}