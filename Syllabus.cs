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

namespace CourseWork2
{
    public partial class Syllabus : Form
    {
        NpgsqlConnection conn;
        string[] nameColumnSubject = { "ID", "Дисциплина", "Семестр", "Специальность", "Кол-во часов лекций", "Кол-во часов практики", "Кол-во часов лабораторных работ"};
        DataTable dt = new DataTable();
        int idUser;
        string role;
        public Syllabus(int idUser, string role)
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
                label1.Location = new Point(695, 24);
                textBox1.Location = new Point(680, 50);
                label2.Location = new Point(710, 85);
                textBox2.Location = new Point(680, 110);
                label3.Location = new Point(690, 145);
                textBox3.Location = new Point(680, 170);
                btnAdd.Visible = false;
                btnChange.Visible = false;
                btnDelete.Visible = false;
            }
            if (this.role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
                label1.Location = new Point(695, 24);
                textBox1.Location = new Point(680, 50);
                label2.Location = new Point(710, 85);
                textBox2.Location = new Point(680, 110);
                label3.Location = new Point(690, 145);
                textBox3.Location = new Point(680, 170);
                btnAdd.Visible = false;
                btnChange.Visible = false;
                btnDelete.Visible = false;
            }
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumnSubject.Length; i++) dt.Columns.Add(nameColumnSubject[i]);
        }

        private void ShowTable()
        {
            int rowsCount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
            }
            dt.Clear();

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT subject_speciality.ID, subject.name_subject, subject_speciality.semester, speciality.name_speciality, subject_speciality.hours_lectures, " +
                $"subject_speciality.hours_practice, subject_speciality.hours_laboratory FROM subject_speciality LEFT JOIN speciality ON subject_speciality.fk_speciality = speciality.ID LEFT JOIN subject ON " +
                $"subject_speciality.fk_name_subject = subject.ID WHERE subject_speciality.delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(Convert.ToInt32(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]), Convert.ToString(dr[5]), Convert.ToString(dr[6]));
            }
            dataGridView1.DataSource = dt;
            cmd.Dispose();
            conn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSyllabus addSubject = new AddSyllabus(this.role);
            addSubject.ShowDialog();
            ShowTable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idSyllabus = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE subject_speciality SET delete_flag = 1 WHERE ID = {idSyllabus}";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                List<string> listWithValuesRow = new List<string>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Value.ToString().Trim());
                }
                AddSyllabus addSubject = new AddSyllabus(listWithValuesRow, this.role);
                addSubject.ShowDialog();
                ShowTable();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "" && textBox3.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%' and Семестр LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
            if (textBox2.Text.Trim() == "" && textBox3.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%'", textBox1.Text.Trim());
            }
            if (textBox3.Text.Trim() != "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%' and Специальность LIKE '%{1}%'", textBox1.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox3.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Дисциплина LIKE '%{1}%' and Семестр LIKE '%{2}%'", textBox3.Text.Trim(), textBox1.Text.Trim(), textBox2.Text.Trim());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Дисциплина LIKE '%{0}%' and Семестр LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Семестр LIKE '%{0}%'", textBox2.Text.Trim());
            }
            if (textBox3.Text.Trim() != "" && textBox1.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Семестр LIKE '%{0}%' and Специальность LIKE '%{1}%'", textBox2.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox3.Text.Trim() != "" && textBox1.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Семестр LIKE '%{1}%' and Дисциплина LIKE '%{2}%'", textBox3.Text.Trim(), textBox2.Text.Trim(), textBox1.Text.Trim());
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Дисциплина LIKE '%{1}%'", textBox3.Text.Trim(), textBox1.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%'", textBox3.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox1.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Семестр LIKE '%{0}%' and Специальность LIKE '%{1}%'", textBox2.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox1.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Семестр LIKE '%{1}%' and Дисциплина LIKE '%{2}%'", textBox3.Text.Trim(), textBox2.Text.Trim(), textBox1.Text.Trim());
            }
        }
    }
}
