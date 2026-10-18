using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using XboxAuthNet.Game.Serialization;

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
            var typeInfo = (JsonTypeInfo<T>)XboxGameJson.DefaultOptions.GetTypeInfo(typeof(T));
            return DecodePayload(jwt, typeInfo);
        }

        /// <summary>호출자가 제공한 생성 메타데이터로 JWT 페이로드를 복원한다.</summary>
        public static T? DecodePayload<T>(string jwt, JsonTypeInfo<T> typeInfo) where T : class
        {
            var payload = DecodePayloadString(jwt);
            return JsonSerializer.Deserialize(payload, typeInfo);
        }
    }
}
