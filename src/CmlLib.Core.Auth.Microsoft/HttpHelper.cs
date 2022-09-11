using System;
using System.Net.Http;

namespace CmlLib.Core.Auth.Microsoft
{
    internal class HttpHelper
    {
        internal static Lazy<HttpClient> DefaultHttpClient
            = new Lazy<HttpClient>(() => new HttpClient());
    }
}
