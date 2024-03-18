using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Transactions;

namespace System
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {

        private static UnitOfWorkAttribute? GetUnitOfWork(ActionDescriptor action)
        {
            var caDesc = action as ControllerActionDescriptor;
            if (caDesc == null)
                return null;
            UnitOfWorkAttribute? unitOfWork = caDesc.ControllerTypeInfo
                .GetCustomAttribute<UnitOfWorkAttribute>();
            if (unitOfWork != null)
                return unitOfWork;
            unitOfWork = caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            return unitOfWork;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = GetUnitOfWork(context.ActionDescriptor);
            if (result == null)
            {
                await next();
                return;
            }
            using TransactionScope txScope = new(TransactionScopeAsyncFlowOption.Enabled);
            List<DbContext> dbs = new List<DbContext>();
            foreach (var item in result.DbContext)
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var db = (DbContext)serviceProvider.GetRequiredService(item);
                dbs.Add(db);
            }
            var ex = await next();
            if (ex.Exception!=null)
            {
                foreach (var db in dbs)
                {
                    await db.SaveChangesAsync();
                }
                txScope.Complete();
            }
        }
    }
}