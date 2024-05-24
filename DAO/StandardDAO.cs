using System.Data.SqlClient;
using System.Data;
using temperature_analysis.Models;

namespace temperature_analysis.DAO
{
    public abstract class StandardDAO<T> where T : StandardViewModel
    {
        /// <summary>
        /// Creates the SQL parameters from the given model.
        /// </summary>
        /// <param name="model">The model to create parameters from.</param>
        /// <returns>An array of SQL parameters.</returns>
        protected abstract SqlParameter[] CreateParameters(T model);

        /// <summary>
        /// Constructs the model from a DataRow.
        /// </summary>
        /// <param name="row">The DataRow to construct the model from.</param>
        /// <returns>The constructed model.</returns>
        protected abstract T MountModel(DataRow row);

        /// <summary>
        /// Gets the name of the table associated with the model.
        /// </summary>
        /// <returns>The table name.</returns>
        protected abstract string TableName();

        /// <summary>
        /// Inserts the model into the database.
        /// </summary>
        /// <param name="model">The model to insert.</param>
        public virtual void Insert(T model)
        {
            HelperDAO.ExecuteProcedure("spInsert_" + TableName(), CreateParameters(model));
        }

        /// <summary>
        /// Updates the model in the database.
        /// </summary>
        /// <param name="model">The model to update.</param>
        public virtual void Update(T model)
        {
            HelperDAO.ExecuteProcedure("spUpdate_" + TableName(), CreateParameters(model));
        }

        /// <summary>
        /// Deletes a model from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the model to delete.</param>
        /// <param name="idRoute">The parameter name for the ID in the stored procedure.</param>
        public virtual void Delete(int id, string idRoute)
        {
            //TODO - Improve this delete method
            var parameters = new SqlParameter[1] {
                new SqlParameter("@" + idRoute, id),
            };

            HelperDAO.ExecuteProcedure("spDelete_" + TableName(), parameters);
        }

        /// <summary>
        /// Retrieves a model from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the model to retrieve.</param>
        /// <returns>The retrieved model, or null if not found.</returns>
        public virtual T? Get(int id)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("id", id),
            };

            var table = HelperDAO.ExecuteProcedureSelect("spGet_" + TableName(), parameters);

            if (table.Rows.Count != 0)
                return MountModel(table.Rows[0]);
            else
                return null;
        }

        /// <summary>
        /// Retrieves all models from the database.
        /// </summary>
        /// <returns>A list of all retrieved models.</returns>
        public virtual List<T> GetAll()
        {
            var table = HelperDAO.ExecuteProcedureSelect("spGetAll_" + TableName(), null);
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
                list.Add(MountModel(row));

            return list;
        }
    }
}
