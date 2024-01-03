using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace ClientBus.Views.Windows
{
   static class WindowRegister
    {
        public static IServiceCollection AddWindow(this IServiceCollection services) => services
            .AddTransient<MainWindow>()
            .AddTransient<RegisterWindow>()
            .AddSingleton<WorkWindow>()
            .AddTransient<TicketWindow>()
            ;
    }
}
