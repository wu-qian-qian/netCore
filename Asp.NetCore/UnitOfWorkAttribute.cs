using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    [AttributeUsage(AttributeTargets.Class
    | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
        public UnitOfWorkAttribute(params Type[] dbContext)
        {
            DbContext = dbContext;
            foreach (var item in DbContext)
            {
                if (!typeof(DbContext).IsAssignableFrom(item))
                    throw new ArgumentException("出现非DBConext类");
            }
        }

        public Type[] DbContext { get; init; }
    }
}
