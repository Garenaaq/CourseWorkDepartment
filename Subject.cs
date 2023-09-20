using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CourseWork2
{
    public partial class Subject : Form
    {
        string[] nameColumnSubject = { "ID", "Дисциплина" };
        NpgsqlConnection conn;
        DataTable dt = new DataTable();
        int idUser;
        string role;
        public Subject(int idUser, string role)
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
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=lecturer;Password=lecturer");
            }
            if (this.role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
            }
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumnSubject.Length; i++) dt.Columns.Add(nameColumnSubject[i]);
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
            cmd.CommandText = "SELECT * FROM subject WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(Convert.ToInt32(dr[0]), Convert.ToString(dr[1]));
            }
            dataGridView1.DataSource = dt;
            cmd.Dispose();
            conn.Close();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSubject addSubject = new AddSubject(this.role);
            addSubject.ShowDialog();
            ShowTable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idSubject = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE subject SET delete_flag = 1 WHERE ID = {idSubject}";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                List<string> listWithValuesRow = new List<string>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Value.ToString().Trim());
                }

                AddSubject addSubject = new AddSubject(listWithValuesRow, this.role);
                addSubject.ShowDialog();
                ShowTable();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%'", textBox1.Text.Trim());
        }
    }
}
