using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FitFoodAPI;

public class AuthOptions
{
    public const string ISSUER = "FF_API_1488"; // издатель токена
    public const string AUDIENCE = "FF_CLIENT_APP"; // потребитель токена
    const string KEY = "bXlzdXBlcnNlY3JldF9zZWNyZXRzZWNyZXRzZWNyZXRrZXkhMTIz";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    
    public static byte[] GenerateSymmetricKey()
    {
        var rng = RandomNumberGenerator.GetInt32(int.MaxValue);
        var key = BitConverter.GetBytes(rng);
        Console.WriteLine(BitConverter.ToString(key));
        return key;
    }
}