using System;
using Serilog;
using System.Data.SqlClient;

namespace ToDoListApplication
{
    public class UpdatingTask
    {
        public void updateData(int id, string desc, bool completed, DateTime date)
        {
            try
            {
                String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                String query = "UPDATE list SET Description = @alias, EndDate = @alias2, Completed = @alias3 where list_Id = @alias1 ";
                Log.Information("connect with sql server.");
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@alias1", id);
                cmd.Parameters.AddWithValue("@alias", desc);
                cmd.Parameters.AddWithValue("@alias2", date);
                cmd.Parameters.AddWithValue("@alias3", completed);
                Log.Information("Executing update query");
                con.Open();
                cmd.ExecuteNonQuery();
                Log.Information("Task updated in to the database.");
                con.Close();
            }
            catch(Exception es)
            {
                Log.Error(es.Message);
            }
        }
    }
}
