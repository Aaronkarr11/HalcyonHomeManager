using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using HalcyonHomeManager.Models;


namespace HalcyonHomeManager.Charting;

public static class LiveChartsConfig
{
    public static void Configure()
    {
        LiveCharts.Configure(config =>
            config
                .AddSkiaSharp()
                .AddDefaultMappers()
                .AddLightTheme()


        .HasMap<LineGraphModelItem>(
        (model, point) => new(point, model.TotalCompleted)));

    }
}

