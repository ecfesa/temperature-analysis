using Microsoft.AspNetCore.Mvc;

namespace temperature_analysis.Controllers
{
    public static class HelperController
    {
        public static bool LoginSessionVerification(ISession session)
        {
            if (session.GetString("UserLogin") == null)
                return false;
            else
                return true;
        }

        public static bool EmployeeSesstionVerification(ISession session)
        {
            if (session.GetString("IsEmployee") == null)
                return false;
            else
                return true;
        }

        public static int? ActualUserID(ISession session)
        {
            return session.GetInt32("ID");
        }

    }
}
