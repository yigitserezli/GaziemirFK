using System;
using System.Security.Cryptography;
using System.Text;

namespace Gfk.Mvc.Helpers
{
	public class HashHelper
	{
        public static byte[] HashToBytes(string text)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        public static string HashToString(string text)
        {
            var hashedBytes = HashToBytes(text);
            var sb = new StringBuilder();
            foreach (var b in hashedBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}

