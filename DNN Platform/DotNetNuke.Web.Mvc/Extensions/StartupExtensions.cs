﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNetNuke.DependencyInjection.Extensions;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotNetNuke.Web.Mvc.Extensions
{
    public static class StartupExtensions
    {
        public static void AddMvcControllers(this IServiceCollection services)
        {
            var controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(TypeExtensions.SafeGetTypes)
                .Where(x => typeof(IDnnController).IsAssignableFrom(x)
                    && x.IsClass
                    && !x.IsAbstract);
            foreach (var controller in controllerTypes)
            {
                services.TryAddTransient(controller);
            }
        }
    }
}
