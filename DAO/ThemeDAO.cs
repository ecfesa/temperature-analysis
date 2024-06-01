using System.Data.SqlClient;
using System.Data;
using temperature_analysis.Models;

namespace temperature_analysis.DAO
{
    public class ThemeDAO : StandardDAO<ThemeViewModel>
    {

        protected override string TableName()
        {
            return "Themes";
        }

        protected override ThemeViewModel MountModel(DataRow row)
        {
            ThemeViewModel theme = new ThemeViewModel();

            theme.Id = (int)row["ID"];
            theme.Description = row["Description"].ToString();
            theme.PrimaryHex = row["PrimaryHex"].ToString();

            return theme;
        }

        protected override SqlParameter[] CreateParameters(ThemeViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
