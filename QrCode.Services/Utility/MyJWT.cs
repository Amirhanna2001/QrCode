using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace LimitlessCareDrPortal.Services.Utility;

public static class MyJwt
{
	public static int GetIdFromToken(string accessToken)
	{
		try
		{
			var pattern = @"(?!(Bearer))(?!\s)\b.+\b";
			var matchedToken = Regex.Match(accessToken, pattern, RegexOptions.IgnorePatternWhitespace);
			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadJwtToken(matchedToken.ToString());
			var id = int.Parse(jsonToken.Claims.First(a => a.Type == "nameid").Value, new CultureInfo("en-US"));
			return id;
		}
		catch
		{
			return -1;
		}
	}
}
