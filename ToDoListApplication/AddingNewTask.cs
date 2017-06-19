using System;
using System.Data;
using System.Data.SqlClient;
using Serilog;

namespace ToDoListApplication
{
    public class AddingNewTask
    {
        int id;
        string desc;

        public int number
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }


        public string des
        {
            get
            {
                return this.desc;
            }
            set
            {
                this.desc = value;
            }
        }
 
        public bool AddTask(int id, string desc, DateTime date)
        {
            bool added = false;
            try
            {
                if (!id_Check(id))
                {
                    String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                    String query = "INSERT INTO list(list_id, description, completed, enddate) " +
                        "VALUES(@alias, @alias1, '0', @alias2)";
                    Log.Information("connect with sql server.");
                    SqlConnection con = new SqlConnection(str);
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@alias", id);
                    cmd.Parameters.AddWithValue("@alias1", desc);
                    cmd.Parameters.AddWithValue("@alias2", date);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    Log.Information("Task inserted in to the database.");
                    con.Close();
                    added = true;
                }
                return added;
            }
            catch (Exception es)
            {
                Log.Error(es.Message);
                return added;

            }


        }

        public bool id_Check(int id)
        {
            String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(str))
            {
                //SqlDataAdapter adapter = new SqlDataAdapter();
                //DataSet ds = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand("Select count(*) from list where list_Id = @alias", con);
                cmd.Parameters.AddWithValue("@alias", id);

                //int TotalRows = todoList.Tables[0].Rows.Count;

                int TotalRows = Convert.ToInt32(cmd.ExecuteScalar());
                if (TotalRows > 0)
                {
                    Log.Information("Id " + id + " Already exist");
                    //con.Close();
                    return true;
                }
                else
                {
                    //con.Close();
                    return false;
                }
            }
        }
    }

}
