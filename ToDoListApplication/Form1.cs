using Serilog;
using System;
using System.Data;

using System.Windows.Forms;

namespace ToDoListApplication
{
    public partial class Form1 : Form
    {
        AddingNewTask adt = new AddingNewTask();
        UpdatingTask ut = new UpdatingTask();

        public Form1()
        {
            InitializeComponent();
            //Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Logger = new LoggerConfiguration().WriteTo.RollingFile("todolist-{Date}.txt").CreateLogger();    
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    createTable();
        //}

        public void createTable()
        {
            //try
            //{
            //    String str = "Data Source=Phoenix;Initial Catalog=TodoList;Integrated Security=True";
            //    String query = "CREATE TABLE list(list_Id INT NOT NULL, " +
            //        "Description VARCHAR(255) NOT NULL, " +
            //        "Completed BIT NOT NULL DEFAULT '0', " +
            //        "PRIMARY KEY (list_Id))";
            //    SqlConnection con = new SqlConnection(str);
            //    SqlCommand cmd = new SqlCommand(query, con);
            //    con.Open();
            //    cmd.ExecuteNonQuery();
            //    DataSet ds = new DataSet();
            //    MessageBox.Show("connect with sql server and table created");
            //    con.Close();
            //}
            //catch (Exception es)
            //{
            //    MessageBox.Show(es.Message);

            //}
        }

        // Add Task button when clicked ID and Description fields are blank and enabled for adding a task.
        // Date filed is populated with current data/time.
        public void AddTask_click(object sender, EventArgs e)
        {

            try
            {
                Log.Information("Add Task clicked.");
                edit(true);
                textBox1.Clear();
                textBoxDescription.Clear();
                dateTimePicker1.Value = DateTime.Now;
                textBox1.Text = todoList.list.list_IdColumn.DefaultValue.ToString();
                //listBindingSource1.MoveLast();
                textBoxDescription.Focus();
                Log.Information("Add Task clicked - Id and Description field enabled for adding tasks.");
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Information("Error in Add task - {0}", es.Message);

            }


        }

        // Default method for form load.
        private void Form1_Load(object sender, EventArgs e)
        {
            this.listTableAdapter.Fill(this.todoList.list);
            edit(false);
            Log.Information("Form Loaded to add, review, update and delete tasks.");
        }

        // Save Update Task button click
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Enabled = false;
                string input_id = textBox1.Text;
                int id = int.Parse(input_id);
                Log.Information("List Id not updated.");

                string input_desc = textBoxDescription.Text;
                Log.Information("Task Description updated/verified");
                DateTime date1 = dateTimePicker1.Value;
                Log.Information("Task Date updated/verified");


                TodoList.listRow rows = todoList.list.FindBylist_Id(id);
                rows.Completed = bool.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                Log.Information("Completed Check box checked/unchecked. - Unchecked by default");

                //Update task method called in UpdateTask.cs
                ut.updateData(id, input_desc, rows.Completed, date1);

                //this.listTableAdapter.Update(this.todoList.list);
                Log.Information("Task inserted in DB");
                edit(false);
                listBindingSource1.EndEdit();
                listTableAdapter.Fill(todoList.list);
                dataGridView1.Refresh();
                Log.Information("Task seen in UI");
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                Log.Error("Error in Delete task - {0}", es.Message);
            }
        }

        // Input field edit value - true/false
        private void edit(bool value)
        {
            textBox1.Enabled = value;
            textBoxDescription.Enabled = value;
            dateTimePicker1.Enabled = value;
            Log.Information("Field enabled for editing.");
        }

        // Update Task button click
        private void button4_Click(object sender, EventArgs e)
        {
            edit(true);
            textBoxDescription.Focus();
            string input_desc = textBoxDescription.Text;
            DateTime date = dateTimePicker1.Value;
            Log.Information("List Description and Date Updated");
        }

        //Click Cancel button
        private void button5_Click(object sender, EventArgs e)
        {
            edit(false);
            int index = dataGridView1.CurrentRow.Index; // Get row index
            DataGridViewRow selectedRow = dataGridView1.Rows[index];
            textBox1.Text = selectedRow.Cells[0].Value.ToString();
            textBoxDescription.Text = selectedRow.Cells[1].Value.ToString();
            listBindingSource1.ResetBindings(false);
        }

        // Save New Task
        public void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string input_id = textBox1.Text;
                int id = int.Parse(input_id);
                Log.Information("List Id input done");

                string input_desc = textBoxDescription.Text;
                DateTime date = dateTimePicker1.Value;
                Log.Information("List Description and Date entered");

                // Adding a method
                if(adt.AddTask(id, input_desc, date))
                {
                    edit(false);
                    listBindingSource1.EndEdit();
                    listTableAdapter.Fill(todoList.list);
                    dataGridView1.Refresh();
                    textBoxDescription.Focus();
                    Log.Information("Task added and seen in UI");
                    Log.Information("*************************");
                    MessageBox.Show("Task has been successfully added.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    edit(false);
                    listBindingSource1.EndEdit();
                    listTableAdapter.Fill(todoList.list);
                    dataGridView1.Refresh();
                    textBoxDescription.Focus();
                    Log.Information("Task not added and not seen in UI");
                    Log.Information("*************************");
                    MessageBox.Show("Task not added as ID already exisiting. ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                todoList.list.RejectChanges();
                Log.Warning("Error saving task - Changes rejected.");
            }
        }
        
       //key down event to delete the selected row. 
       private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Delete the selected record
            if(e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Log.Information("Current selected item deleted.");
                    DataRowView drv = (DataRowView)listBindingSource1.Current;
                    TodoList.listRow row = (TodoList.listRow)drv.Row;
                    listTableAdapter.Delete(row.list_Id, row.Description, row.Completed, row.EndDate);
                    listBindingSource1.RemoveCurrent();
                    Log.Information("Current selected deleted from db and view updated.");
                }
            }
        }

        // Key Press event for task Id
        // Validating key character input for characters and special characters
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

        // Key Press event for description
        // Validating the length of the description to be less the 255 characters
        private void textBoxDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBoxDescription.TextLength > 254)
            {
                MessageBox.Show("Max length of task description reached!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(textBoxDescription.TextLength < 0)
            {
                MessageBox.Show("Min length of task description should be 5 char!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Form close 
        private void Form1_Leave(object sender, EventArgs e)
        {
            Log.CloseAndFlush();
        }

        //Selecting Cells in DataGrid
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex; // Get row index
            DataGridViewRow selectedRow = dataGridView1.Rows[index];
            textBox1.Text = selectedRow.Cells[0].Value.ToString();
            textBoxDescription.Text = selectedRow.Cells[1].Value.ToString();
        }
    }
}
