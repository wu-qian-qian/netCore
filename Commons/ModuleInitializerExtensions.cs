using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public static class ModuleInitializerExtensions
    {
        public static IServiceCollection InitModuleInitializer(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies) 
            {
                var moduleTypes = assembly.GetTypes().
                    Where(p=>!p.IsAbstract&&typeof(IModuleInitializer).IsAssignableFrom(p));
                foreach (var type in moduleTypes) 
                {
                    var obj = (IModuleInitializer?)Activator.CreateInstance(type);
                    if(obj==null)
                    {
                        throw new ApplicationException("程序初始化失败");
                    }
                    obj.Initialize(services);
                }
            }
            return services;
        }
    }
}
