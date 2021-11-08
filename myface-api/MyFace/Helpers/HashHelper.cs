using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyFace.Helpers
{
    public static class PasswordHashHelper
    {
        public struct BasicAuth
        {
            public string Username;
            public string Password;

            public BasicAuth(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }

        public static string GetSalt()
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public static string GetHashedPassword(string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static string EncodeBase64(BasicAuth user)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(user.Username + ':' + user.Password);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static BasicAuth DecodeBase64(string header)
        {
            var base64EncodedBytes = Convert.FromBase64String(header);
            var plainText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            var strings = plainText.Split(':');
            return new BasicAuth(strings[0], strings[1]);
        }
    }
}