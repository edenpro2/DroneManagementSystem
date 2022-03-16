using System.Linq;

namespace BO
{
    public static class UserVerifier
    {
        private static readonly string[] Domains =
        {
            "com", "co.il", "co.de", "org", "net", "edu", "co"
        };

        /// <summary>
        /// Checks if email is valid according to a standard 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckEmail(string email)
        {
            if (email == "")
            {
                return false;
            }

            return email.Contains('@') && Domains.ToList().Exists(email.Contains);
        }
    }
}