using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using RasManager.Util;
using System.Text;
using System.Threading.Tasks;

namespace RasManager.Util
{
    public enum SendType
    {
        GET = 0,
        POST,
    }

    public class HttpApiClient : Singleton<HttpApiClient>
    {
        private HttpClient _client;

        const string url = @"http://localhost:3000/";

        public Uri BaseUrl
        {
            get { return _client.BaseAddress; }
            set { _client.BaseAddress = value; }
        }

        private HttpApiClient()
        {
            this._client = new HttpClient();
            BaseUrl = new Uri(url);

            try
            {
                string ipSettingFile = "server_ip.setting";
                if (File.Exists(ipSettingFile))
                {
                    BaseUrl = new Uri(File.ReadAllText(ipSettingFile));
                }
                else
                {
                    File.WriteAllText(ipSettingFile, url);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        ~HttpApiClient()
        {
            _client.Dispose();
        }

        public void SendData<T, K>(SendType senderType, string apiAddr, T data, Action<HttpStatusCode, K> callback)
        {
            switch (senderType)
            {
                case SendType.GET:
                    _client.GetAsync(apiAddr)
                        .ContinueWith(response =>
                        {
                            Trace.WriteLine($"Get Addr : {apiAddr}\nResponse : {response.Result.StatusCode}");

                            if (response.Result.StatusCode == HttpStatusCode.OK)
                            {
                                Trace.WriteLine(response.Result.Content.ReadAsStringAsync().Result);

                                response.Result.Content.ReadAsStringAsync()
                                    .ContinueWith(result =>
                                    {
                                        callback.Invoke(response.Result.StatusCode, result.Result.ConvertJsonToObject<K>());
                                    });
                            }
                            else
                            {
                                callback.Invoke(response.Result.StatusCode, default(K));
                            }
                        });
                    break;

                case SendType.POST:
                    _client.PostAsJsonAsync<T>(apiAddr, data)
                        .ContinueWith(response =>
                        {
                            Trace.WriteLine($"Post Addr : {apiAddr}\nResponse : {response.Result.StatusCode}");

                            if (response.Result.StatusCode == HttpStatusCode.OK)
                            {
                                Trace.WriteLine(response.Result.Content.ReadAsStringAsync().Result);

                                response.Result.Content.ReadFromJsonAsync<K>()
                                    .ContinueWith(result =>
                                    {
                                        callback.Invoke(response.Result.StatusCode, result.Result);
                                    });
                            }
                            else
                            {
                                callback.Invoke(response.Result.StatusCode, default(K));
                            }
                        });
                    break;

                default:
                    throw new ArgumentException("Not support senderType" + senderType);
            }
        }
    }
}
