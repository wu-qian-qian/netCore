using StackExchange.Redis;
using Commons;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Drawing;
namespace Redis
{
    public interface IRedisHelper
    {
        IEnumerable<RedisKey> GetRedisKeys(int size = 500);
        bool DeleteKey(RedisKey key);
    }
    public class RedisHelper : IRedisHelper
    {
        //用来存set list 的key 供后续的删除，更新使用
        private readonly IConnectionMultiplexer redis;
        private readonly IServer server;
        private readonly IDatabase db;
        private readonly static int mydb = 0;
        public RedisHelper(IConnectionMultiplexer redis, IServer server)
        {
            this.redis = redis;
            this.server = server;
            db = redis.GetDatabase();
        }

        public async Task InserStringAsync<TKey, TResult>(TKey key, TResult value, DateType dateType = DateType.Josn, TimeSpan? time = null) where TResult : class
        {
            string reKey = key.GetHashCode().ToString();
            string reValue = "";
            switch (dateType)
            {
                case DateType.Josn:
                    reValue = value.ToJsonString();
                    break;
                case DateType.String:
                    reValue = value.ToString();
                    break;
                default:
                    break;
            }
            await db.StringSetAsync(reKey, reValue, time);
        }

        public async Task InserSetAsync<TKey, TResult>(TKey key, TResult value, DateType dateType = DateType.Josn) where TResult : class
        {
            string reKey = key.GetHashCode().ToString();
            string reValue = "";
            switch (dateType)
            {
                case DateType.Josn:
                    reValue = value.ToJsonString();
                    break;
                case DateType.String:
                    reValue = value.ToString();
                    break;
                default:
                    break;
            }
            await db.SetAddAsync(reKey, reValue);
        }

        /// <summary>
        ///插入哈希
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="filed">哈希列表key</param>
        /// <param name="value">存储值</param>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public async Task InserHashAsync<TKey, TResult>(TKey key, string filed, TResult value, DateType dateType = DateType.Josn)
        {
            string reKey = key.GetHashCode().ToString();
            string reValue = "";
            switch (dateType)
            {
                case DateType.Josn:
                    reValue = value.ToJsonString();
                    break;
                case DateType.String:
                    reValue = value.ToString();
                    break;
                default:
                    break;
            }
            await db.HashSetAsync(reKey, filed, reValue);
        }

        public async Task InserListAsync<TKey, TResult>(TKey key, TResult value, DateType dateType = DateType.Josn) where TResult : class
        {
            string reKey = key.GetHashCode().ToString();
            string reValue = "";
            switch (dateType)
            {
                case DateType.Josn:
                    reValue = value.ToJsonString();
                    break;
                case DateType.String:
                    reValue = value.ToString();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// josn格式取
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Result<TResult>> GetStringAsync<Tkey, TResult>(Tkey key) where TResult : class
        {
            string reKey = key.GetHashCode().ToString();
            string json = await db.StringGetAsync(reKey);
            await db.KeyDeleteAsync(reKey);
            Result<TResult> tRes = null;
            try
            {
                TResult result = json.ParseJson<TResult>();
                tRes = new Result<TResult>(result, "");
                return tRes;
            }
            catch (Exception ex)
            {
                tRes = new Result<TResult>(null, $"解析失败：{ex.Message}");
                return tRes;
                //
            }
        }


        /// <summary>
        /// 直接取
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Result<string>> GetStringAsync<Tkey>(Tkey key)
        {
            string reKey = key.GetHashCode().ToString();
            string json = await db.StringGetAsync(reKey);
            await db.KeyDeleteAsync(reKey);
            Result<string> tRes = new Result<string>(json, "");
            return tRes;
        }

        /// <summary>
        /// Set存储
        /// 以key value形式存储，value是以字符的格式存储
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Result<TResult>>> GetSetAsync<Tkey, TResult>(Tkey key, int index = 10, bool isAll = false) where TResult : class
        {
            string reKey = key.GetHashCode().ToString();
            List<Result<TResult>> resultList = new List<Result<TResult>>();
            IEnumerable<RedisValue> value = null;
            if (isAll)
            {
                value = (await db.SetMembersAsync(reKey));
            }
            else
            {
                value = (await db.SetMembersAsync(reKey)).Take(index);
            }
            try
            {
                foreach (var item in value)
                {
                    var json = Convert.ToString(item);
                    TResult result = json.ParseJson<TResult>();
                    resultList.Add(new Result<TResult>(result, ""));
                }
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new Result<TResult>(null, $"解析失败：{ex.Message}"));
                return resultList;
            }
        }
       public async Task<Result<List<string>>> GetSetAsync<Tkey>(Tkey key, int index = 10, bool isAll = false)
        {
            string reKey = key.GetHashCode().ToString();
            IEnumerable<RedisValue> value = null;
            if (isAll)
            {
                value = (await db.SetMembersAsync(reKey));
            }
            else
            {
                value = (await db.SetMembersAsync(reKey)).Take(index);
            }
            List<string> tempStrs = new List<string>();
            foreach (var item in value)
            {
                var json = Convert.ToString(item);
                tempStrs.Add(json);
            }
            return new Result<List<string>>(tempStrs, "");
        }
        public bool DeleteKey(RedisKey key)
        {
            return db.KeyDelete(key);
        }
        public IEnumerable<RedisKey> GetRedisKeys(int size = 500)
        {
            return server.Keys(database: mydb, pattern: "*", size);
        }
    }
    public record Result<T>(T ObjectVlue, string Message);
    public enum DateType
    {
        Josn,
        String,
    }
}