using DataAccessLanguage.Demo.Blazor;
using DataAccessLanguage.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace DataAccessLanguage.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddBlazorWebView();
            serviceCollection.AddSingleton<HttpClient>();
            serviceCollection.AddSingleton<IJsLazyLoad, JsLazyLoad>();
            serviceCollection.AddSingleton<ICssLazyLoad, CssLazyLoad>();

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new JsonToObjectConverter());

            serviceCollection.AddDataAccessLanguage(
                (x, s) =>
                {
                    x.Add("getAssemblyName", x => new GetAssemblyFunc());
                },
                null,
                jsonOptions
            );
            Resources.Add("services", serviceCollection.BuildServiceProvider());


            InitializeComponent();
        }
    }
}
