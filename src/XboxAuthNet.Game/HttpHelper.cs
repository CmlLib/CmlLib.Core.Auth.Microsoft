using System;
using System.Net.Http;

namespace XboxAuthNet.Game
{
    internal class HttpHelper
    {
        internal static Lazy<HttpClient> DefaultHttpClient
            = new Lazy<HttpClient>(() => new HttpClient());
    }
}
