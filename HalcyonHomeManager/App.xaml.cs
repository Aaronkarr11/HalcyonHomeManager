#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
using HalcyonHomeManager.BusinessLogic;
using HalcyonHomeManager.DataLayer;
using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.Models;
using HalcyonHomeManager.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;


namespace HalcyonHomeManager;

public partial class App : Application
{
    public HttpClient Client { get; set; }

    public static IServiceProvider _services;
    public static IAlertService _alertSvc;
    //WorkTaskDatabase _workTaskDatabase;
    //ProjectDatabase _projectDatabase;
    //ErrorLogDatabase _errorLogDatabase;
    //HouseHoldDatabase _houseHoldDatabase;
    //RequestItemsDatabase _requestItemsDatabase;

    const int WindowWidth = 1000;
    const int WindowHeight = 800;

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
#if WINDOWS
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
#endif
        });
        Client = new HttpClient();
        _services = provider;
        Client.Timeout = TimeSpan.FromSeconds(10);
        //var _workTaskDatabase = _services.GetService<WorkTaskDatabase>();
        //_projectDatabase = _services.GetService<ProjectDatabase>();
        //_errorLogDatabase = _services.GetService<ErrorLogDatabase>();
        //_houseHoldDatabase = _services.GetService<HouseHoldDatabase>();
        //_requestItemsDatabase = _services.GetService<RequestItemsDatabase>();

        //var appService = new TransactionManager(_workTaskDatabase, _projectDatabase, _errorLogDatabase, _houseHoldDatabase, _requestItemsDatabase);

        var appService = new TransactionManager();
        _alertSvc = _services.GetService<IAlertService>();
        DependencyService.RegisterSingleton<ITransactionManager>(appService);
        MainPage = new AppShell();

        LiveCharts.Configure(config =>
        config
        .AddSkiaSharp()
        .AddDefaultMappers()
        .AddLightTheme()
        .HasMap<LineGraphModelItem>((completed, point) =>
        {
            // use the city Population property as the primary value
            point.PrimaryValue = (int)completed.TotalCompleted;
            // and the index of the city in our cities array as the secondary value
            point.SecondaryValue = point.Context.Index;
        }));
    }
}
