using System.Security.Cryptography;
using AccounteeCommon.Options;
using AccounteeService.Services.Interfaces;

namespace AccounteeService.Services
{
    public sealed class PasswordHandler : IPasswordHandler
    {
        private PwdOptions PasswordOptions { get; }

        public PasswordHandler(PwdOptions passwordOptions)
        {
            PasswordOptions = passwordOptions;
        }

        public void CreateHash(string input, out string hash, out string salt)
        {
            var saltArray = GetSalt();
            var hashArray = GetHash(input, saltArray);

            hash = Convert.ToBase64String(hashArray);
            salt = Convert.ToBase64String(saltArray);
        }

        public bool IsValid(string input, string hash, string salt)
        {
            var origHash = Convert.FromBase64String(hash);
            var origSalt = Convert.FromBase64String(salt);

            var testHash = GetHash(input, origSalt);
            var res = origHash.SequenceEqual(testHash);
            return res;
        }
    
        private byte[] GetHash(string input, byte[] salt)
        {
        
            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, PasswordOptions.Iterations, HashAlgorithmName.SHA256);
            var hashArray = pbkdf2.GetBytes(PasswordOptions.HashSize);
            return hashArray;
        }
    
        private byte[] GetSalt()
        {
            var provider = RandomNumberGenerator.Create();
            var saltArray = new byte[PasswordOptions.SaltSize];
            provider.GetBytes(saltArray);
            return saltArray;
        }
    }
}