using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static class HashGenerator
{
    /*
        * Generate
        * 
        * Generates a hash and salt for a given password.
        * 
        * @param password The password to generate a hash and salt for.
        * @param salt Base64 The salt used to generate the hash.
        * @param hash Base64 The hash of the password.
    */
    public static (string salt, string hash) Generate(string password)
    {
        var Key = new byte[32];
        RandomNumberGenerator.Fill(Key);
        using var crypto = new HMACSHA512() {
            Key = Key
        };
        
        return 
        (
            Convert.ToBase64String(crypto.Key), 
            Convert.ToBase64String(crypto.ComputeHash(Encoding.UTF8.GetBytes(password)))
        );
    }

    public static bool Validate(string password, string salt, string hash)
    {
        using var crypto = new HMACSHA512(Convert.FromBase64String(salt));
        var computedHash = Convert.ToBase64String(crypto.ComputeHash(Encoding.UTF8.GetBytes(password)));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != hash[i]) return false;
        }

        return true;
    }
}