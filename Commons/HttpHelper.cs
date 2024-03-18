using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace Commons
{
    public static class HttpHelper
    {
        public static async Task<T?> GetJosnAsync<T>(HttpClient http,Uri uri,CancellationToken cancellationToken = default)
        {
            string result =await http.GetStringAsync(uri, cancellationToken);
            return result.ParseJson<T>();
        }

        public static async Task SaveToFileAsync(this HttpResponseMessage httpRequest,string path,CancellationToken cancellationToken = default)
        {
            if (httpRequest.IsSuccessStatusCode == false)
                throw new ArgumentException($"状态码移除{nameof(httpRequest)}");
            using FileStream info = new FileStream(path, FileMode.CreateNew);
            await httpRequest.Content.CopyToAsync(info, cancellationToken);
        }
        public static async Task<HttpStatusCode> DownlodaFileAsync(this HttpClient client,Uri uri,string localPath,CancellationToken cancellationToken = default)
        {
            var rep =await client.GetAsync(uri, cancellationToken);
            if (rep.IsSuccessStatusCode)
            {
                await SaveToFileAsync(rep, localPath,cancellationToken);
                return rep.StatusCode;
            }
            return HttpStatusCode.OK;
        }
    }
}
