using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.EventBus
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UserEventBus(this IApplicationBuilder builder)
        {
            object? bus = builder.ApplicationServices.GetService(typeof(IEventBus));
            if (bus == null)
                throw new ApplicationException("未获取到IEventBus实例");
            return builder;
        }
    }
}
