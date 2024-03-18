using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Froum.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Forum.DomainModels;

namespace EntityFrameworkCore
{
    public class BaseDbContext:DbContext
    {
        private IMediator mediator;

        public BaseDbContext(DbContextOptions options,IMediator mediator):base(options)
        {
            this.mediator = mediator;
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("请使用异步的方法");
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (mediator != null)
            {
                await mediator.DispatchDomainEventsAsync(this);
            }
            var softDetele = this.ChangeTracker.Entries<ISoftDelete>()
                .Where(p => EntityState.Modified == p.State && p.Entity.IsDelete)
                .Select(p => p.Entity).ToList();
            var result =await SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            softDetele.ForEach(p => this.Entry(p).State = EntityState.Detached);
            return result;
        }
    }
}
