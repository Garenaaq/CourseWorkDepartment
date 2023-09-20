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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CourseWork2
{
    public partial class AddSubject : Form
    {
        NpgsqlConnection conn;
        List<string> rowValues;
        public AddSubject(string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            rowValues = new List<string>();
        }

        public AddSubject(List<string> rowValues, string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            this.rowValues = rowValues;
            btnAdd.Text = "Изменить";
            ChangeRow();
        }

        private void ChangeRow()
        {
            int i = 1;
            foreach (Control c in this.Controls)
            {
                if (c is System.Windows.Forms.TextBox)
                {
                    c.Text = rowValues[i];
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Вы ввели не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (Convert.ToBoolean(rowValues.Count))
                {
                    if (MessageBox.Show("Вы уверены?", "Изменить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"SELECT EXISTS (SELECT name_subject FROM subject WHERE name_subject = '{textBox1.Text.Trim()}' AND ID != {Convert.ToInt32(rowValues[0])})";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("Такая дисциплина уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = $"UPDATE subject SET name_subject = '{textBox1.Text.Trim()}' WHERE ID = {Convert.ToInt32(rowValues[0])}";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            conn.Close();
                            this.Close();
                            return;
                        }

                    }
                }
                else
                {
                    if (MessageBox.Show("Вы уверены?", "Добавить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"SELECT EXISTS (SELECT name_subject FROM subject WHERE name_subject = '{textBox1.Text.Trim()}' AND delete_flag = 0)";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("Такая дисциплина уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            cmd.CommandText = $"INSERT INTO subject VALUES (DEFAULT, '{textBox1.Text.Trim()}', 0)";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            conn.Close();
                            this.Close();
                        }
                    }
                }

            }
        }
    }
}
