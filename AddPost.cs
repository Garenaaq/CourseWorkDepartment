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
    public partial class AddPost : Form
    {
        List<string> rowValues;
        NpgsqlConnection conn;
        public AddPost(string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            rowValues = new List<string>();
        }

        public AddPost(List<string> rowValues, string role)
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
            foreach (Control c in this.Controls)
            {
                if (c.Name == "textBoxNamePost") c.Text = rowValues[1].Trim();
                if (c.Name == "textBoxLectureHours") c.Text = rowValues[2].Trim();
                if (c.Name == "textBoxPracticeHours") c.Text = rowValues[3].Trim();
                if (c.Name == "textBoxLabHours") c.Text = rowValues[4].Trim();
            }
        }

        private void textBoxNamePost_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNamePost.Text) || string.IsNullOrWhiteSpace(textBoxPracticeHours.Text)
                || string.IsNullOrWhiteSpace(textBoxLectureHours.Text) || string.IsNullOrWhiteSpace(textBoxLabHours.Text))
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
                        cmd.CommandText = $"SELECT EXISTS (SELECT name_post FROM post WHERE name_post = '{textBoxNamePost.Text.Trim()}' AND ID != {Convert.ToInt32(rowValues[0])})";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("Такая должность уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            cmd.CommandText = $"UPDATE post SET name_post = '{textBoxNamePost.Text.Trim()}', lecture_hours = {textBoxLectureHours.Text.Trim()}, " +
                                $"practice_hours = {textBoxPracticeHours.Text.Trim()}, laboratory_hours = {textBoxLabHours.Text.Trim()} WHERE ID = {rowValues[0]}";
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
                        cmd.CommandText = $"SELECT EXISTS (SELECT name_post FROM post WHERE name_post = '{textBoxNamePost.Text.Trim()}' AND delete_flag = 0)";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("Такая должность уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            cmd.CommandText = $"INSERT INTO post VALUES (DEFAULT, '{textBoxNamePost.Text.Trim()}', {textBoxLectureHours.Text.Trim()}, " +
                                $"{textBoxPracticeHours.Text.Trim()}, {textBoxLabHours.Text.Trim()}, 0)";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            conn.Close();
                            this.Close();
                        }
                    }
                }
                
            }
        }

        private void textBoxLectureHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxPracticeHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxLabHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
