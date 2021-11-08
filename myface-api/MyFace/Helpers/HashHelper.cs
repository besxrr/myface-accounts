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

        public static string BasicAuthEncode(BasicAuth user)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(user.Username + ':' + user.Password);
            var converted =  Convert.ToBase64String(plainTextBytes);
            return "Authorization: Basic " + converted;
        }

        public static BasicAuth DecodeBase64(string header)
        {
            //TODO What if user has a ':' in their name or password...
            var trimmedHeader = header.Remove(0, 21);
            var base64EncodedBytes = Convert.FromBase64String(trimmedHeader);
            var plainText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            //TODO Must be a better way of doing this
            var strings = plainText.Split(':');
            return new BasicAuth(strings[0], strings[1]);
        }
    }
}