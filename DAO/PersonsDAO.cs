using System.Data.SqlClient;
using System.Data;
using temperature_analysis.Models;
using System;

namespace temperature_analysis.DAO
{
    public class PersonsDAO : StandardDAO<PersonViewModel>
    {
        protected override string TableName()
        {
            return "Persons";
        }

        protected override SqlParameter[] CreateParameters(PersonViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@FirstName", model.FirstName          ?? (object) DBNull.Value);
            parameters[1] = new SqlParameter("@LastName", model.LastName            ?? (object) DBNull.Value);
            parameters[2] = new SqlParameter("@Email", model.Email                  ?? (object) DBNull.Value);
            parameters[3] = new SqlParameter("@Username", model.Username            ?? (object) DBNull.Value);
            parameters[4] = new SqlParameter("@PasswordHash", model.PasswordHash    ?? (object) DBNull.Value);
            parameters[5] = new SqlParameter("@PhoneNumber", model.PhoneNumber      ?? (object) DBNull.Value);
            parameters[6] = new SqlParameter("@ThemeId", model.ThemeId.ToString()   ?? (object) DBNull.Value);
            return parameters;
        }

        public SqlParameter[] CreateParametersForUpdate(PersonViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@PersonID", model.Id);
            parameters[1] = new SqlParameter("@FirstName", model.FirstName);
            parameters[2] = new SqlParameter("@LastName", model.LastName);
            parameters[3] = new SqlParameter("@Email", model.Email);
            parameters[4] = new SqlParameter("@Username", model.Username);
            parameters[5] = new SqlParameter("@PasswordHash", model.PasswordHash);
            parameters[6] = new SqlParameter("@PhoneNumber", model.PhoneNumber ?? (object)DBNull.Value);
            parameters[7] = new SqlParameter("@ThemeId", model.ThemeId.ToString()   ?? (object) DBNull.Value);
            return parameters;
        }

        public override void Update(PersonViewModel model)
        {
            HelperDAO.ExecuteProcedure("spUpdate_" + TableName(), CreateParametersForUpdate(model));
        }

        protected override PersonViewModel MountModel(DataRow row)
        {
            PersonViewModel person = new PersonViewModel();

            person.Id = (int)row["PersonID"];
            person.FirstName = row["FirstName"].ToString();
            person.LastName = row["LastName"].ToString();
            person.Username = row["Username"].ToString();
            person.PasswordHash = "";
            person.Email = row["Email"].ToString();
            person.PhoneNumber = row["PhoneNumber"] != DBNull.Value ? row["PhoneNumber"].ToString() : null;
            person.ThemeId = Convert.ToInt32(row["ThemeId"]);


            if (row.Table.Columns.Contains("ThemeDescription"))
                person.ThemeDescription = row["ThemeDescription"].ToString();

            if (row.Table.Columns.Contains("PrimaryHex"))
                person.ThemeHex = row["PrimaryHex"].ToString();

            return person;
        }

        public bool LoginExists(string username, string password)
        {

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Username", username);
            parameters[1] = new SqlParameter("@PasswordHash", password);

            string sql = "SELECT * FROM " + TableName() + " WHERE Username = @Username AND PasswordHash = @PasswordHash";

            return HelperDAO.ExecuteSelect(sql, parameters).Rows.Count >= 1;
        }

        public int LoginExists(string username, string password, bool return_ID)
        {

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Username", username);
            parameters[1] = new SqlParameter("@PasswordHash", password);

            string sql = "SELECT * FROM " + TableName() + " WHERE Username = @Username AND PasswordHash = @PasswordHash";

            return (int)HelperDAO.ExecuteSelect(sql, parameters).Rows[0]["PersonID"];
        }

        public List<PersonViewModel> GetSearchPersons(string query)
        {

            List<PersonViewModel> persons = new List<PersonViewModel>();

            string sql = "SELECT * FROM Persons WHERE CONCAT(FirstName, " + "\' \'" + ", LastName) LIKE @Query";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Query", SqlDbType.NVarChar) { Value = '%' + query + '%' };

            try
            {
                foreach (DataRow row in HelperDAO.ExecuteSelect(sql, parameters).Rows)
                {
                    persons.Add(MountModel(row));
                }
            }
            catch
            {
                persons.Clear();
            }

            return persons;

        }

        public PersonViewModel? Get(int? id)
        {
            var p = new SqlParameter[]{
                new SqlParameter("PersonId", id),
            };

            var table = HelperDAO.ExecuteProcedureSelect("spGet_" + TableName(), p);

            if (table.Rows.Count != 0)
                return MountModel(table.Rows[0]);
            else
                return null;
        }
    }
}
