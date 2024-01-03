using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;



namespace ClientBus.ViewModels
{
   static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddTransient<MainWindowViewModel>()
            .AddTransient<RegisterWindowViewModel>()
            .AddTransient<LoginWindowViewModel>()
            .AddSingleton<WorkWindowViewModel>()
            ;
        
    }
}
