using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoListApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createTable();
        }

        public void createTable()
        {
           try
            {
                //String str = "server=WIN764-KINCHIT\\SQLEXPRESS;database=TodoList;UID=omnitech;password=0mn!Cell";
                String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                //String str = "Data Source=(WIN764-KINCHIT\\SQLEXPRESS); " + "Database='TodoList';" + "UID=WIN764-KINCHIT\\omnitech;" + "PASSWORD=0mn!Cell;";
                String query = "CREATE TABLE list(list_Id INT NOT NULL, " +
                    "Description VARCHAR(255) NOT NULL, " +
                    "Completed BIT NOT NULL DEFAULT '0', " +
                    "PRIMARY KEY (list_Id))";
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
                MessageBox.Show(es.Message);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

           try
            {
                int temp = 1;
                edit(true);
                textBox1.Clear();
                textBoxDescription.Clear();
                textBox1.Text = todoList.list.list_IdColumn.DefaultValue.ToString();
                //todoList.list.AddlistRow(todoList.list.NewlistRow());
                //todoList.list.AddlistRow(todoList.list.NewlistRow());
                listBindingSource1.MoveLast();
                textBoxDescription.Focus();


                //String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                //String query = "INSERT INTO list(list_id, description, completed, enddate) " +
                //    "VALUES(1, 'Wakeup', '0', GetDate())";
                //SqlConnection con = new SqlConnection(str);
                //SqlCommand cmd = new SqlCommand(query, con);
                //con.Open();
                //cmd.ExecuteNonQuery();
                //DataSet ds = new DataSet();
                //MessageBox.Show("connect with sql server and table created");
                //con.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.listTableAdapter.Fill(this.todoList.list);
            edit(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
                String query = "DELETE FROM list" +
                    " WHERE list_Id = '1';";
                SqlConnection con = new SqlConnection(str);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                DataSet ds = new DataSet();
                //String q1 = "ALTER TABLE list" +
                //    " ADD EndDate DATETIME NOT NULL DEFAULT (GETDATE());";
                //MessageBox.Show("connect with sql server and table created");
                //SqlConnection con = new SqlConnection(str);
                //SqlCommand cmd = new SqlCommand(q1, con);
                //con.Open();
                //cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);

            }
        }

        //ALTER TABLE yourTable 
        //ADD COLUMN new_date DATETIME NOT NULL DEFAULT 20110126143000 AFTER preceding_col

        private void edit(bool value)
        {
            textBox1.Enabled = value;
            textBoxDescription.Enabled = value;
            dateTimePicker1.Enabled = value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            edit(true);
            textBoxDescription.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            edit(false);
            listBindingSource1.ResetBindings(false);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // validation

            // insert

            // display

            try
            {
                string input_id = textBox1.Text;
                int id = int.Parse(input_id);

                string input_desc = textBoxDescription.Text;
                DateTime date = dateTimePicker1.Value;
                listTableAdapter.Insert(id, input_desc, false, date);

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


                edit(false);
                listBindingSource1.EndEdit();
                //listTableAdapter.Update(todoList.list);
                listTableAdapter.Fill(todoList.list);
                dataGridView1.Refresh();
                textBoxDescription.Focus();
                MessageBox.Show("Data has been successfully added.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                todoList.list.RejectChanges();
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13) //Enter
            {
                if(MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    listBindingSource1.RemoveCurrent();
                }
            }
        }
    }
}
