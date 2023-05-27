using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RestaurantHomework.Authorization.Bll.Utils;

public static class PasswordHasher
{
    private static readonly byte[] Salt = Enumerable.Range(0, 16).Select(x => (byte)x).ToArray();

    public static string HashPassword(string password)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-7.0
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return hashed;
    }
}