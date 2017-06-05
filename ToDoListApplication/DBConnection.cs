using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoListApplication
{
    public class DBConnection
    {
          
        public void createDatabase()
        {
            string str;
            SqlConnection myConn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=Phoenix");

            str = "CREATE Database ToDoList" +
                "(NAME = ToDoList, " +
                "FILENAME = 'C:\\ToDoList.mdf', " +
                "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                "LOG ON (NAME = ToDoList_Log, " +
                "FILENAME = 'C:\\ToDoListLog.ldf', " +
                "SIZE = 1MB, " +
                "MAXSIZE = 5MB, " +
                "FILEGROWTH = 10%)";

            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                MessageBox.Show("DataBase is Created Successfully", "ToDoListApplication", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ////String str = "server=WIN764-KINCHIT\\SQLEXPRESS;database=TodoList;UID=omnitech;password=0mn!Cell";
                //String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                ////String str = "Data Source=(WIN764-KINCHIT\\SQLEXPRESS); " + "Database='TodoList';" + "UID=WIN764-KINCHIT\\omnitech;" + "PASSWORD=0mn!Cell;";
                //String query = "CREATE TABLE list(list_Id INT NOT NULL, " +
                //    "Description VARCHAR(255) NOT NULL, " +
                //    "Completed BIT NOT NULL DEFAULT '0', " +
                //    "PRIMARY KEY (list_Id))";
                //SqlConnection con = new SqlConnection(str);
                //SqlCommand cmd = new SqlCommand(query, con);
                //con.Open();
                //cmd.ExecuteNonQuery();
                //DataSet ds = new DataSet();
                //MessageBox.Show("connect with sql server and table created");
                //con.Close();


            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public void insertData()
        {

            try
            {
                String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                String query = "INSERT INTO list(list_id, description, completed, enddate) " +
                    "VALUES(1, 'Wakeup', '0', GetDate())";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                DataSet ds = new DataSet();
                MessageBox.Show("connect with sql server and table created");
                con.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        public void deleteData()
        {
            //String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
            //String query = "DELETE FROM list" +
            //    " WHERE list_Id = '1';";
            //SqlConnection con = new SqlConnection(str);
            //SqlCommand cmd = new SqlCommand(query, con);
            //con.Open();
            //cmd.ExecuteNonQuery(
            //DataSet ds = new DataSet();

            //String q1 = "ALTER TABLE list" +
            //    " ADD EndDate DATETIME NOT NULL DEFAULT (GETDATE());";
            //MessageBox.Show("connect with sql server and table created");
            //SqlConnection con = new SqlConnection(str);
            //SqlCommand cmd = new SqlCommand(q1, con);
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();
        }

        public void saveData()
        {
            //String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
            //String query = "INSERT INTO list(list_id, description, completed) " +
            //    "VALUES(id, des, '0')";
            //SqlConnection con = new SqlConnection(str);
            //SqlCommand cmd = new SqlCommand(query, con);
            //con.Open();
            //cmd.ExecuteNonQuery();
            //DataSet ds = new DataSet();
            //MessageBox.Show("connect with sql server and table created");
            //con.Close();
        }

    }
}
