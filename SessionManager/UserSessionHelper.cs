using Newtonsoft.Json;
using SchoolManagementDatabaseFirst.Models;

namespace SchoolManagementDatabaseFirst.SessionManager
{
    public static class UserSessionHelper
    {
        private const string SessionKey = "CurrentUser";

        public static void SetCurrentUser(HttpContext context, User currentUser)
        {
            context.Session.SetString(SessionKey, JsonConvert.SerializeObject(currentUser));
        }

        public static User GetCurrentUser(HttpContext context)
        {
            var userData = context.Session.GetString(SessionKey);
            return userData != null ? JsonConvert.DeserializeObject<User>(userData) : null;
        }

        public static void ClearCurrentUser(HttpContext context)
        {
            context.Session.Remove(SessionKey);
        }
    }

}
