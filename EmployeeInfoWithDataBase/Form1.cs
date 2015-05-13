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

namespace EmployeeInfoWithDataBase
{
    public partial class employeeInfoForm : Form
    {
        public employeeInfoForm()
        {
            InitializeComponent();
        }

        private Employee anEmployee = new Employee();
        private int rowAffected;
        private List<Employee> myList = new List<Employee>();
        private bool isUpdateMode = false;
        private int employeeId;

        private void saveButton_Click(object sender, EventArgs e)
        {
            anEmployee.name = nameTextBox.Text;
            anEmployee.address = addressTextBox.Text;
            anEmployee.email = emailTextBox.Text;
            anEmployee.salary = Convert.ToDouble(salaryTextBox.Text);
            if (IfEmailExist(anEmployee.email))
            {
                MessageBox.Show("Email already Exist!");
                return;
            }
            string connectionString = @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";

            SqlConnection connection = new SqlConnection(connectionString);
            String query = "INSERT INTO employeeTB VALUES ('" + anEmployee.name + "','" + anEmployee.address + "','" +
                           anEmployee.email + "','" + anEmployee.salary + "');";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            rowAffected = cmd.ExecuteNonQuery();
            connection.Close();
            if (rowAffected > 0)
            {
                MessageBox.Show("Updated Successfully!");
            }
            else
            {
                MessageBox.Show("Update Failed!");
            }
        }

        public bool IfEmailExist(String email)
        {
            string connectionString = @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";

            SqlConnection connection = new SqlConnection(connectionString);
            String query = "SELECT * FROM employeeTB WHERE email='" + email + "'";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            bool IfEmailExist = false;
            while (reader.Read())
            {
                IfEmailExist = true;
                break;
            }
            reader.Close();
            connection.Close();
            return IfEmailExist;
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            ShowAllEmployee();
        }

        public void LoadEmployeeListView(List<Employee> employees)
        {
            employeeListView.Items.Clear();
            foreach (var employee in employees)
            {
                ListViewItem item = new ListViewItem(employee.id.ToString());
                item.SubItems.Add(employee.name);
                item.SubItems.Add(employee.address);
                item.SubItems.Add(employee.email);
                item.SubItems.Add(employee.salary.ToString());
                employeeListView.Items.Add(item);
            }
        }

        public void ShowAllEmployee()
        {
            string connectionString = @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            String query = "SELECT * FROM employeeTB";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Employee> myList = new List<Employee>();
            while (reader.Read())
            {

                Employee anEmployee = new Employee();
                anEmployee.id = int.Parse(reader["ID"].ToString());
                anEmployee.name = reader["Name"].ToString();
                anEmployee.address = reader["Address"].ToString();
                anEmployee.email = reader["Email"].ToString();
                anEmployee.salary = Convert.ToDouble(reader["Salary"].ToString());

                myList.Add(anEmployee);
            }
            reader.Close();
            connection.Close();
            LoadEmployeeListView(myList);
        }

        private void employeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = employeeListView.SelectedItems[0];

            int id = int.Parse(item.Text.ToString());

            Employee anEmployee = GetEmployeeByID(id);

            if (anEmployee != null)
            {
                isUpdateMode = true;
                //saveButton.Text = "Update";
                employeeId = anEmployee.id;
                nameTextBox.Text = item.SubItems[1].Text;
                addressTextBox.Text = item.SubItems[2].Text;
                emailTextBox.Text = item.SubItems[3].Text;
                salaryTextBox.Text = item.SubItems[4].Text;
            }
        }

        public Employee GetEmployeeByID(int id)
        {
            string connectionString = @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";

            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM employeeTB WHERE ID ='" + id + "'";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> myList = new List<Employee>();

            while (reader.Read())
            {

                Employee anEmployee = new Employee();
                anEmployee.id = int.Parse(reader["ID"].ToString());
                anEmployee.name = reader["Name"].ToString();
                anEmployee.address = reader["Address"].ToString();
                anEmployee.email = reader["Email"].ToString();
                anEmployee.salary = Convert.ToDouble(reader["Salary"].ToString());
                myList.Add(anEmployee);
            }
            reader.Close();
            connection.Close();
            return myList.FirstOrDefault();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            anEmployee.name = nameTextBox.Text;
            anEmployee.address = addressTextBox.Text;
            anEmployee.email = emailTextBox.Text;
            anEmployee.salary = Convert.ToDouble(salaryTextBox.Text);
            if (isUpdateMode)
            {
                string connectionString =
                    @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";
                SqlConnection connection = new SqlConnection(connectionString);

                string query = "UPDATE employeeTB SET Name ='" + anEmployee.name + "', Address ='" + anEmployee.address +
                               "', Email ='" + anEmployee.email + "', Salary ='" + anEmployee.salary + "' WHERE ID = '" +
                               employeeId + "'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Updated Successfully!");
                    saveButton.Text = "Save";
                    employeeId = 0;
                    isUpdateMode = false;
                    ShowAllEmployee();
                }
                else
                {
                    MessageBox.Show("Update Failed!");
                }
            }

            else
            {
                if (IfEmailExist(anEmployee.email))
                {
                    MessageBox.Show("Email already Exist!");
                    return;
                }
                string connectionString = @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";

                SqlConnection connection = new SqlConnection(connectionString);
                String query = "INSERT INTO employeeTB VALUES ('" + anEmployee.name + "','" + anEmployee.address + "','" +
                               anEmployee.email + "','" + anEmployee.salary + "');";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                rowAffected = cmd.ExecuteNonQuery();
                connection.Close();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Updated Successfully!");
                }
                else
                {
                    MessageBox.Show("Update Failed!");
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            ListViewItem item = employeeListView.SelectedItems[0];

            int id = int.Parse(item.Text.ToString());

            //Employee anEmployee = GetEmployeeByID(id);

            if (anEmployee != null)
            {
                string connectionString = @"Server=PC-301-11\SQLEXPRESS;Database=EmployeeInfoDB;Trusted_Connection=True;";
                SqlConnection connection = new SqlConnection(connectionString);
                String query = "Select * From employeeTB WHERE ID ='" + anEmployee.id+ "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                rowAffected = cmd.ExecuteNonQuery();
                connection.Close();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Delete Successfully!");
                }
                else
                {
                    MessageBox.Show("Delete Failed!");
                }
            }

        }
    }
}
