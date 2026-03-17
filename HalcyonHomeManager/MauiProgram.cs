using CommunityToolkit.Maui;
using HalcyonHomeManager.BusinessLogic;
using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.Services;
using LiveChartsCore.SkiaSharpView.Maui;
using Maui.FixesAndWorkarounds;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;



namespace HalcyonHomeManager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp()
            .UseLiveCharts()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMauiWorkarounds()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IAlertService, AlertService>();
       // builder.Services.AddSingleton<RequestItemsDatabase>();
        //builder.Services.AddSingleton<ErrorLogDatabase>();
        //builder.Services.AddSingleton<HouseHoldDatabase>();
       // builder.Services.AddSingleton<ProjectDatabase>();
       // builder.Services.AddSingleton<WorkTaskDatabase>();

        builder.Services.AddTransient<ITransactionManager, TransactionManager>();
        return builder.Build();
    }
}
