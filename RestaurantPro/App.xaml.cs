using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPro.Core;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure;
using RestaurantPro.Infrastructure.Services;
using RestaurantPro.Login;
using Unity;

namespace RestaurantPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IInventoryService, InventoryService>();

            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();

            var window = new MainWindow{DataContext = mainWindowViewModel};
            window.Show();

        }
    }
}
