using CmlLib.Core.Auth.Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Bedrock.Auth
{
    public static class Extensions
    {
        public static BedrockLoginHandlerBuilder ForBedrockEdition(this LoginHandlerBuilder builder)
        {
            if (string.IsNullOrEmpty(builder.ClientId))
                throw new InvalidOperationException("ClientId was empty");

            return new BedrockLoginHandlerBuilder(builder.ClientId, builder.HttpClient);
        }
    }
}
