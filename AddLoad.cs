using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CourseWork2
{
    public partial class AddLoad : Form
    {
        List<string> rowValues;
        NpgsqlConnection conn;
        List<string> listWithYear;
        List<string> listWithSpeciality;
        int idSubjectSpec;
        int idLecturer;
        // Время преподавателя
        int freeLectureHoursLect;
        int freePracticeHoursLect;
        int freeLabHoursLect;
        //
        int freeLectureHours;
        int freePracticeHours;
        int freeLabHours;
        public AddLoad(string role)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            rowValues = new List<string>();
            AddYearInComboBox();
            addSpecialityInComboBox();
            LabelTextClear();
            AddLecturerInComboBox();
        }

        public AddLoad(string role, List<string> rowValues)
        {
            InitializeComponent();
            if (role == "admin") conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            this.rowValues = rowValues;
            button1.Text = "Изменить";
            AddYearInComboBox();
            addSpecialityInComboBox();
            LabelTextClear();
            AddLecturerInComboBox();
            ChangeRow();
        }

        private void ChangeRow()
        {
            string FIOLecturer;

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user FROM authorization_info WHERE authorization_info.ID = {rowValues[1]}";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            FIOLecturer = $"{dr[0]} {dr[1]} {dr[2]}";
            dr.Close();
            conn.Close();


            foreach (Control c in this.Controls)
            {
                if (c.Name == "comboBoxYear") c.Text = rowValues[8].Trim();
                if (c.Name == "comboBoxSpeciality") c.Text = rowValues[2].Trim();
                if (c.Name == "textBoxLecture") c.Text = rowValues[5].Trim();
                if (c.Name == "textBoxPractice") c.Text = rowValues[6].Trim();
                if (c.Name == "textBoxLab") c.Text = rowValues[7].Trim();
            }
            comboBoxLecturer.Text = FIOLecturer.Trim();
            comboBoxSubject.Text = rowValues[3].Trim();
            comboBoxSemester.Text = rowValues[4].Trim();
        }


        private void добавитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddYear year = new AddYear();
            year.ShowDialog();
            comboBoxYear.Items.Clear();
            AddYearInComboBox();
        }

        private void LabelTextClear()
        {
            label12.Text = "Кол-во часов лекций - ";
            label13.Text = "Кол-во часов практики - ";
            label14.Text = "Кол-во часов лабораторных - ";
        }

        private void AddYearInComboBox()
        {
            listWithYear = new List<string>();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM academic_year";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listWithYear.Add(Convert.ToString(dr[1]).Trim());
                }
            }
            conn.Close();
            cmd.Dispose();
            dr.Close();
            for (int i = 0; i < listWithYear.Count; i++) comboBoxYear.Items.Add(listWithYear[i]);
        }

        private void AddLecturerInComboBox()
        {
            List<int>  listWithIDLecturer = new List<int>();
            List<string> listWithLecturerFIO = new List<string>();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT fk_user_info FROM lecturer WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listWithIDLecturer.Add(Convert.ToInt32(dr[0]));
                }
            }
            dr.Close();

            foreach (var idLecturer in listWithIDLecturer)
            {
                cmd.CommandText = $"SELECT last_name_user, name_user, patronymic_user FROM authorization_info WHERE ID = {idLecturer}";
                dr = cmd.ExecuteReader();
                dr.Read();
                string lecturerFIO = $"{dr[0]} {dr[1]} {dr[2]}";
                listWithLecturerFIO.Add(lecturerFIO);
                dr.Close();
            }
            conn.Close();
            cmd.Dispose();

            for (int i = 0; i < listWithLecturerFIO.Count; i++) comboBoxLecturer.Items.Add(listWithLecturerFIO[i]);
        }

        private void addSpecialityInComboBox()
        {
            listWithSpeciality = new List<string>();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT name_speciality FROM speciality WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listWithSpeciality.Add(Convert.ToString(dr[0]));
            }
            dr.Close();
            conn.Close();
            cmd.Dispose();

            for (int i = 0; i < listWithSpeciality.Count; i++) comboBoxSpeciality.Items.Add(listWithSpeciality[i]);
        }

        private void comboBoxLecturer_TextChanged(object sender, EventArgs e)
        {
            label5.Text = "Кол-во часов лекций -";
            label6.Text = "Кол-во часов практики -";
            label7.Text = "Кол-во часов лабораторных -";

            string[] lecturerFIO = comboBoxLecturer.Text.Split(' ');
            if (lecturerFIO.Length != 1)
            {
                string lastName = lecturerFIO[0];
                string name = lecturerFIO[1];
                string patronymic = lecturerFIO[2];

                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = $"SELECT authorization_info.ID FROM authorization_info INNER JOIN lecturer ON lecturer.fk_user_info = authorization_info.ID WHERE authorization_info.last_name_user = '{lastName}' AND authorization_info.name_user = '{name}' AND authorization_info.patronymic_user = '{patronymic}' AND lecturer.delete_flag = 0";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();

                int idUser = Convert.ToInt32(dr[0]);

                dr.Close();

                cmd.CommandText = $"SELECT lecture_hours, practice_hours, laboratory_hours, rate, lecturer.ID FROM post, lecturer WHERE lecturer.fk_user_info = {idUser} AND post.ID = lecturer.fk_post AND lecturer.delete_flag != 1";
                dr = cmd.ExecuteReader();
                dr.Read();

                int lectureHours = Convert.ToInt32(dr[0]) * Convert.ToInt32(dr[3]);
                int practiceHours = Convert.ToInt32(dr[1]) * Convert.ToInt32(dr[3]);
                int labHours = Convert.ToInt32(dr[2]) * Convert.ToInt32(dr[3]);
                idLecturer = Convert.ToInt32(dr[4]);
                dr.Close();

                int busyLectureHours = 0;
                int busyPracticeHours = 0;
                int busyLabHours = 0;
   
                cmd.CommandText = $"SELECT lecture_hours, practice_hours, laboratory_hours FROM load WHERE fk_lecturer = {idLecturer} AND fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}' AND delete_flag != 1) AND delete_flag != 1";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    busyLectureHours += Convert.ToInt32(dr[0]);
                    busyPracticeHours += Convert.ToInt32(dr[1]);
                    busyLabHours += Convert.ToInt32(dr[2]);
                }
                dr.Close();

                this.freeLectureHoursLect = lectureHours - busyLectureHours;
                this.freePracticeHoursLect = practiceHours - busyPracticeHours;
                this.freeLabHoursLect = labHours - busyLabHours;

                cmd.Dispose();
                conn.Close();

                label5.Text += " " + this.freeLectureHoursLect;
                label6.Text += " " + this.freePracticeHoursLect;
                label7.Text += " " + this.freeLabHoursLect;
            }
        }

        private void comboBoxSpeciality_TextChanged(object sender, EventArgs e)
        {
            LabelTextClear();
            comboBoxSubject.Items.Clear();
            comboBoxSemester.Items.Clear();
            List<string> listSubjectSpec = new List<string>();
            List<string> listSubjectSquad = new List<string>();

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT DISTINCT subject.name_subject FROM subject_speciality INNER JOIN speciality ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID WHERE subject_speciality.delete_flag = 0 AND speciality.delete_flag = 0 AND subject.delete_flag = 0 AND speciality.name_speciality = '{comboBoxSpeciality.Text}'";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listSubjectSpec.Add(Convert.ToString(dr[0]));
            }
            dr.Close();

            cmd.Dispose();
            conn.Close();
            for (int i = 0; i < listSubjectSpec.Count; i++) comboBoxSubject.Items.Add(listSubjectSpec[i]);
        }

        private void comboBoxSubject_TextChanged(object sender, EventArgs e)
        {
            LabelTextClear();
            comboBoxSemester.Items.Clear();
            List<int> listSemester = new List<int>();

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT semester FROM subject_speciality WHERE fk_name_subject = (SELECT ID FROM subject WHERE name_subject = '{comboBoxSubject.Text}') AND " +
                $"fk_speciality = (SELECT ID FROM speciality WHERE name_speciality = '{comboBoxSpeciality.Text}')";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listSemester.Add(Convert.ToInt32(dr[0]));
            }
            dr.Close();
            cmd.Dispose();
            conn.Close();
            for (int i = 0; i < listSemester.Count; i++) comboBoxSemester.Items.Add(listSemester[i]);
        }

        private void FreeHoursSquad()
        {
            if (!string.IsNullOrWhiteSpace(comboBoxSpeciality.Text.Trim()) &&
               !string.IsNullOrWhiteSpace(comboBoxYear.Text.Trim()) && !string.IsNullOrWhiteSpace(comboBoxSubject.Text.Trim()) && !string.IsNullOrWhiteSpace(comboBoxSemester.Text.Trim()) )
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = $"SELECT hours_lectures, hours_practice, hours_laboratory, ID FROM subject_speciality WHERE fk_name_subject = (SELECT ID FROM subject WHERE name_subject = '{comboBoxSubject.Text}') AND " +
                    $"fk_speciality = (SELECT ID FROM speciality WHERE name_speciality = '{comboBoxSpeciality.Text}') AND semester = {comboBoxSemester.Text}";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();

                int lectureHours = Convert.ToInt32(dr[0]);
                int practiceHours = Convert.ToInt32(dr[1]);
                int labHours = Convert.ToInt32(dr[2]);
                idSubjectSpec = Convert.ToInt32(dr[3]);

                dr.Close();


                int busyLectureHours = 0;
                int busyPracticeHours = 0;
                int busyLabHours = 0;
                cmd.CommandText = $"SELECT lecture_hours, practice_hours, laboratory_hours FROM load WHERE fk_subj_spec = {idSubjectSpec} " +
                    $"AND fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}' AND delete_flag = 0) AND delete_flag = 0";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    busyLectureHours += Convert.ToInt32(dr[0]);
                    busyPracticeHours += Convert.ToInt32(dr[1]);
                    busyLabHours += Convert.ToInt32(dr[2]);
                }
                dr.Close();

                this.freeLectureHours = lectureHours - busyLectureHours;
                this.freePracticeHours = practiceHours - busyPracticeHours;
                this.freeLabHours = labHours - busyLabHours;

                cmd.Dispose();
                conn.Close();

                label12.Text += " " + this.freeLectureHours;
                label13.Text += " " + this.freePracticeHours;
                label14.Text += " " + this.freeLabHours;
            }
        }

        private void comboBoxSemester_TextChanged(object sender, EventArgs e)
        {
            LabelTextClear();

            if (comboBoxSpeciality.Text.Trim() != "")
            {
                FreeHoursSquad();
            }
            if (comboBoxLecturer.Text != "")
            {
                label5.Text = "Кол-во часов лекций -";
                label6.Text = "Кол-во часов практики -";
                label7.Text = "Кол-во часов лабораторных -";

                int lectureHours = 0;
                int practiceHours = 0;
                int labHours = 0;

                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = $"SELECT load.lecture_hours, load.practice_hours, load.laboratory_hours FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID WHERE load.fk_lecturer = {idLecturer} AND load.fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}') AND subject_speciality.semester = {comboBoxSemester.Text} AND load.delete_flag != 1 AND subject_speciality.delete_flag != 1";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lectureHours += Convert.ToInt32(dr[0]);
                    practiceHours += Convert.ToInt32(dr[1]);
                    labHours += Convert.ToInt32(dr[2]);
                }
                dr.Close();

                cmd.CommandText = $"SELECT lecture_hours, practice_hours, laboratory_hours, rate, lecturer.ID FROM post, lecturer WHERE lecturer.ID = {idLecturer} AND post.ID = lecturer.fk_post AND lecturer.delete_flag != 1";
                dr = cmd.ExecuteReader();
                dr.Read();

                int lectureHoursAll = Convert.ToInt32(dr[0]) * Convert.ToInt32(dr[3]);
                int practiceHoursAll = Convert.ToInt32(dr[1]) * Convert.ToInt32(dr[3]);
                int labHoursAll = Convert.ToInt32(dr[2]) * Convert.ToInt32(dr[3]);
                idLecturer = Convert.ToInt32(dr[4]);

                dr.Close();

                conn.Close();

                label5.Text += " " + $"{lectureHoursAll - lectureHours}";
                label6.Text += " " + $"{practiceHoursAll - practiceHours}";
                label7.Text += " " + $"{labHoursAll - labHours}";

                this.freeLectureHoursLect = lectureHoursAll - lectureHours;
                this.freePracticeHoursLect = practiceHoursAll - practiceHours;
                this.freeLabHoursLect = labHoursAll - labHours;
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

        private void textBoxPrectice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBoxLab_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(rowValues.Count))
            {
                if (string.IsNullOrWhiteSpace(comboBoxLecturer.Text.Trim()) || string.IsNullOrWhiteSpace(comboBoxSpeciality.Text.Trim()) ||
                    string.IsNullOrWhiteSpace(comboBoxYear.Text.Trim()) || string.IsNullOrWhiteSpace(comboBoxSubject.Text.Trim()) || string.IsNullOrWhiteSpace(comboBoxSemester.Text.Trim()))
                {
                    MessageBox.Show("Вы указали не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Вы уверены?", "Добавить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string hoursLecturer = textBoxLecture.Text.Trim();
                        string hoursPractice = textBoxPractice.Text.Trim();
                        string hoursLaboratory = textBoxLab.Text.Trim();

                        if (textBoxLecture.Text.Trim() == "") hoursLecturer = null;
                        if (textBoxPractice.Text.Trim() == "") hoursPractice = null;
                        if (textBoxLab.Text.Trim() == "") hoursLaboratory = null;

                        if (this.freeLectureHoursLect - Convert.ToInt32(hoursLecturer) < 0 || this.freePracticeHoursLect - Convert.ToInt32(hoursPractice) < 0 || this.freeLabHoursLect - Convert.ToInt32(hoursLaboratory) < 0)
                        {
                            MessageBox.Show("У данного преподавателя не хватает времени!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (this.freeLectureHours - Convert.ToInt32(hoursLecturer) < 0 || this.freePracticeHours - Convert.ToInt32(hoursPractice) < 0 || this.freeLabHours - Convert.ToInt32(hoursLaboratory) < 0)
                        {
                            MessageBox.Show("Вы указываете лишнее время!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = $"SELECT EXISTS (SELECT * FROM load WHERE fk_lecturer = {idLecturer} AND fk_subj_spec = {idSubjectSpec} AND fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}') AND delete_flag = 0)";
                            cmd.ExecuteNonQuery();
                            if (Convert.ToBoolean(cmd.ExecuteScalar()))
                            {
                                MessageBox.Show("Данная нагрузка уже есть в базе!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                conn.Close();
                            }
                            else
                            {
                                if (hoursLecturer == null && hoursPractice == null && hoursLaboratory == null) MessageBox.Show("Вы не указали ни одной нагрузки!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else
                                {
                                    cmd.CommandText = $"INSERT INTO load VALUES (DEFAULT, {Convert.ToInt32(hoursLecturer)}, {Convert.ToInt32(hoursPractice)}, {Convert.ToInt32(hoursLaboratory)}, {idLecturer}, {idSubjectSpec}, (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}'), 0)";
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
            else
            {
                if (string.IsNullOrWhiteSpace(comboBoxLecturer.Text.Trim()) || string.IsNullOrWhiteSpace(comboBoxSpeciality.Text.Trim()) ||
                    string.IsNullOrWhiteSpace(comboBoxYear.Text.Trim()) || string.IsNullOrWhiteSpace(comboBoxSubject.Text.Trim()) || string.IsNullOrWhiteSpace(comboBoxSemester.Text.Trim()))
                {
                    MessageBox.Show("Вы указали не все данные!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Вы уверены?", "Изменить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        NpgsqlCommand cmd1 = new NpgsqlCommand();
                        conn.Open();
                        cmd1.Connection = conn;
                        cmd1.CommandText = $"SELECT lecture_hours, practice_hours, laboratory_hours, rate, lecturer.ID FROM post, lecturer WHERE lecturer.fk_user_info = {rowValues[1]} AND post.ID = lecturer.fk_post";
                        NpgsqlDataReader dr = cmd1.ExecuteReader();
                        dr.Read();

                        int lectureHours = Convert.ToInt32(dr[0]) * Convert.ToInt32(dr[3]);
                        int practiceHours = Convert.ToInt32(dr[1]) * Convert.ToInt32(dr[3]);
                        int labHours = Convert.ToInt32(dr[2]) * Convert.ToInt32(dr[3]);
                        idLecturer = Convert.ToInt32(dr[4]);

                        dr.Close();

                        int busyLectureHours = 0;
                        int busyPracticeHours = 0;
                        int busyLabHours = 0;
                        cmd1.CommandText = $"SELECT lecture_hours, practice_hours, laboratory_hours FROM load WHERE fk_lecturer = {idLecturer} AND fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}') AND ID != {rowValues[0]} AND load.delete_flag != 1";
                        dr = cmd1.ExecuteReader();
                        while (dr.Read())
                        {
                            busyLectureHours += Convert.ToInt32(dr[0]);
                            busyPracticeHours += Convert.ToInt32(dr[1]);
                            busyLabHours += Convert.ToInt32(dr[2]);
                        }
                        dr.Close();

                        this.freeLectureHoursLect = lectureHours - busyLectureHours;
                        this.freePracticeHoursLect = practiceHours - busyPracticeHours;
                        this.freeLabHoursLect = labHours - busyLabHours;

                        cmd1.Dispose();
                        conn.Close();



                        //////////////////// Здесь будут изменения

                        string hoursLecturer = textBoxLecture.Text.Trim();
                        string hoursPractice = textBoxPractice.Text.Trim();
                        string hoursLaboratory = textBoxLab.Text.Trim();

                        if (textBoxLecture.Text.Trim() == "") hoursLecturer = null;
                        if (textBoxPractice.Text.Trim() == "") hoursPractice = null;
                        if (textBoxLab.Text.Trim() == "") hoursLaboratory = null;
                       

                        if (this.freeLectureHoursLect - Convert.ToInt32(hoursLecturer) < 0 || this.freePracticeHoursLect - Convert.ToInt32(hoursPractice) < 0 || this.freeLabHoursLect - Convert.ToInt32(hoursLaboratory) < 0)
                        {
                            MessageBox.Show("У данного преподавателя не хватает времени!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (this.freeLectureHours + Convert.ToInt32(rowValues[5]) - Convert.ToInt32(hoursLecturer) < 0 || this.freePracticeHours + Convert.ToInt32(rowValues[6]) - Convert.ToInt32(hoursPractice) < 0 || this.freeLabHours + Convert.ToInt32(rowValues[7]) - Convert.ToInt32(hoursLaboratory) < 0)
                        {
                            MessageBox.Show("Вы указываете лишнее время!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = $"SELECT EXISTS (SELECT * FROM load WHERE fk_lecturer = {idLecturer} AND fk_subj_spec = {idSubjectSpec} AND fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}') AND delete_flag = 0 AND ID != {rowValues[0]})";
                            cmd.ExecuteNonQuery();
                            if (Convert.ToBoolean(cmd.ExecuteScalar()))
                            {
                                MessageBox.Show("Данная нагрузка уже есть в базе!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                conn.Close();
                            }
                            else
                            {
                                if (hoursLecturer == null && hoursPractice == null && hoursLaboratory == null) MessageBox.Show("Вы не указали ни одной нагрузки!", "!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else
                                {
                                    cmd.CommandText = $"UPDATE load SET lecture_hours = {Convert.ToInt32(hoursLecturer)}, practice_hours = {Convert.ToInt32(hoursPractice)}, laboratory_hours = {Convert.ToInt32(hoursLaboratory)}, fk_lecturer = {idLecturer}, fk_subj_spec = {idSubjectSpec}, fk_academic_year = (SELECT ID FROM academic_year WHERE year = '{comboBoxYear.Text}') WHERE ID = {rowValues[0]}";
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

        private void comboBoxYear_TextChanged(object sender, EventArgs e)
        {
            LabelTextClear();
            FreeHoursSquad();
            comboBoxLecturer.Text = "";
        }
    }
}
