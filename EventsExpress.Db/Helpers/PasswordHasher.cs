﻿using System.Security.Cryptography;
using System.Text;

namespace EventsExpress.Db.Helpers
{
    public static class PasswordHasher
    {
        public static string GenerateHash(string password, string salt)
        {
            ASCIIEncoding asciiEnc = new ASCIIEncoding();
            byte[] byteSourceText = asciiEnc.GetBytes(salt + password);
            MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
            byte[] byteHash = md5Hash.ComputeHash(byteSourceText);

            return GetStringFromBytes(byteHash);
        }

        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            return GetStringFromBytes(salt);
        }

        private static string GetStringFromBytes(byte[] bytes)
        {
            var bld = new StringBuilder();
            for (int i = 0; i < bytes.Length; ++i)
            {
                bld.Append(bytes[i]);
            }

            return bld.ToString();
        }
    }
}
