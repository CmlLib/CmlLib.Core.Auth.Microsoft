using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft
{
    public static class Extensions
    {
        public static JavaEditionLoginHandlerBuilder ForJavaEdition(this LoginHandlerBuilder builder)
        {
            var cid = builder.ClientId;
            if (cid == null || string.IsNullOrEmpty(cid))
                cid = JavaEditionLoginHandlerBuilder.DefaultClientId;

            return new JavaEditionLoginHandlerBuilder(cid, builder.HttpClient);
        }
    }
}
