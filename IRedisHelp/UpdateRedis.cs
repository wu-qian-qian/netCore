using Longbow.Tasks;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;



namespace Redis

{

    public class UpdateRedis : ITask

    {









        public async Task Execute(IServiceProvider provider, CancellationToken cancellationToken)

        {

            IRedisHelper redis = provider.CreateScope().ServiceProvider.GetService<IRedisHelper>();

            //

            var keys = redis.GetRedisKeys();

            foreach (var item in keys)

            {

                redis.DeleteKey(item);

            }

            //拉取数据

        }

    }

}