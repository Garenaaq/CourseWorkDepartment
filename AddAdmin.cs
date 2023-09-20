using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork2
{
    public partial class AddAdmin : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
        List<string> rowValues;
        int lectuer;
        public AddAdmin()
        {
            InitializeComponent();
            this.rowValues = new List<string>();
        }

        public AddAdmin(List<string> rowValues, int lectuer)
        {
            InitializeComponent();
            this.rowValues = rowValues;
            btnAdd.Text = "Изменить";
            ChangeRow();
            this.lectuer = lectuer;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void ChangeRow()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == "textBoxLastName") c.Text = rowValues[1].Trim();
                if (c.Name == "textBoxName") c.Text = rowValues[2].Trim();
                if (c.Name == "textBoxPatronymic") c.Text = rowValues[3].Trim();
                if (c.Name == "textBoxLogin") c.Text = rowValues[4].Trim();
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLastName.Text) || string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxPatronymic.Text) 
                || string.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                MessageBox.Show("Вы ввели не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (Convert.ToBoolean(rowValues.Count))
                {
                    if (MessageBox.Show("Вы уверены?", "Изменить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = $"SELECT EXISTS (SELECT * FROM authorization_info WHERE login = '{textBoxLogin.Text.Trim()}' AND ID != {rowValues[0]})";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            if (lectuer == 0)
                            {
                                MessageBox.Show("Администратор с таким логином уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                conn.Close();
                            }
                            else
                            {
                                MessageBox.Show("Преподаватель с таким логином уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                conn.Close();
                            }
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(textBoxPass.Text))
                            {
                                cmd.CommandText = $"UPDATE authorization_info SET last_name_user = '{textBoxLastName.Text.Trim()}', name_user = '{textBoxName.Text.Trim()}', " +
                                    $"patronymic_user = '{textBoxPatronymic.Text.Trim()}', login = '{textBoxLogin.Text.Trim()}' WHERE ID = {rowValues[0]}";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                conn.Close();
                                this.Close();
                            }
                            else
                            {
                                string passwordHash = ComputeSha256Hash(Convert.ToString(textBoxPass.Text.Trim()));
                                cmd.CommandText = $"UPDATE authorization_info SET last_name_user = '{textBoxLastName.Text.Trim()}', name_user = '{textBoxName.Text.Trim()}', " +
                                    $"patronymic_user = '{textBoxPatronymic.Text.Trim()}', login = '{textBoxLogin.Text.Trim()}', pass = '{passwordHash}' WHERE ID = {rowValues[0]}";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                conn.Close();
                                this.Close();
                            }
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("Вы уверены?", "Добавить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string passwordHash = ComputeSha256Hash(Convert.ToString(textBoxPass.Text.Trim()));
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = $"SELECT EXISTS (SELECT * FROM authorization_info WHERE login = '{textBoxLogin.Text.Trim()}')";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("Администратор с таким логином уже существует", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            cmd.CommandText = $"INSERT INTO authorization_info VALUES (DEFAULT, '{textBoxLastName.Text.Trim()}', '{textBoxName.Text.Trim()}', '{textBoxPatronymic.Text.Trim()}', " +
                                $"'{textBoxLogin.Text.Trim()}', '{passwordHash}', 'admin')";
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
