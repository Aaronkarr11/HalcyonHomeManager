using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace HalcyonHomeManager.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ITransactionManager _transactionServices;
        private LiveChartsCore.Painting.Paint _labelTheme;
        public HomeViewModel(ITransactionManager transactionServices)
        {
            _transactionServices = transactionServices;

            var name = "Welcome!";
            Title = name;
        }

        public async void OnAppearing()
        {
            try
            {
                var theme = Application.Current.RequestedTheme;
                if (theme != AppTheme.Dark)
                {
                    _labelTheme = new SolidColorPaint(new SKColor(30, 30, 30));
                }
                else
                {
                    _labelTheme = new SolidColorPaint(new SKColor(255, 255, 255));
                }

                IsBusy = true;
                LineGraphTitle = $"Total Completed Work for {DateTime.Now.Year}";
                PieGraphTitle = $"Completed Percentage for {DateTime.Now.Year}";
                BarGraphTitle = $"Completion of Last Month vs Current Month";
                DashBoardData = await _transactionServices.GetDashBoardData();

                if (DashBoardData.NoWorkToShow)
                {
                    App._alertSvc.ShowAlert("No Data!", $"There is no work to show here. Go to the Work tab to create a 'New Project'. New 'Work Tasks' can then be managed");
                }

                //Find max value for linegraph
                int maxNumber = 0;
                var Values = DashBoardData.lineGraphModel;
                foreach (var month in Values)
                {
                    if (maxNumber < month.TotalCompleted)
                    {
                        maxNumber = month.TotalCompleted;
                    }
                }
                maxNumber = maxNumber + 2;

                DashBoardData.percentageData.percentCompleted = DashBoardData?.percentageData?.percentCompleted.ToString() == "NaN" ? 0 : DashBoardData.percentageData.percentCompleted;
                DashBoardData.percentageData.percentUnCompleted = DashBoardData?.percentageData?.percentUnCompleted.ToString() == "NaN" ? 100 : DashBoardData.percentageData.percentUnCompleted;
                var graphData = new double[] { DashBoardData.percentageData.percentUnCompleted, DashBoardData.percentageData.percentCompleted };
                BarSeries = new ISeries[]
                   {
                       
           new ColumnSeries<int>
        {
               
             ShowDataLabels = true,
            Name = DashBoardData.barGraphData.LastMonth,
            Values = new [] { DashBoardData.barGraphData.CompletedCountForLastMonth},
           Fill = new SolidColorPaint(SKColors.Blue),
           DataLabelsPaint = _labelTheme,
           DataLabelsSize = 12,
           DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
            DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 10),
            
            },
            new ColumnSeries<int>
        {
                         ShowDataLabels = true,
            Name = DashBoardData.barGraphData.CurrentMonth,
            Values = new [] { DashBoardData.barGraphData.CompletedCountForCurrentMonth },
            Fill = new SolidColorPaint(SKColors.LightBlue),
            DataLabelsPaint = _labelTheme,
            DataLabelsSize = 12,
            DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
             DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 10),
        }
                   };

                BarXAxes = new List<Axis>
            {

                new Axis
                {
                 LabelsPaint = null,
                 Labels = null
                }

            };

                BarYAxes = new List<Axis>
            {
                new Axis
                {
                     LabelsPaint = _labelTheme,
                    TextSize = 12,
                    MinStep = 0
                }

            };

                var data = new double[] { DashBoardData.percentageData.percentUnCompleted, DashBoardData.percentageData.percentCompleted };
                int counter = 1;
                Series = data.AsPieSeries((value, series) =>
                {
                    if (counter == 1)
                    {
                        series.Name = $"Uncompleted Work Tasks: {value}%";
                        series.DataLabelsPaint = _labelTheme;

                        series.Fill = new SolidColorPaint(SKColors.Yellow); series.DataLabelsSize = 0;
                        series.ToolTipLabelFormatter = p => $"{p.AsDataLabel} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
                    }
                    else if (counter == 2) { series.Name = $"Completed Work Tasks: {value}%"; series.DataLabelsPaint = _labelTheme; series.Fill = new SolidColorPaint(SKColors.Green); series.DataLabelsSize = 0; series.ToolTipLabelFormatter = p => $"{p.AsDataLabel} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})"; }
                    counter++;
                }).ToArray();


                List<string> labels = new List<string>();

                foreach (var item in DashBoardData.lineGraphModel)
                {
                    labels.Add(item.Name);
                }
                ;

                List<LineGraphModelItem> lineGraphModels = new List<LineGraphModelItem>();

                SeriesCollection = new ISeries[]
                {
    new LineSeries<LineGraphModelItem>
    {
        Values = new List<LineGraphModelItem>(lineGraphModels = DashBoardData.lineGraphModel),

        Fill = new SolidColorPaint(SKColors.LightSkyBlue),
        Name = $"Total Completed Work",
        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 1 },
        GeometryStroke = new SolidColorPaint(SKColors.DarkBlue){ StrokeThickness = 1 },
        ShowDataLabels = true,
        DataLabelsSize = 12,
        DataLabelsPaint = _labelTheme,
        DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
        DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 1),
        XToolTipLabelFormatter =
            (chartPoint) => $"{chartPoint.Model.Name}: {chartPoint.Model.TotalCompleted}"
    }
                };

                XAxes = new List<Axis>
                 {
                     new Axis
                     {
                         Labels = labels,
                         TextSize = 12,
                         MinStep = 0
                     }
                 };

                YAxes = new List<Axis>
                 {
                     new Axis
                     {
                         MinLimit = -1,
                         MaxLimit = maxNumber,
                         TextSize = 12
                     }
                 };

            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "HomeViewModel", "OnAppearing");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }



        private DashBoard _dashboard;
        public DashBoard DashBoardData
        {
            get => _dashboard;
            set => SetProperty(ref _dashboard, value);
        }

        private string _lineGraphTitle;
        public string LineGraphTitle
        {
            get => _lineGraphTitle;
            set => SetProperty(ref _lineGraphTitle, value);
        }

        private string _pieGraphTitle;
        public string PieGraphTitle
        {
            get => _pieGraphTitle;
            set => SetProperty(ref _pieGraphTitle, value);
        }

        private string _barGraphTitle;
        public string BarGraphTitle
        {
            get => _barGraphTitle;
            set => SetProperty(ref _barGraphTitle, value);
        }

        private IEnumerable<ISeries> _series;
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        private IEnumerable<ISeries> _barSeries;
        public IEnumerable<ISeries> BarSeries
        {
            get => _barSeries;
            set => SetProperty(ref _barSeries, value);
        }

        private IEnumerable<ISeries> _barSeries2;
        public IEnumerable<ISeries> BarSeries2
        {
            get => _barSeries2;
            set => SetProperty(ref _barSeries2, value);
        }



        private ISeries[] _lineSeries;
        public ISeries[] SeriesCollection
        {
            get => _lineSeries;
            set => SetProperty(ref _lineSeries, value);
        }

        private List<Axis> _xaxes;
        public List<Axis> XAxes
        {
            get => _xaxes;
            set => SetProperty(ref _xaxes, value);
        }

        private List<Axis> _barXaxes;
        public List<Axis> BarXAxes
        {
            get => _barXaxes;
            set => SetProperty(ref _barXaxes, value);
        }

        private List<Axis> _barYaxes;
        public List<Axis> BarYAxes
        {
            get => _barYaxes;
            set => SetProperty(ref _barYaxes, value);
        }

        private List<Axis> _barXaxes2;
        public List<Axis> BarXAxes2
        {
            get => _barXaxes2;
            set => SetProperty(ref _barXaxes2, value);
        }

        private List<Axis> _barYaxes2;
        public List<Axis> BarYAxes2
        {
            get => _barYaxes2;
            set => SetProperty(ref _barYaxes2, value);
        }

        private List<Axis> _yaxes;
        public List<Axis> YAxes
        {
            get => _yaxes;
            set => SetProperty(ref _yaxes, value);
        }


        //public IEnumerable<ISeries> Series { get; set; }
    }
}
