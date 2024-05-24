using System.Data;
using System.Data.SqlClient;

namespace temperature_analysis.DAO
{
    public class HelperDAO
    {
        /// <summary>
        /// Reads the connection string from a file and establishes a connection to the database.
        /// </summary>
        /// <returns>An open SqlConnection object.</returns>
        /// <exception cref="ApplicationException">
        /// Thrown when the connection string cannot be read from the file or the connection cannot be opened.
        /// </exception>
        private static SqlConnection GetConnection()
        {
            string filePath = "connectionString.txt";
            string connectionString;

            try
            {
                connectionString = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not read the connection string from the file.", ex);
            }

            SqlConnection connection;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Could not open a connection to the database.", ex);
            }

            return connection;
        }

        /// <summary>
        /// Executes a stored procedure without returning any result.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the stored procedure.</param>
        public static void ExecuteProcedure(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as a DataTable.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the stored procedure.</param>
        /// <returns>A DataTable containing the results of the stored procedure.</returns>
        public static DataTable ExecuteProcedureSelect(string procedureName, SqlParameter[]? parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(procedureName, connection))
                {
                    if (parameters != null)
                        adapter.SelectCommand.Parameters.AddRange(parameters);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    return table;
                }
            }
        }

        /// <summary>
        /// Executes a SQL query and returns the result as a DataTable.
        /// </summary>
        /// <param name="sqlQuery">The SQL query to execute.</param>
        /// <param name="parameters">The parameters to pass to the SQL query.</param>
        /// <returns>A DataTable containing the results of the SQL query.</returns>
        public static DataTable ExecuteSelect(string sqlQuery, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection))
                {
                    if (parameters != null)
                        adapter.SelectCommand.Parameters.AddRange(parameters);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
