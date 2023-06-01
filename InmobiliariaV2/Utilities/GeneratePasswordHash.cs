using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace InmobiliariaV2.Utilities
{
    public class GeneratePasswordHash
    {
        public static string GenerateHash(string password, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
