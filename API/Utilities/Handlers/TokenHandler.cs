using API.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Utilities.Handlers;
public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;
    
    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /*
     * Method : Generate
     * <summary>Digunakan untuk generate token serta mendefinisikan strukturnya</summary>
     */
    public string Generate(IEnumerable<Claim> claims)
    {

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTService:SecretKey"]));
        
        //Deklarasi variabel signingCredential yang digunakan untuk menyimpan enkripsi secretKey dengan algoritma HmacSha256
        var signingCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        //konfigurasi bentuk atau struktur token
        var tokenOptions = new JwtSecurityToken(issuer: _configuration["JWTService:Issuer"],
                                                audience: _configuration["JWTService:Audience"],
                                                claims: claims,
                                                expires: DateTime.Now.AddMinutes(5), 
                                                signingCredentials: signingCredential);
        //Enkripsi token
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return encodedToken;
    }
}
