﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace ClientBus.Services
{
  static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
          .AddTransient<ServiceUser> ()
         .AddTransient<ServiceTrip> ()
            .AddTransient<ServiceTicket>()
            ;
    }
}
