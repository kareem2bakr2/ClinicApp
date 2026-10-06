using System.Security.Cryptography;

namespace ClinicApp.Comman
{
    public static class CommanExtensions
    {
        public static string FRONTENDURL { get { return "https://localhost:7214"; } }

        public static string GeneratePassword(int Length = 8) 
        {
            const string chars =
               "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
               "abcdefghijklmnopqrstuvwxyz" +
               "0123456789" +
               "!@#$%^&*";

            return string.Create(Length, chars, (result, chars) 
                => {
                    for (int i = 0; i < result.Length; i++) {
                        result[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];    
                    }
                });
        }
    
    
    }
}
