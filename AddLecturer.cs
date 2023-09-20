using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CourseWork2
{
    public partial class AddLecturer : Form
    {
        List<string> listWithPost;
        NpgsqlConnection conn;
        List<string> rowValues;
        public AddLecturer(string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            rowValues = new List<string>();
            AddPostInComboBox();

        }

        public AddLecturer(List<string> rowValues, string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            this.rowValues = rowValues;
            btnAdd.Text = "Изменить";
            textBoxLogin.Enabled = false;
            textBoxPasswrd.Enabled = false;
            textBoxLogin.Visible = false;
            textBoxPasswrd.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            comboBoxRate.Location = new Point(179, 230);
            btnAdd.Location = new Point(164, 320);
            label5.Location = new Point(28, 230);
            this.MinimumSize = new Size(478, 410);
            this.Size = new Size(478, 410);
            AddPostInComboBox();
            ChangeRow();
        }

        private void ChangeRow()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == "textBoxPasswrd") continue;
                if (c.Name == "textBoxLastName") c.Text = rowValues[1].Trim();
                if (c.Name == "textBoxName") c.Text = rowValues[2].Trim();
                if (c.Name == "textBoxPatronymic") c.Text = rowValues[3].Trim();
                if (c.Name == "comboBoxPost") c.Text = rowValues[4].Trim();
                if (c.Name == "textBoxExperience") c.Text = rowValues[5].Trim();
                if (c.Name == "comboBoxRate") c.Text = rowValues[6].Trim();
            }
        }

        private void AddPostInComboBox()
        {
            listWithPost = new List<string>();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM post WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listWithPost.Add(Convert.ToString(dr[1]).Trim());
                }
            }
            dr.Close();
            conn.Close();
            cmd.Dispose();
            for (int i = 0; i < listWithPost.Count; i++) comboBoxPost.Items.Add(listWithPost[i]);
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
            if (Convert.ToBoolean(rowValues.Count))
            {
                if (string.IsNullOrWhiteSpace(textBoxPatronymic.Text)
                || string.IsNullOrWhiteSpace(comboBoxPost.Text) || string.IsNullOrWhiteSpace(textBoxExperience.Text)
                || string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxLastName.Text))
                {
                    MessageBox.Show("Вы ввели не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn;
                    int numberHeadOnChair = 0;

                    if (comboBoxPost.Text.Trim().ToLower().Contains("зав"))
                    {
                        conn.Open();
                        cmd.CommandText = $"SELECT COUNT(*) FROM lecturer WHERE lecturer.fk_post = (SELECT ID FROM post WHERE name_post = " +
                                $"'{comboBoxPost.Text.Trim()}') AND ID != {rowValues[0]} AND lecturer.delete_flag = 0";
                        cmd.ExecuteNonQuery();
                        NpgsqlDataReader reader1 = cmd.ExecuteReader();
                        reader1.Read();
                        numberHeadOnChair = Convert.ToInt32(reader1[0]);
                        reader1.Close();
                        cmd.Dispose();
                        conn.Close();
                    }   

                    if (Convert.ToBoolean(numberHeadOnChair)) MessageBox.Show("На данной кафедре хватает людей с этой должностью", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                    else
                    {
                        NpgsqlCommand cmd1 = new NpgsqlCommand();
                        cmd1.Connection = conn;
                        if (MessageBox.Show("Вы уверены?", "Изменить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (comboBoxPost.Text.Trim().ToLower().Contains("зав"))
                            {
                                try
                                {
                                    conn.Open();
                                    cmd1.CommandText = $"CALL update_lecturer_info('{comboBoxPost.Text.Trim()}', " +
                                        $"'{textBoxExperience.Text.Trim().Replace(textBoxExperience.Text.Trim()[2], '-')}', {rowValues[0]}," +
                                        $" {comboBoxRate.Text.Trim()})";
                                    cmd1.ExecuteNonQuery();

                                    cmd1.CommandText = $"UPDATE authorization_info SET user_role = 'head_chair', last_name_user = '{textBoxLastName.Text.Trim()}', " +
                                        $"name_user = '{textBoxName.Text.Trim()}', patronymic_user = '{textBoxPatronymic.Text.Trim()}' " +
                                        $"WHERE ID = {rowValues[rowValues.Count - 1]}";
                                    cmd1.ExecuteNonQuery();

                                    cmd1.Dispose();
                                    conn.Close();
                                    this.Close();
                                }
                                catch
                                {
                                    cmd1.Dispose();
                                    conn.Close();
                                    MessageBox.Show("Введите дату в формате ДД-ММ-ГГГГ!", "!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                            else
                            {
                                try
                                {
                                    conn.Open();
                                    cmd1.CommandText = $"CALL update_lecturer_info('{comboBoxPost.Text.Trim()}', " +
                                        $"'{textBoxExperience.Text.Trim().Replace(textBoxExperience.Text.Trim()[2], '-')}', {rowValues[0]}," +
                                        $" {comboBoxRate.Text.Trim()})";
                                    cmd1.ExecuteNonQuery();
                                    cmd1.CommandText = $"UPDATE authorization_info SET user_role = 'lecturer', last_name_user = '{textBoxLastName.Text.Trim()}', " +
                                        $"name_user = '{textBoxName.Text.Trim()}', patronymic_user = '{textBoxPatronymic.Text.Trim()}' " +
                                        $"WHERE ID = {rowValues[rowValues.Count - 1]}";
                                    cmd1.ExecuteNonQuery();
                                    cmd1.Dispose();
                                    conn.Close();
                                    this.Close();
                                }
                                catch
                                {
                                    cmd1.Dispose();
                                    conn.Close();
                                    MessageBox.Show("Введите дату в формате ДД-ММ-ГГГГ!", "!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                        }                  
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBoxLogin.Text) || string.IsNullOrWhiteSpace(textBoxPatronymic.Text)
                || string.IsNullOrWhiteSpace(comboBoxPost.Text) || string.IsNullOrWhiteSpace(textBoxExperience.Text)
                || string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxLastName.Text) || string.IsNullOrWhiteSpace(textBoxPasswrd.Text))
                {
                    MessageBox.Show("Вы ввели не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Вы уверены?", "Добавить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.CommandText = $"SELECT EXISTS (SELECT * FROM authorization_info WHERE login = '{textBoxLogin.Text.Trim()}')";
                        cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cmd.Dispose();
                            conn.Close();
                        }
                        else
                        {
                            conn.Close();
                            int numberHeadOnChair = 0;

                            if (comboBoxPost.Text.Trim().ToLower().Contains("зав") || comboBoxPost.Text.Trim().ToLower().Contains("зам")) 
                            {
                                conn.Open();
                                cmd.CommandText = $"SELECT COUNT(*) FROM lecturer WHERE lecturer.fk_post = (SELECT ID FROM post WHERE name_post = " +
                                        $"'{comboBoxPost.Text.Trim()}') AND lecturer.delete_flag = 0";
                                cmd.ExecuteNonQuery();
                                NpgsqlDataReader reader1 = cmd.ExecuteReader();
                                reader1.Read();
                                numberHeadOnChair = Convert.ToInt32(reader1[0]);
                                reader1.Close();

                                reader1.Close();
                                cmd.Dispose();
                                conn.Close();
                            }

                            if (numberHeadOnChair == 1) MessageBox.Show("На данной кафедре хватает людей с этой должностью! Освободите её прежде чем добавить кого-то нового!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else
                            {
                                if (comboBoxPost.Text.Trim().ToLower().Contains("зав"))
                                {
                                    NpgsqlCommand cmd1 = new NpgsqlCommand();
                                    cmd1.Connection = conn;
                                    try
                                    {
                                        conn.Open();
                                        string passwordHash = ComputeSha256Hash(Convert.ToString(textBoxPasswrd.Text.Trim()));
                                        cmd1.CommandText = $"INSERT INTO authorization_info VALUES (DEFAULT, '{textBoxLastName.Text.Trim()}', '{textBoxName.Text.Trim()}'," +
                                            $"'{textBoxPatronymic.Text.Trim()}', '{textBoxLogin.Text.Trim()}', '{passwordHash}', 'head_chair')"; // тут пароль
                                        cmd1.ExecuteNonQuery();

                                        cmd1.CommandText = $"CALL insert_lecturer('{comboBoxPost.Text.Trim()}', '{textBoxExperience.Text.Trim().Replace(textBoxExperience.Text.Trim()[2], '-')}'," +
                                            $" '{textBoxLogin.Text.Trim()}', {comboBoxRate.Text.Trim()}, 0)";
                                        cmd1.ExecuteNonQuery();

                                        cmd1.Dispose();
                                        conn.Close();
                                        this.Close();
                                    }
                                    catch
                                    {
                                        cmd1.CommandText = $"DELETE FROM authorization_info WHERE login = '{textBoxLogin.Text.Trim()}'";
                                        cmd1.ExecuteNonQuery();
                                        cmd1.Dispose();
                                        conn.Close();
                                        MessageBox.Show("Введите дату в формате ДД-ММ-ГГГГ!", "!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }

                                }
                                else
                                {
                                    NpgsqlCommand cmd1 = new NpgsqlCommand();
                                    cmd1.Connection = conn;
                                    try
                                    {
                                        conn.Open();
                                        string passwordHash = ComputeSha256Hash(Convert.ToString(textBoxPasswrd.Text.Trim()));
                                        cmd1.CommandText = $"INSERT INTO authorization_info VALUES (DEFAULT, '{textBoxLastName.Text.Trim()}', '{textBoxName.Text.Trim()}'," +
                                            $"'{textBoxPatronymic.Text.Trim()}', '{textBoxLogin.Text.Trim()}', '{passwordHash}', 'lecturer')"; // тут пароль
                                        cmd1.ExecuteNonQuery();

                                        cmd1.CommandText = $"CALL insert_lecturer('{comboBoxPost.Text.Trim()}', '{textBoxExperience.Text.Trim().Replace(textBoxExperience.Text.Trim()[2], '-')}'," +
                                            $" '{textBoxLogin.Text.Trim()}', {comboBoxRate.Text.Trim()}, 0)";
                                        cmd1.ExecuteNonQuery();

                                        cmd1.Dispose();
                                        conn.Close();
                                        this.Close();
                                    }
                                    catch
                                    {
                                        cmd1.CommandText = $"DELETE FROM authorization_info WHERE login = '{textBoxLogin.Text.Trim()}'";
                                        cmd1.ExecuteNonQuery();
                                        cmd1.Dispose();
                                        conn.Close();
                                        MessageBox.Show("Введите дату в формате ДД-ММ-ГГГГ!", "!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }

        private void textBoxLastName_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxName_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxPatronymic_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxExperience_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8 && number != 46)
            {
                e.Handled = true;
            }
        }

    }
}
