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
                String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
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

        private void AddTask_click(object sender, EventArgs e)
        {

           try
            {
                edit(true);
                textBox1.Clear();
                textBoxDescription.Clear();
                textBox1.Text = todoList.list.list_IdColumn.DefaultValue.ToString();
                listBindingSource1.MoveLast();
                textBoxDescription.Focus();
                if(textBoxDescription.TextLength > 254)
                {
                    MessageBox.Show("Max length of task description reached!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
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
                listBindingSource1.RemoveCurrent();
                listBindingSource1.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                listTableAdapter.Fill(todoList.list);
                todoList.list.AcceptChanges();
                dataGridView1.Refresh();
                textBoxDescription.Focus();
                MessageBox.Show("Data has been successfully deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);    
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);

            }
        }

        
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

                if(!id_Check(id))
                {
                    listTableAdapter.Insert(id, input_desc, false, date);
                    edit(false);
                listBindingSource1.EndEdit();
                listTableAdapter.Fill(todoList.list);
                dataGridView1.Refresh();
                textBoxDescription.Focus();
                MessageBox.Show("Data has been successfully added.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                todoList.list.RejectChanges();
            }
        }

        
       public bool id_Check(int id)
        {
           String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
           SqlConnection con = new SqlConnection(str);
           SqlCommand cmd = new SqlCommand("Select count(*) from list where list_Id = @alias", con);
           cmd.Parameters.AddWithValue("@alias", this.textBox1.Text);
           con.Open();
           int TotalRows = todoList.Tables[0].Rows.Count;
           TotalRows = Convert.ToInt32(cmd.ExecuteScalar());
                if (TotalRows > 0)
                {
                    MessageBox.Show("Id " + textBox1.Text + " Already exist");
                    con.Close();
                    return true;
                }
            else
            {
                con.Close();
                return false;
            }   
        }

       private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataRowView drv = (DataRowView)listBindingSource1.Current;
                    TodoList.listRow row = (TodoList.listRow)drv.Row;
                    listTableAdapter.Delete(row.list_Id, row.Description, row.Completed, row.EndDate);
                    listBindingSource1.RemoveCurrent();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBoxDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBoxDescription.TextLength > 254)
            {
                MessageBox.Show("Max length of task description reached!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(textBoxDescription.TextLength < 5)
            {
                MessageBox.Show("Min length of task description should be 5 char!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
