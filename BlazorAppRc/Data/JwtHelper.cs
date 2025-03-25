using System.IdentityModel.Tokens.Jwt;
namespace BlazorAppRc.Data
{
    public static class JwtHelper
    {
        public static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim == null)
                return true;

            var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));
            return exp < DateTimeOffset.UtcNow;
        }
    }
}
