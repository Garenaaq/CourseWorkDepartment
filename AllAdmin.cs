using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork2
{
    public partial class AllAdmin : Form
    {
        string[] nameColumn = { "ID", "Фамилия", "Имя", "Отчество", "Логин" };
        NpgsqlConnection conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
        DataTable dt = new DataTable();
        int lecturer = 0;
        int idUser;
        public AllAdmin(int idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
        }

        public AllAdmin(int lecturer, int idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.lecturer = lecturer;
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = false;
            btnChange.Location = new Point(543, 22);
            label1.Location = new Point(560, 71);
            textBox1.Location = new Point(543, 99);
            label2.Location = new Point(569, 130);
            textBox2.Location = new Point(543, 160);
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
            if (lecturer == 0)
            {
                cmd.CommandText = $"SELECT ID, last_name_user, name_user, patronymic_user, login FROM authorization_info WHERE user_role = 'admin'";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dt.Rows.Add(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]));
                }
                dataGridView1.DataSource = dt;
                cmd.Dispose();
                conn.Close();
            }
            if (lecturer == 1)
            {
                cmd.CommandText = "SELECT authorization_info.ID, authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, authorization_info.login FROM authorization_info INNER JOIN lecturer ON lecturer.fk_user_info = authorization_info.ID WHERE (user_role = 'lecturer' OR user_role = 'head_chair') AND lecturer.delete_flag = 0";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dt.Rows.Add(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]));
                }
                dataGridView1.DataSource = dt;
                cmd.Dispose();
                conn.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddAdmin addAdmin = new AddAdmin();
            addAdmin.ShowDialog();
            ShowTable();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                List<string> listWithValuesRow = new List<string>();

                listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString().Trim());
                listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString().Trim());
                listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString().Trim());
                listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString().Trim());
                listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value.ToString().Trim());

                AddAdmin addAdmin = new AddAdmin(listWithValuesRow, lecturer);
                addAdmin.ShowDialog();
                ShowTable();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить администратора?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idAdmin = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"DELETE FROM authorization_info WHERE ID = {idAdmin}";
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
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Фамилия LIKE '%{0}%' and Логин LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Логин LIKE '%{0}%'", textBox2.Text.Trim());
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Фамилия LIKE '%{0}%' and Логин LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
        }
    }
}
