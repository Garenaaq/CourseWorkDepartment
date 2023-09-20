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
    public partial class RollbackTable : Form
    {
        NpgsqlConnection conn;
        string[] nameColumn = { "ID", "Дата", "Изменения", "Таблица" };
        DataTable dt = new DataTable();
        public RollbackTable(string role)
        {
            InitializeComponent();
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            if (role == "admin")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            }
            textBoxNumBack.Visible = false;
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
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

            cmd.CommandText = $"SELECT ID, time, method_, tab_name FROM rollback_table";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]));
            }
            dataGridView1.DataSource = dt;
            dr.Close();
            cmd.Dispose();
            conn.Close();
            if (dataGridView1.Rows.Count == 0) btnRollback.Enabled = false;
            else btnRollback.Enabled = true;
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                if (comboBox1.Text == "По дате")
                {
                    if (dataGridView1.SelectedCells.Count != 0)
                    {
                        if (MessageBox.Show("Вы уверены, что хотите откатить БД?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            conn.Open();
                            int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                            string dateRollback = dataGridView1[1, indRowSelected].Value.ToString();
                            NpgsqlCommand command = new NpgsqlCommand();
                            command.Connection = conn;
                            command.CommandText = $"CALL func_back('{dateRollback}')";
                            command.ExecuteNonQuery();
                            command.Dispose();
                            conn.Close();
                            ShowTable();
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(textBoxNumBack.Text))
                    {
                        if (MessageBox.Show("Вы уверены, что хотите откатить БД?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            conn.Open();
                            int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                            string dateRollback = dataGridView1[1, indRowSelected].Value.ToString();
                            NpgsqlCommand command = new NpgsqlCommand();
                            command.Connection = conn;
                            command.CommandText = $"CALL func_back_num({textBoxNumBack.Text.Trim()})";
                            command.ExecuteNonQuery();
                            command.Dispose();
                            conn.Close();
                            ShowTable();
                        }
                    }
                }
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "По дате") textBoxNumBack.Visible = true;
            else textBoxNumBack.Visible = false;
        }

        private void textBoxNumBack_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
