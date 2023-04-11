using System.Security.Claims;
using System.Text.Json;

namespace TeslaRent_Client.Helpers
{
    // 221. В проект Client в папку Helpers добавить класс JwtParser
    public static class JwtParser
    {
        /// <summary>
        /// Parses the claims from a JWT string.
        /// </summary>
        /// <param name="jwt">The JWT string.</param>
        /// <returns>A collection of claims.</returns>
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString())));
            return claims;
        }

        /// <summary>
        /// Parses a Base64 string without padding into a byte array.
        /// </summary>
        /// <param name="base64">The Base64 string to parse.</param>
        /// <returns>A byte array containing the parsed Base64 string.</returns>
        static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
