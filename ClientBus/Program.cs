using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;



namespace ClientBus
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

         public static IHostBuilder CreateHostBuilder(string[] Args)
         {
             var host_builder = Host.CreateDefaultBuilder(Args);
             host_builder.UseContentRoot(Environment.CurrentDirectory);
             host_builder.ConfigureAppConfiguration((host , cfg)=>
             {
                 cfg.SetBasePath(Environment.CurrentDirectory);
                
                
             });
            host_builder.ConfigureServices(App.ConfigureServices);

            return host_builder;
         }

     }
        
    
}
