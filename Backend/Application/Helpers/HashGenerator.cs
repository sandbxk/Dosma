using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static class HashGenerator
{
    public static void Generate(string password, out string passwordHash, out string passwordSalt)
    {
        using (var crypto = new HMACSHA512())
        {
            passwordSalt = Convert.ToBase64String(crypto.Key);
            passwordHash = Convert.ToBase64String(crypto.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }

    public static (string salt, string hash) Generate(string password)
    {
        using var crypto = new HMACSHA512();
        
        return 
        (
            Convert.ToBase64String(crypto.Key), 
            Convert.ToBase64String(crypto.ComputeHash(Encoding.UTF8.GetBytes(password)))
        );
    }

    public static bool Validate(string password, string salt, string hash)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        using var crypto = new HMACSHA512(saltBytes);
        
        byte[] computedHash = crypto.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Convert.FromBase64String(hash));
    }
}