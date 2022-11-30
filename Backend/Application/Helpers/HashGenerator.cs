using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static class HashGenerator
{
    /// <summary>
    ///     Generates a hash and salt for a given password.
    /// </summary>
    /// <param name="password">The password to generate a hash and salt for.</param>
    /// <returns>The generated salt and hash as Base64 strings.</returns>
    /// <remarks>
    ///     This method is used by
    ///     <list>
    ///         <item><see cref="ObjectGenerator.GenerateUser"/></item>
    ///     </list>
    /// </remarks>
    /// <completionlist cref="(HashGenerator, ObjectGenerator)"/>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
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

    /// <summary>
    ///     Generates a hash for a given password and salt.
    /// </summary>
    /// <param name="password">The password to generate a hash for. (the requested password to test)</param>
    /// <param name="salt">The salt to use when generating the hash. (saved for the spesific user)</param>
    /// <param name="hash">the stored hash (saved for a spesific user)</param>
    /// <returns>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>True</term>
    ///             <description>
    ///                The generated hash from the <see cref="password"/> 
    ///                and <see cref="salt"/> is equal to the stored <see cref="hash"/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>False</term>
    ///             <description>The comparison failed</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <remarks>
    ///     This method is used by
    ///     <list>
    ///         <item><see cref="AuthenticationService.Login"/></item>
    ///     </list>
    /// </remarks>
    /// <completionlist cref="(HashGenerator, AuthenticationService)"/>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public static bool Validate(string password, string salt, string hash)
    {
        using var crypto = new HMACSHA512(Convert.FromBase64String(salt));
        var computedHash = Convert.ToBase64String(crypto.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return computedHash.SequenceEqual(hash);
    }
}
