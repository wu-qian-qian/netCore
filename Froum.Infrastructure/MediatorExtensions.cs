using Forum.DomainModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Froum.Infrastructure
{
    public static class MediatorExtensions
    {
 
        public static IServiceCollection AddMediatR(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
          return  services.AddMediatR(assemblies);
        }

        public static async Task DispatchDomainEventsAsync(this IMediator  mediator,DbContext ctx)
        {
            var entity = ctx.ChangeTracker.Entries<IDomainEvents>()
                .Where(p => p.Entity.GetDomainHanlde().Any()).ToList();
            var events = entity.SelectMany(p => p.Entity.GetDomainHanlde()).ToList();
            entity.ForEach(p => p.Entity.ClearDomainEvents());
            foreach (var item in events)
            {
              await  mediator.Publish(item);
            }
        }
    }
}
