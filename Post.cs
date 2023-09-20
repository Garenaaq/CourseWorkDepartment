using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork2
{
    public partial class Post : Form
    {
        string[] nameColumnPost = { "ID", "Должность", "Кол-во часов лекций", "Кол-во часов практики", "Кол-во часов лабораторных"};
        NpgsqlConnection conn;
        int idUser;
        string role;
        public Post(int idUser, string role)
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
                btnChange.Visible = false;
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }
            if (this.role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
                btnChange.Visible = false;
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }
            ShowTable();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns[0].Visible = false;
        }

        private void CreateNameColumn(DataGridView dataGridView1)
        {
            dataGridView1.ColumnCount = nameColumnPost.Length;
            for (int i = 0; i < nameColumnPost.Length; i++)
            {
                dataGridView1.Columns[i].HeaderText = nameColumnPost[i];
            }
            dataGridView1.Columns[0].Visible = false;
        }

        private void ShowTable()
        {
            dataGridView1.Rows.Clear();
            CreateNameColumn(dataGridView1);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM post WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(Convert.ToInt32(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]));
                
            }
            dr.Close();
            cmd.Dispose();
            conn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddPost post = new AddPost(this.role);
            post.ShowDialog();
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
                    int idPost = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE post SET delete_flag = 1 WHERE ID = {idPost}";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            }
            
        }

        private void Post_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(this.btnDelete, "Вы удалите всю строчку!");
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

                AddPost addPost = new AddPost(listWithValuesRow, this.role);
                addPost.ShowDialog();
                ShowTable();
            }
        }
    }
}
