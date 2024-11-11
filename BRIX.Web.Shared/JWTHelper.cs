using System.Security.Claims;
using System.Text.Json;

namespace BRIX.Web.Shared
{
    public class JWTHelper
    {
        public static List<Claim> ParseClaimsFromJwt(string jwt)
        {
            List<Claim> claims = [];
            string payload = jwt.Split('.')[1];
            byte[] jsonBytes = ParseBase64WithoutPadding(payload);
            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null)
            {
                return [];
            }

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles);

            if (roles != null)
            {
                string rolesString = roles.ToString() ?? string.Empty;

                if (rolesString.Trim().StartsWith('['))
                {
                    string[] parsedRoles = JsonSerializer.Deserialize<string[]>(rolesString) ?? [];

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, rolesString));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? "")));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
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
