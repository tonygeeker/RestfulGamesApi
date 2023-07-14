using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;
using System.Security.Cryptography;
using System.Text;

namespace RestfulGamesApi.BusinessServiceLayer
{
    public static class SecurityUtility
    {
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static bool ValidateUsernameAvailability(string emailAddress, string userName, IService<User> userService)
        {
            if (userName == null)
            {
                return false;
            }
            var dbPlayer = userService.GetQueryable(x => x.Username.ToLower() == userName.ToLower() && emailAddress.ToLower() != x.Email.ToLower()).Count();

            return dbPlayer <= 0;
        }
    }
}
