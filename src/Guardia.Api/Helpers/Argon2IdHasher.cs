using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Guardia.Api.Helpers;
public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    private const int GradoDeParalelismo = 8;
    private const int Iteraciones = 4;    
    private const int MemoriaACubrir = 12288;

    public string HashPassword(TUser user, string password)
    {
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = GradoDeParalelismo,
            Iterations = Iteraciones,
            MemorySize = MemoriaACubrir
        };

        var hashBytes = argon2.GetBytes(32);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hashBytes)}";
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        try
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return PasswordVerificationResult.Failed;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(providedPassword))
            {
                Salt = salt,
                DegreeOfParallelism = GradoDeParalelismo,
                Iterations = Iteraciones,
                MemorySize = MemoriaACubrir
            };

            var newHashBytes = argon2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(hash, newHashBytes) ?
                PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
        catch
        {
            return PasswordVerificationResult.Failed;
        }
    }
}
