using System;

namespace Capstone.Security
{
    public class PasswordGenerator
    {
        public string oneTimeGenerator()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            char[] randomPass = new char[8];
            Random random = new Random();
            for (int i = 0; i < randomPass.Length; i++)
            {
                randomPass[i] = chars[random.Next(chars.Length)];
            }
            return new String(randomPass);
        }
    }
}
