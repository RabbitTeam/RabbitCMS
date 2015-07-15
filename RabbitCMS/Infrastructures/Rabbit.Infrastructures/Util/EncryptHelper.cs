using System;
using System.Text;

namespace Rabbit.Infrastructures.Util
{
    public sealed class EncryptHelper
    {
        public static string Encrypt(string content)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        }
    }
}