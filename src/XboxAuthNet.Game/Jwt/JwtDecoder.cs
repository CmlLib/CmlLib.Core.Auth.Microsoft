using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace XboxAuthNet.Game.Jwt
{
    public static class JwtDecoder
    {
        /// <summary>
        /// decode jwt payload
        /// </summary>
        /// <param name="jwt">entire jwt</param>
        /// <returns>decoded jwt payload</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException">invalid jwt</exception>
        public static string DecodePayloadString(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
                throw new ArgumentNullException(jwt);

            var spl = jwt.Split('.');
            if (spl.Length != 3)
                throw new FormatException("invalid jwt");

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
                    throw new FormatException("jwt payload");

            }

            var decodedPayload = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPayload)); // encodedPayload can't be null since string.Split never return null element
            return decodedPayload;
        }

        /// <summary>
        /// decode jwt payload and deserialize
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns>deserialized object of jwt payload</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException">invalid jwt</exception>
        public static T? DecodePayload<T>(string jwt) where T : class
        {
            var payload = DecodePayloadString(jwt);
            return JsonSerializer.Deserialize<T>(payload);
        }
    }
}
