using System;
using XboxAuthNet.Game.Jwt;
using NUnit.Framework;

namespace XboxAuthNet.Game.Test
{
    public class TestJwtDecoder
    {
        [Test]
        // encoded payload length: 95 % 4 = 3
        [TestCase("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJpc3MiLCJpYXQiOjE2NTcxOTUzNTYsImV4cCI6MTY1NzE5NjU2OCwiYXVkIjoiYXVkIiwic3ViIjoic3ViIn0._Oii5N3wxCTHNMzznLAr6hIMkDTdcHn33If0YjLd6Wk",
            "{\"iss\":\"iss\",\"iat\":1657195356,\"exp\":1657196568,\"aud\":\"aud\",\"sub\":\"sub\"}")]
        // encoded payload length: 96 % 4 = 0
        [TestCase("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJpc3MiLCJpYXQiOjE2NTcxOTUzNTYsImV4cCI6MTY1NzE5NjU2OCwiYXVkIjoiYXVkZCIsInN1YiI6InN1YiJ9._8FqWLFG1xQI8ydIiIiy9Jpilu-zsvhEkK65H3NICcY",
            "{\"iss\":\"iss\",\"iat\":1657195356,\"exp\":1657196568,\"aud\":\"audd\",\"sub\":\"sub\"}")]
        // encoded payload length: 98 % 4 = 2
        [TestCase("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJpc3MiLCJpYXQiOjE2NTcxOTUzNTYsImV4cCI6MTY1NzE5NjU2OCwiYXVkIjoiYXVkZGQiLCJzdWIiOiJzdWIifQ._Oii5N3wxCTHNMzznLAr6hIMkDTdcHn33If0YjLd6Wk",
            "{\"iss\":\"iss\",\"iat\":1657195356,\"exp\":1657196568,\"aud\":\"auddd\",\"sub\":\"sub\"}")]
        public void TestNormalJwt(string inputJwt, string expectedPayload)
        {
            var result = JwtDecoder.DecodePayloadString(inputJwt);
            Assert.AreEqual(expectedPayload, result);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestArgumentNullException(string inputJwt)
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                JwtDecoder.DecodePayloadString(inputJwt);
            });
        }

        [Test]
        [TestCase(".")]
        [TestCase("hi")]
        [TestCase("head.!@#$.tail")]
        public void TestFormatException(string inputJwt)
        {
            Assert.Throws(typeof(FormatException), () =>
            {
                JwtDecoder.DecodePayloadString(inputJwt);
            });
        }
    }
}