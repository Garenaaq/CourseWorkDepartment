using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CourseWork2
{
    public partial class StudyLoad : Form
    {
        string[] nameColumnLoad = { "ID", "ФИО", "Специальность", "Дисциплина", "Семестр", "Кол-во часов лекций", "Кол-во часов практики", "Кол-во часов лабораторных", "Год"};
        NpgsqlConnection conn;
        DataTable dt = new DataTable();
        int idUser;
        string role;
        List<int> listWithIduser = new List<int>();
        public StudyLoad(int idUser, string role)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.role = role;
            if (this.role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
            }
            if (this.role == "admin")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
            }
            if (this.role == "lecturer")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=lecturer;Password=lecturer");
                btnAdd.Visible = false;
                btnChange.Visible = false;
                btnDelete.Visible = false;
                label1.Visible = false;
                textBox1.Visible = false;
                label3.Location = new Point(970, 30);
                textBox3.Location = new Point(957, 60);
                label4.Location = new Point(986, 90);
                textBox4.Location = new Point(957, 120);
                checkBoxRemainder.Visible = false;
                checkBoxSum.Visible = false;
            }
            if (this.role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
                btnAdd.Visible = false;
                btnChange.Visible = false;
                btnDelete.Visible = false;
                label1.Location = new Point(999, 30);
                textBox1.Location = new Point(957, 60);
                label3.Location = new Point(970, 90);
                textBox3.Location = new Point(957, 120);
                label4.Location = new Point(986, 150);
                textBox4.Location = new Point(957, 180);
                checkBoxRemainder.Location = new Point(970, 220);
                checkBoxSum.Location = new Point(970, 300);
            }
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumnLoad.Length; i++) dt.Columns.Add(nameColumnLoad[i]);
        }

        private void CreateNameColumnForRemainder() 
        {
            dt.Rows.Clear();
            dt.Columns.Clear();
            string[] nameColumnLoad = { "Специальность", "Дисциплина", "Семестр", "Кол-во часов лекций", "Кол-во часов практики", "Кол-во часов лабораторных", "Год" };
            for (int i = 0; i < nameColumnLoad.Length; i++) dt.Columns.Add(nameColumnLoad[i]);
        }

        private void CreateNameColumnForSum()
        {
            dt.Rows.Clear();
            dt.Columns.Clear();
            string[] nameColumnLoad = { "ФИО", "Специальность", "Семестр", "Кол-во часов лекций", "Кол-во часов практики", "Кол-во часов лабораторных", "Год" };
            for (int i = 0; i < nameColumnLoad.Length; i++) dt.Columns.Add(nameColumnLoad[i]);
        }

        private void ShowTable()
        {
            int rowsCount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
            }
            dt.Clear();
            listWithIduser.Clear();

            NpgsqlCommand cmd = new NpgsqlCommand();

            if (this.role == "head_chair")
            {
                cmd.CommandText = $"SELECT load.ID, load.lecture_hours, load.practice_hours, load.laboratory_hours, subject.name_subject, speciality.name_speciality, academic_year.year, " +
                $"authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester, authorization_info.ID FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality " +
                $"ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                $"load.fk_academic_year = academic_year.ID INNER JOIN lecturer ON load.fk_lecturer = lecturer.ID INNER JOIN authorization_info ON " +
                $"lecturer.fk_user_info = authorization_info.ID WHERE load.delete_flag = 0";
            }
            if (this.role == "root" || this.role == "admin")
            {
                cmd.CommandText = $"SELECT load.ID, load.lecture_hours, load.practice_hours, load.laboratory_hours, subject.name_subject, speciality.name_speciality, academic_year.year, " +
                $"authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester, authorization_info.ID FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality " +
                $"ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                $"load.fk_academic_year = academic_year.ID INNER JOIN lecturer ON load.fk_lecturer = lecturer.ID INNER JOIN authorization_info ON " +
                $"lecturer.fk_user_info = authorization_info.ID WHERE load.delete_flag = 0";
            }
            if (this.role == "lecturer")
            {
                cmd.CommandText = $"SELECT load.ID, load.lecture_hours, load.practice_hours, load.laboratory_hours, subject.name_subject, speciality.name_speciality, academic_year.year, " +
                $"authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester, authorization_info.ID FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality " +
                $"ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                $"load.fk_academic_year = academic_year.ID INNER JOIN lecturer ON load.fk_lecturer = lecturer.ID INNER JOIN authorization_info ON " +
                $"lecturer.fk_user_info = authorization_info.ID WHERE load.delete_flag = 0 AND authorization_info.ID = {idUser}";
            }
            conn.Open();
            cmd.Connection = conn;
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listWithIduser.Add(Convert.ToInt32(dr[11]));
                string lastNameUser = dr[7].ToString();
                char nameUser = dr[8].ToString()[0];
                char patronymicUser = dr[9].ToString()[0];
                lastNameUser += $" {nameUser}. " + $"{patronymicUser}.";
                dt.Rows.Add(dr[0].ToString(), lastNameUser, dr[5].ToString(), dr[4].ToString(), dr[10].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[6].ToString());
            }
            dataGridView1.DataSource = dt;
            conn.Close();
            dr.Close();
        }

        private void ShowTableWithRemainder()
        {
            CreateNameColumnForRemainder();
            int rowsCount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
            }
            dt.Clear();
            listWithIduser.Clear();

            NpgsqlCommand cmd = new NpgsqlCommand();

            if (this.role == "head_chair")
            {
                cmd.CommandText = "SELECT subject_speciality.hours_lectures - SUM(load.lecture_hours), subject_speciality.hours_practice - SUM(load.practice_hours), " +
                    "subject_speciality.hours_laboratory - SUM(load.laboratory_hours), subject.name_subject, speciality.name_speciality, academic_year.year, " +
                    "subject_speciality.semester FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality ON " +
                    "subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                    "load.fk_academic_year = academic_year.ID WHERE load.delete_flag = 0 GROUP BY subject_speciality.hours_lectures, subject_speciality.hours_practice, " +
                    "subject_speciality.hours_laboratory, subject.name_subject, speciality.name_speciality, academic_year.year, subject_speciality.semester";
            }
            if (this.role == "root" || this.role == "admin")
            {
                cmd.CommandText = "SELECT subject_speciality.hours_lectures - SUM(load.lecture_hours), subject_speciality.hours_practice - SUM(load.practice_hours), " +
                   "subject_speciality.hours_laboratory - SUM(load.laboratory_hours), subject.name_subject, speciality.name_speciality, academic_year.year, " +
                   "subject_speciality.semester FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality ON " +
                   "subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                   "load.fk_academic_year = academic_year.ID WHERE load.delete_flag = 0 GROUP BY subject_speciality.hours_lectures, subject_speciality.hours_practice, " +
                   "subject_speciality.hours_laboratory, subject.name_subject, speciality.name_speciality, academic_year.year, subject_speciality.semester";
            }
            if (this.role == "lecturer")
            {
                cmd.CommandText = "SELECT subject_speciality.hours_lectures - SUM(load.lecture_hours), subject_speciality.hours_practice - SUM(load.practice_hours), " +
                   "subject_speciality.hours_laboratory - SUM(load.laboratory_hours), subject.name_subject, speciality.name_speciality, academic_year.year, " +
                   "subject_speciality.semester FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality ON " +
                   "subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                   "load.fk_academic_year = academic_year.ID WHERE load.delete_flag = 0 GROUP BY subject_speciality.hours_lectures, subject_speciality.hours_practice, " +
                   "subject_speciality.hours_laboratory, subject.name_subject, speciality.name_speciality, academic_year.year, subject_speciality.semester";
            }
            conn.Open();
            cmd.Connection = conn;
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (Convert.ToInt32(dr[0]) == 0 && Convert.ToInt32(dr[1]) == 0 && Convert.ToInt32(dr[2]) == 0) continue;
                dt.Rows.Add(dr[4].ToString(), dr[3].ToString(), dr[6].ToString(), dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[5].ToString());
            }
            dataGridView1.DataSource = dt;
            conn.Close();
            dr.Close();
        }

        private void ShowTableWithSum()
        {
            dt.Rows.Clear();
            dt.Columns.Clear();
            CreateNameColumnForSum();
            int rowsCount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
            }
            dt.Clear();
            listWithIduser.Clear();

            NpgsqlCommand cmd = new NpgsqlCommand();

            if (this.role == "head_chair")
            {
                cmd.CommandText = $"SELECT SUM(load.lecture_hours), SUM(load.practice_hours), SUM(load.laboratory_hours), speciality.name_speciality, academic_year.year, " +
                $"authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality " +
                $"ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                $"load.fk_academic_year = academic_year.ID INNER JOIN lecturer ON load.fk_lecturer = lecturer.ID INNER JOIN authorization_info ON " +
                $"lecturer.fk_user_info = authorization_info.ID WHERE load.delete_flag = 0 GROUP BY speciality.name_speciality, academic_year.year, authorization_info.last_name_user, " +
                $"authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester";
            }
            if (this.role == "root" || this.role == "admin")
            {
                cmd.CommandText = $"SELECT SUM(load.lecture_hours), SUM(load.practice_hours), SUM(load.laboratory_hours), speciality.name_speciality, academic_year.year, " +
                $"authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality " +
                $"ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                $"load.fk_academic_year = academic_year.ID INNER JOIN lecturer ON load.fk_lecturer = lecturer.ID INNER JOIN authorization_info ON " +
                $"lecturer.fk_user_info = authorization_info.ID WHERE load.delete_flag = 0 GROUP BY speciality.name_speciality, academic_year.year, authorization_info.last_name_user, " +
                $"authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester";
            }
            if (this.role == "lecturer")
            {
                cmd.CommandText = $"SELECT SUM(load.lecture_hours), SUM(load.practice_hours), SUM(load.laboratory_hours), speciality.name_speciality, academic_year.year, " +
                $"authorization_info.last_name_user, authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester FROM load INNER JOIN subject_speciality ON load.fk_subj_spec = subject_speciality.ID INNER JOIN speciality " +
                $"ON subject_speciality.fk_speciality = speciality.ID INNER JOIN subject ON subject_speciality.fk_name_subject = subject.ID INNER JOIN academic_year ON " +
                $"load.fk_academic_year = academic_year.ID INNER JOIN lecturer ON load.fk_lecturer = lecturer.ID INNER JOIN authorization_info ON " +
                $"lecturer.fk_user_info = authorization_info.ID WHERE load.delete_flag = 0 GROUP BY speciality.name_speciality, academic_year.year, authorization_info.last_name_user, " +
                $"authorization_info.name_user, authorization_info.patronymic_user, subject_speciality.semester";
            }
            conn.Open();
            cmd.Connection = conn;
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string lastNameUser = dr[5].ToString();
                char nameUser = dr[6].ToString()[0];
                char patronymicUser = dr[7].ToString()[0];
                lastNameUser += $" {nameUser}. " + $"{patronymicUser}.";
                dt.Rows.Add(lastNameUser, dr[3].ToString(), dr[8].ToString(), dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[4].ToString());
            }
            dataGridView1.DataSource = dt;
            conn.Close();
            dr.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddLoad addload = new AddLoad(this.role);
            addload.ShowDialog();
            ShowTable();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                List<string> listWithValuesRow = new List<string>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Value.ToString().Trim().Replace(",", "."));
                }

                listWithValuesRow[1] = Convert.ToString(listWithIduser[dataGridView1.CurrentRow.Index]);
                AddLoad addload = new AddLoad(this.role, listWithValuesRow);
                addload.ShowDialog();
                ShowTable();
            }
        }

        private void checkBoxRemainder_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            checkBoxSum.Checked = false;
            this.ActiveControl = null;
            dt = new DataTable();
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dataGridView1.DataSource = dt;
            if (!checkBoxRemainder.Checked)
            {
                dt.Rows.Clear();
                dt.Columns.Clear();
                CreateNameColumn();
                dataGridView1.Columns[0].Visible = false;
                btnDelete.Enabled = true;
                btnChange.Enabled = true;
                btnAdd.Enabled = true;
                textBox1.Enabled = true;
                ShowTable();
                return;
            }
            btnDelete.Enabled = false;
            btnChange.Enabled = false;
            btnAdd.Enabled = false;
            textBox1.Enabled = false;
            ShowTableWithRemainder();
        }

        private void checkBoxSum_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxRemainder.Checked = false;
            this.ActiveControl = null;
            dt = new DataTable();
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dataGridView1.DataSource = dt;
            if (!checkBoxSum.Checked)
            {
                dt.Rows.Clear();
                dt.Columns.Clear();
                CreateNameColumn();
                dataGridView1.Columns[0].Visible = false;
                btnDelete.Enabled = true;
                btnChange.Enabled = true;
                btnAdd.Enabled = true;
                textBox1.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                ShowTable();
                return;
            }
            btnDelete.Enabled = false;
            btnChange.Enabled = false;
            btnAdd.Enabled = false;
            textBox1.Enabled = true;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            ShowTableWithSum();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idLoad = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE load SET delete_flag = 1 WHERE ID = {idLoad}";
                    command.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%'", textBox1.Text.Trim());
            }
            if (textBox3.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%' and Дисциплина LIKE '%{1}%'", textBox1.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox3.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%' and Семестр LIKE '%{1}%'", textBox1.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%' and Дисциплина LIKE '%{1}%' and Семестр LIKE '%{2}%'", textBox1.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%'", textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%' and ФИО LIKE '%{1}%'", textBox3.Text.Trim(), textBox1.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%' and Семестр LIKE '%{1}%'", textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%' and Дисциплина LIKE '%{1}%' and Семестр LIKE '%{2}%'", textBox1.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Семестр LIKE '%{0}%'", textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%' and Семестр LIKE '%{1}%'", textBox1.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Семестр LIKE '%{0}%' and Дисциплина LIKE '%{1}%'", textBox4.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("ФИО LIKE '%{0}%' and Семестр LIKE '%{1}%' and Дисциплина LIKE '%{2}%'", textBox1.Text.Trim(), textBox4.Text.Trim(), textBox3.Text.Trim());
            }
        }

        private void checkBoxRemainder_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

    }
}
