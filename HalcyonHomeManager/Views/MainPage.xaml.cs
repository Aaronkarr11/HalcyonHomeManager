using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace HalcyonHomeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class MainPage : ContentPage
    {
        HomeViewModel _viewModel;
        private LiveChartsCore.Painting.Paint _labelTheme;
        public MainPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<ITransactionManager>();
            BindingContext = _viewModel = new HomeViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            _viewModel.OnAppearing();

            var theme = Application.Current.RequestedTheme;
            if (theme != AppTheme.Dark)
            {
                _labelTheme = new SolidColorPaint(new SKColor(30, 30, 30));
            }
            else
            {
                _labelTheme = new SolidColorPaint(new SKColor(255, 255, 255));
            }




            PieChart.LegendTextPaint = _labelTheme;
            PieChart.LegendTextSize = 14;

            LineChart.LegendTextSize = 12;

            if (LineChart.Series != null)
            {
                foreach (var series in LineChart.Series)
                {
                    series.DataLabelsPaint = _labelTheme;
                    series.DataLabelsSize = 12;
                    series.ShowDataLabels = true;
                }
            }


            BarChart.LegendTextPaint = _labelTheme;
            BarChart.LegendTextSize = 14;

            ApplyChartTheme();


            if (LineChart.XAxes is not null)
            {
                foreach (var axis in LineChart.XAxes)
                    axis.LabelsPaint = _labelTheme;
            }

            if (LineChart.YAxes is not null)
            {
                foreach (var axis in LineChart.YAxes)
                    axis.LabelsPaint = _labelTheme;
            }

            LineChart.InvalidateMeasure();

        }

        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"HelpPage");
        }

        [Obsolete]
        private void AboutButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Version 1.0.8; Build 8", $"Copyright {DateTime.Now.Year} - Aaron Karr - made with love <3 \r\n\r\n GitHub URL: https://github.com/Aaronkarr11/HalcyonHomeManager", "OK");
        }

        private void OnBarChartLoaded(object sender, EventArgs e)
        {
            if (BarChart.Series is null)
                return;

            foreach (var series in BarChart.Series)
            {
                series.DataLabelsPaint = _labelTheme; // your white/dark paint
                series.DataLabelsSize = 14;
                series.ShowDataLabels = true;         // required for ColumnSeries
            }
        }

        private void OnLineChartLoaded(object sender, EventArgs e)
        {
            if (LineChart.Series is null)
                return;

            foreach (var series in LineChart.Series)
            {
                series.DataLabelsPaint = _labelTheme; 
                series.DataLabelsSize = 12;
                series.ShowDataLabels = true; 
            }
            LineChart.LegendTextSize = 12;
    
            if (LineChart.XAxes is not null)
            {
                foreach (var axis in LineChart.XAxes)
                    axis.LabelsPaint = _labelTheme;
            }

            if (LineChart.YAxes is not null)
            {
                foreach (var axis in LineChart.YAxes)
                    axis.LabelsPaint = _labelTheme;
            }

        }

        private void ApplyChartTheme()
        {
            if (LineChart.Series is not null)
            {
                foreach (var series in LineChart.Series)
                {
                    series.DataLabelsPaint = _labelTheme;
                    series.DataLabelsSize = 12;
                    series.ShowDataLabels = true;
                }
            }
            LineChart.LegendTextSize = 12;

            if (LineChart.XAxes is not null)
            {
                foreach (var axis in LineChart.XAxes)
                    axis.LabelsPaint = _labelTheme;
            }

            if (LineChart.YAxes is not null)
            {
                foreach (var axis in LineChart.YAxes)
                    axis.LabelsPaint = _labelTheme;
            }
        }


    }
}