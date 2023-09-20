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
    public partial class AddSyllabus : Form
    {
        NpgsqlConnection conn;
        List<string> rowValues;
        List<string> listWithSpeciality;
        List<string> listWithSubject;
        public AddSyllabus(string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            AddSpecialityInComboBox();
            AddSubjectInComboBox();
            rowValues = new List<string>();
        }

        public AddSyllabus(List<string> rowValues, string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            AddSpecialityInComboBox();
            AddSubjectInComboBox();
            btnAdd.Text = "Изменить";
            this.rowValues = rowValues;
            ChangeRow();
        }

        private void ChangeRow()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == "comboBoxSubject") c.Text = rowValues[1].Trim();
                if (c.Name == "textBoxSemestr") c.Text = rowValues[2].Trim();
                if (c.Name == "comboBoxSpeciality") c.Text = rowValues[3].Trim();
                if (c.Name == "textBoxLecture") c.Text = rowValues[4].Trim();
                if (c.Name == "textBoxPractice") c.Text = rowValues[5].Trim();
                if (c.Name == "textBoxLaboratory") c.Text = rowValues[6].Trim();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSemestr.Text) || string.IsNullOrWhiteSpace(comboBoxSpeciality.Text) 
                || string.IsNullOrWhiteSpace(comboBoxSubject.Text))
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
                        cmd.CommandText = $"SELECT EXISTS (SELECT * FROM subject_speciality WHERE semester = {Convert.ToInt32(textBoxSemestr.Text.Trim())} AND fk_name_subject = " +
                            $"(SELECT ID FROM subject WHERE name_subject = '{comboBoxSubject.Text.Trim()}') AND ID != {Convert.ToInt32(rowValues[0])} AND " +
                            $"fk_speciality = (SELECT ID FROM speciality WHERE name_speciality = '{comboBoxSpeciality.Text.Trim()}'))";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("У данной специальности в этом семестре уже есть такая дисциплина!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            string hoursLecturer = textBoxLecture.Text.Trim();
                            string hoursPractice = textBoxPractice.Text.Trim();
                            string hoursLaboratory = textBoxLaboratory.Text.Trim();

                            if (hoursLecturer == "") hoursLecturer = null;
                            if (hoursPractice == "") hoursPractice = null;
                            if (hoursLaboratory == "") hoursLaboratory = null;

                            cmd.CommandText = $"CALL update_subject_speciality_info('{comboBoxSubject.Text.Trim()}', {textBoxSemestr.Text.Trim()}, '{comboBoxSpeciality.Text.Trim()}', " +
                                $"{hoursLecturer}, {hoursPractice}, {hoursLaboratory}, {rowValues[0]})";
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
                        cmd.CommandText = $"SELECT EXISTS (SELECT * FROM subject_speciality WHERE fk_name_subject = (SELECT ID FROM subject WHERE " +
                            $"name_subject = '{comboBoxSubject.Text.Trim()}') AND semester = {textBoxSemestr.Text.Trim()} AND " +
                            $"fk_speciality = (SELECT ID FROM speciality WHERE name_speciality = '{comboBoxSpeciality.Text.Trim()}'))";
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.ExecuteScalar()))
                        {
                            MessageBox.Show("У данной специальности в этом семестре уже есть такая дисциплина!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                        }
                        else
                        {
                            string hoursLecturer = textBoxLecture.Text.Trim();
                            string hoursPractice = textBoxPractice.Text.Trim();
                            string hoursLaboratory = textBoxLaboratory.Text.Trim();
                            if (hoursLecturer == "") hoursLecturer = null;
                            if (hoursPractice == "") hoursPractice = null;
                            if (hoursLaboratory == "") hoursLaboratory = null;

                            cmd.CommandText = $"INSERT INTO subject_speciality VALUES (DEFAULT, (SELECT ID FROM subject WHERE name_subject = '{comboBoxSubject.Text.Trim()}'), {Convert.ToInt32(textBoxSemestr.Text.Trim())}, " +
                                $"(SELECT ID FROM speciality WHERE name_speciality = '{comboBoxSpeciality.Text.Trim()}'), {Convert.ToInt32(hoursLecturer)}, " +
                                $"{Convert.ToInt32(hoursPractice)}, {Convert.ToInt32(hoursLaboratory)}, 0)";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            conn.Close();
                            this.Close();
                        }
                    }
                }
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
            cmd.Dispose();
            for (int i = 0; i < listWithSpeciality.Count; i++) comboBoxSpeciality.Items.Add(listWithSpeciality[i]);
        }

        private void AddSubjectInComboBox()
        {
            listWithSubject = new List<string>();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT name_subject FROM subject WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listWithSubject.Add(Convert.ToString(dr[0]).Trim());
                }
            }
            dr.Close();
            conn.Close();
            cmd.Dispose();
            for (int i = 0; i < listWithSubject.Count; i++) comboBoxSubject.Items.Add(listWithSubject[i]);
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxSemestr_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxLecture_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxPractice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxLaboratory_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
