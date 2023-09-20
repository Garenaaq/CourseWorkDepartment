using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace CourseWork2
{
    public partial class Autorization : Form
    {
        string role;
        int idUser;
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
        public Autorization()
        {
            InitializeComponent();
            textBoxPasswrd.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLogin.Text) || string.IsNullOrWhiteSpace(textBoxPasswrd.Text)) MessageBox.Show("Вы не ввели логин или пароль!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else 
            {        
                if (textBoxLogin.Text.Trim() == "root" && textBoxPasswrd.Text.Trim() == "root")
                {
                    role = "root";
                    idUser = 0;
                    MainApp mainApp = new MainApp(this, role, idUser);
                    mainApp.Show();
                }
                else
                {
                    string passSha256 = ComputeSha256Hash(textBoxPasswrd.Text.Trim());
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT user_role, ID FROM authorization_info WHERE login='{textBoxLogin.Text.Trim()}' AND pass = '{passSha256}'";
                    cmd.ExecuteNonQuery();
                    NpgsqlDataReader read = cmd.ExecuteReader();

                    if (read.HasRows)
                    {
                        while (read.Read())
                        {
                            role = Convert.ToString(read["user_role"]);
                            idUser = Convert.ToInt32(read["ID"]);
                        }
                        conn.Close();
                        read.Close();

                        MainApp mainApp = new MainApp(this, role, idUser);
                        mainApp.Show();
                    }
                    else
                    {
                        MessageBox.Show("Неправильно введён логин или пароль!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
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

        private void checkPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPass.Checked)
            {
                textBoxPasswrd.UseSystemPasswordChar = false;
            }
            else
            {
                textBoxPasswrd.UseSystemPasswordChar = true;
            }
        }
    }
}
