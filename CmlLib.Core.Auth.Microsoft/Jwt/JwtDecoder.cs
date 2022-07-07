using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft.Jwt
{
    public class JwtDecoder
    {
        public static string DecodePayloadString(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
                throw new ArgumentNullException(jwt);

            var spl = jwt.Split('.');
            if (spl.Length != 3)
                throw new ArgumentException("invalid jwt");

            var encodedPayload = spl[1];
            switch (encodedPayload.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    encodedPayload += "==";
                    break;
                case 3:
                    encodedPayload += "=";
                    break;
                default:
                    throw new ArgumentException("invalid jwt payload");

            }

            var decodedPayload = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPayload));
            return decodedPayload;
        }

        public static T DecodePayload<T>(string jwt)
        {
            var payload = DecodePayloadString(jwt);
            return JsonSerializer.Deserialize<T>(payload);
        }
    }
}
