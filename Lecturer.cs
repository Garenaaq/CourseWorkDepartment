using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CourseWork2
{
    public partial class Lecturer : Form
    {
        string[] nameColumn = { "ID", "Фамилия", "Имя", "Отчество", "Должность", "Дата приёма", "Ставка" };
        NpgsqlConnection conn;
        NpgsqlConnection conn2;
        DataTable dt = new DataTable();
        int idUser;
        string role;
        int idChair;

        public Lecturer(int idUser, string role)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.role = role;
            if (this.role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            if (this.role == "admin")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            }
            if (this.role == "lecturer")
            {
                btnAddLecturer.Visible = false;
                btnDeleteLecturer.Visible = false;
                btnChange.Visible = false;
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=lecturer;Password=lecturer");
                label1.Location = new Point(906, 26);
                textBox1.Location = new Point(886, 59);
                label2.Location = new Point(900, 90);
                textBox2.Location = new Point(880, 115);
            }
            if (this.role == "head_chair")
            {
                btnAddLecturer.Visible = false;
                btnDeleteLecturer.Visible = false;
                btnChange.Visible = false;
                label1.Location = new Point(906, 26);
                textBox1.Location = new Point(886, 59);
                label2.Location = new Point(900, 90);
                textBox2.Location = new Point(880, 115);
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
            }
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            if (role == "lecturer") dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[0].Visible = false;
        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumn.Length; i++) dt.Columns.Add(nameColumn[i]);
        }

        private void ShowTable()
        {
            int rowsCount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
            }
            dt.Clear();

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            if (this.role == "admin" || this.role == "root") cmd.CommandText = "SELECT * FROM lecturer WHERE delete_flag = 0";
            if (this.role == "head_chair") cmd.CommandText = $"SELECT * FROM lecturer WHERE delete_flag = 0";
            if (this.role == "lecturer") cmd.CommandText = $"SELECT * FROM lecturer WHERE delete_flag = 0";

            NpgsqlDataReader dr = cmd.ExecuteReader();

            if (this.role == "root")
            {
                conn2 = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            if (this.role == "admin")
            {
                conn2 = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            }
            if (this.role == "lecturer")
            {
                conn2 = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=lecturer;Password=lecturer");
            }
            if (this.role == "head_chair")
            {
                conn2 = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
            }

            NpgsqlCommand cmd2 = new NpgsqlCommand();
            conn2.Open();
            cmd2.Connection = conn2;
            NpgsqlDataReader dr2;

            while (dr.Read())
            {
                int id_lecturer = Convert.ToInt32(dr["ID"]);

                cmd2.CommandText = $"SELECT name_post FROM post WHERE ID = {Convert.ToInt32(dr["fk_post"])}";
                dr2 = cmd2.ExecuteReader();
                dr2.Read();

                string post = (string)dr2[0];

                dr2.Close();

                cmd2.CommandText = $"SELECT last_name_user, name_user, patronymic_user FROM authorization_info WHERE ID = {dr["fk_user_info"]}";
                dr2 = cmd2.ExecuteReader();
                dr2.Read();

                string lastName = Convert.ToString(dr2[0]);
                string name = Convert.ToString(dr2[1]);
                string patronymic = Convert.ToString(dr2[2]);

                dr2.Close();

                string dateOfReceipt = Convert.ToString(dr["date_of_receipt"]);
                dateOfReceipt = dateOfReceipt.Replace("0:00:00", "");


                string rate = Convert.ToString(dr["rate"]);

                dt.Rows.Add(id_lecturer, lastName, name, patronymic, post, dateOfReceipt, rate);
            }
            dataGridView1.DataSource = dt;
            cmd.Dispose();
            cmd2.Dispose();
            conn.Close();
        }

        private void btnAddLecturer_Click(object sender, EventArgs e)
        {
            AddLecturer addLecturer = new AddLecturer(this.role);
            addLecturer.ShowDialog();
            ShowTable();
        }


        private void btnDeleteLecturer_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idLecturer = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE authorization_info SET pass = NULL WHERE ID = (SELECT fk_user_info FROM lecturer WHERE ID = {idLecturer})";
                    command.ExecuteNonQuery();

                    command.CommandText = $"UPDATE lecturer SET delete_flag = 1 WHERE ID = {idLecturer}";
                    command.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Фамилия LIKE '%{0}%'", textBox1.Text.Trim());
            }

            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Фамилия LIKE '%{0}%' and Должность LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                List<string> listWithValuesRow = new List<string>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Value.ToString().Trim().Replace(",", "."));
                }
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = $"SELECT ID FROM authorization_info WHERE ID = (SELECT fk_user_info FROM lecturer WHERE ID = {listWithValuesRow[0]})";
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                string userID = Convert.ToString(dr[0]);
                conn.Close();
                command.Dispose();
                dr.Close();

                listWithValuesRow.Add(userID);
                AddLecturer addLecturer = new AddLecturer(listWithValuesRow, this.role);
                addLecturer.ShowDialog();
                ShowTable();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Должность LIKE '%{0}%'", textBox2.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox1.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Фамилия LIKE '%{0}%' and Должность LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
        }
    }
}
