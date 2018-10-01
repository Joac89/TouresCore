using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TouresAuthenticate.Model;

namespace TouresAuthenticate.Service
{
	public class JwtService
	{
		public JwtResponse SetJWT(string userName, JwtModel jwtParams)
		{
			var exportToken = "";
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
			var token = new JwtSecurityToken
			(
				issuer: jwtParams.Issuer,
				audience: jwtParams.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(double.Parse(jwtParams.Expire)),
				//expires: DateTime.UtcNow.AddMinutes(double.Parse("2")),
				notBefore: DateTime.UtcNow,
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.SigningKey)), SecurityAlgorithms.HmacSha256)
			);
			exportToken = new JwtSecurityTokenHandler().WriteToken(token);

			return new JwtResponse()
			{
				Status = exportToken != "" ? true : false,
				Token = exportToken != "" ? exportToken : ""
			};
		}
	}
}
