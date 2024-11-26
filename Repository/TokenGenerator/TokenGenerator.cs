using EMS_Project.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_Project.Repository.TokenGenerator
{
    public class TokenGenerator
    {
        public string CreateToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12Ei5SePg1urP_9WSagKaAqorU3f6-cf1AT1rLp81luzdn88Px6aR7WYoxYWH1EUePWGQxsm1r78mgbYMgoPwpVMTh62Y8Uaf7g_ucXi_s3vKFfS1paVQlz39HtFptFAGNiMoP7CXd6_pTTk36IkZMbjWkrxzPIhsTmJ6koqp4k"));

            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                "http://localhost:5278",
                "http://localhost:5278",
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
