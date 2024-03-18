using Longbow.Tasks;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;





namespace Redis

{

    public static class ServicesCollectionExtensions

    {

        public static IServiceCollection AddRedis(this IServiceCollection services, string redisConn)

        {

            IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConn);

            IServer server = redisConnMultiplexer.GetServer(redisConn);

            //services.AddSingleton(typeof(IServer), server);

            //services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);

            services.AddSingleton<IRedisHelper>(sp => { return new RedisHelper(redisConnMultiplexer, server); });

            services.AddTaskServices();

            //59秒调一次

            TaskServicesManager.GetOrAdd<UpdateRedis>("RedisUpdate", TriggerBuilder.Build("*/59 * * * * *"));

            return services;

        }

    }

}