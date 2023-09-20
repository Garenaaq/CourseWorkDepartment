using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CourseWork2
{
    public partial class AddGroup : Form
    {
        List<string> rowValues;
        NpgsqlConnection conn;
        List<string> listWithSpeciality;
        public AddGroup(string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            AddSpecialityInComboBox();
            this.rowValues = new List<string>();
        }

        public AddGroup(List<string> rowValues, string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            this.rowValues = rowValues;
            button1.Text = "Изменить";
            AddSpecialityInComboBox();
            ChangeRow();
        }

        private void ChangeRow()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == "textBox1") c.Text = rowValues[1].Trim();
                if (c.Name == "comboBoxSpeciality") c.Text = rowValues[2].Trim();
                if (c.Name == "textBox2") c.Text = rowValues[3].Trim();
                if (c.Name == "textBox3") c.Text = rowValues[5].Trim();
            }
        }

        private void AddSpecialityInComboBox()
        {
            listWithSpeciality = new List<string>();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT name_speciality FROM speciality WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listWithSpeciality.Add(Convert.ToString(dr[0]).Trim());
                }
            }
            dr.Close();
            conn.Close();
            for (int i = 0; i < listWithSpeciality.Count; i++) comboBoxSpeciality.Items.Add(listWithSpeciality[i]);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text)
                || string.IsNullOrWhiteSpace(comboBoxSpeciality.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Вы ввели не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (Convert.ToBoolean(rowValues.Count))
                {
                    if (Char.IsNumber(textBox1.Text.Trim()[textBox1.Text.Trim().Length - 1]) && Char.IsNumber(textBox1.Text.Trim()[textBox1.Text.Trim().Length - 2]))
                    {
                        if (MessageBox.Show("Вы уверены?", "Изменить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = $"SELECT EXISTS (SELECT name_squad FROM squad WHERE name_squad = '{textBox1.Text.Trim()}' AND ID != {rowValues[0]} AND " +
                                $"recruitment_year = '{textBox3.Text.Trim()}' AND delete_flag = 0)";
                            cmd.ExecuteNonQuery();
                            if (Convert.ToBoolean(cmd.ExecuteScalar()))
                            {
                                MessageBox.Show("Такая группа уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                conn.Close();
                            }
                            else
                            {
                                cmd.CommandText = $"CALL update_squad_info('{textBox1.Text.Trim()}', '{comboBoxSpeciality.Text}', {textBox2.Text.Trim()}," +
                                    $" {textBox1.Text.Trim()[textBox1.Text.Trim().Length - 2]}, '{textBox3.Text.Trim()}', {rowValues[0]})";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                conn.Close();
                                this.Close();
                            } 
                        }
                    }
                    else
                    {
                        MessageBox.Show("Последние два символа в названии группы должны быть цифрами!\n(Курс | Группа)", "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (Char.IsNumber(textBox1.Text.Trim()[textBox1.Text.Trim().Length - 1]) && Char.IsNumber(textBox1.Text.Trim()[textBox1.Text.Trim().Length - 2]))
                    {
                        if (MessageBox.Show("Вы уверены?", "Добавить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = $"SELECT EXISTS (SELECT name_squad FROM squad WHERE name_squad = '{textBox1.Text.Trim()}' AND recruitment_year = '{textBox3.Text.Trim()}' AND delete_flag = 0)";
                            cmd.ExecuteNonQuery();
                            if (Convert.ToBoolean(cmd.ExecuteScalar()))
                            {
                                MessageBox.Show("Такая группа уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                conn.Close();
                            }
                            else
                            {
                                cmd.CommandText = $"INSERT INTO squad VALUES (DEFAULT, '{textBox1.Text.Trim()}', (SELECT speciality.ID FROM speciality WHERE " +
                                    $"name_speciality = '{comboBoxSpeciality.Text}'), " +
                                    $"{textBox2.Text.Trim()}, {textBox1.Text.Trim()[textBox1.Text.Trim().Length - 2]}, '{textBox3.Text.Trim()}', 0)";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                conn.Close();
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Последние два символа в названии группы должны быть цифрами!\n(Курс | Группа)", "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
