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
    public partial class AddYear : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
        public AddYear()
        {

            InitializeComponent();
        }

        private void Добавить_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim()[textBox1.Text.Trim().Length - 5] != '-')
            {
                MessageBox.Show("Введите год в формате 2014-2015", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = $"SELECT EXISTS (SELECT year FROM academic_year WHERE year = '{textBox1.Text.Trim()}')";
                cmd.ExecuteNonQuery();
                if (Convert.ToBoolean(cmd.ExecuteScalar()))
                {
                    MessageBox.Show("Данный учебный год уже существует в базе!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conn.Close();
                }
                else
                {
                    cmd.CommandText = $"INSERT INTO academic_year VALUES (DEFAULT, '{textBox1.Text.Trim()}', 0)";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();
                    this.Close();
                }
            }
        }
    }
}
