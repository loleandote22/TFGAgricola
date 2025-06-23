using AplicacionTFG.Presentation.Eventos;
using AplicacionTFG.Presentation.Inventario;
using AplicacionTFG.Presentation.Perfil;
using AplicacionTFG.Presentation.Personal;
using CommunityToolkit.Mvvm.Messaging;
using Uno.Resizetizer;

namespace AplicacionTFG;
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                // Register Json serializers (ISerializer and ISerializer)
                .UseSerialization((context, services) => services
                    .AddContentSerializer(context)
                    .AddJsonTypeInfo(WeatherForecastContext.Default.IImmutableListWeatherForecast))
                .UseHttp((context, services) =>
                {
#if DEBUG
                    // DelegatingHandler will be automatically injected
                    services.AddTransient<DelegatingHandler, DebugHttpHandler>();
#endif
                    services.AddSingleton<IWeatherCache, WeatherCache>();
                    //services.AddKiotaClient<WeatherServiceClient>(
                    //context,
                    //options: new EndpointOptions { Url = context.Configuration["ApiClient:Url"]! }
                    //);

                })
                .ConfigureServices((context, services) =>
                {
                    // TODO: Register your services
                    //services.AddSingleton<IMyService, MyService>();
                    services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);

                })
                .UseNavigation(RegistroRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegistroRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<LoginPage, LoginViewModel>(),
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<InicioPage, InicioViewModel>(),
            new ViewMap<PerfilPage, PerfilViewModel>(),
        #region Personal
            new ViewMap<PersonalPage, PersonalViewModel>(),
            new DataViewMap<PersonaPage, PersonaViewModel, EntityNumber>(),
        #endregion
        #region Inventario
            new ViewMap<InventarioPage, InventarioViewModel>(),
            new DataViewMap<ElementoPage, ElementoViewModel, EntityNumber>(),
            new DataViewMap<EdicionElementoPage, EdicionElementoViewModel, InventarioConsulta>(),
        #endregion
        #region Eventos
            new ViewMap<EventosMesPage, EventosMesViewModel>(),
            new ViewMap<AñadirEventoPage, AñadirEventoViewModel>(),
            new DataViewMap<EventosDiaPage, EventosDiaViewModel, EntityDate>(),
            new DataViewMap<EventoPage, EventoViewModel, EntityNumber>(),
        #endregion
            new DataViewMap<SecondPage, SecondViewModel, Entity>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new ("Login", View: views.FindByViewModel<LoginViewModel>(), IsDefault:true),
                    new ("Main", View: views.FindByViewModel<MainViewModel>(),
                    Nested: [
                        new ("Inicio", View: views.FindByViewModel<InicioViewModel>()),
                        new ("Inventario",View: views.FindByViewModel<InventarioViewModel>(),
                        Nested:[
                            new ("Elemento",View: views.FindByViewModel<ElementoViewModel>()),
                            new ("EdicionElemento",View: views.FindByViewModel<EdicionElementoViewModel>()),
                            ]),
                        new ("Personal", View: views.FindByViewModel<PersonalViewModel>()),
                        new ("Persona", View: views.FindByViewModel<PersonaViewModel>()),
                        new ("EventosMes", View: views.FindByViewModel<EventosMesViewModel>()),
                        new ("EventosDia", View: views.FindByViewModel<EventosDiaViewModel>()),
                        new ("Evento", View: views.FindByViewModel<EventoViewModel>()),
                        new ("Perfil", View: views.FindByViewModel<PerfilViewModel>()),
                    ]
                    ),
                    new ("Second", View: views.FindByViewModel<SecondViewModel>()),
                ]
            )
        );
    }
}
