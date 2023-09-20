using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork2
{
    public partial class Groups : Form
    {
        string[] nameColumn = { "Код группы", "Группа",  "Специальность", "Количество студентов", "Курс", "Год" };
        NpgsqlConnection conn;
        DataTable dt = new DataTable();
        int idUser;
        string role;
        public Groups(int idUser, string role)
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
                btnChange.Visible = false;
                btnAddGroup.Visible = false;
                btnDeleteGroup.Visible = false;
                label1.Location = new Point(695, 24);
                textBox1.Location = new Point(670, 50);
                label2.Location = new Point(700, 85);
                textBox2.Location = new Point(670, 110);
                label4.Location = new Point(740, 145);
                textBox3.Location = new Point(670, 170);
                label5.Location = new Point(710, 205);
                textBox4.Location = new Point(670, 230);
            }
            if (this.role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
                btnChange.Visible = false;
                btnAddGroup.Visible = false;
                btnDeleteGroup.Visible = false;
                label1.Location = new Point(695, 24);
                textBox1.Location = new Point(670, 50);
                label2.Location = new Point(700, 85);
                textBox2.Location = new Point(670, 110);
                label4.Location = new Point(740, 145);
                textBox3.Location = new Point(670, 170);
                label5.Location = new Point(710, 205);
                textBox4.Location = new Point(670, 230);
            }
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumn.Length; i++) dt.Columns.Add(nameColumn[i]);
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
            if (this.role == "admin" || this.role == "root")
            {
                cmd.CommandText = "SELECT squad.ID, squad.name_squad, speciality.name_speciality, squad.number_students, squad.course, squad.recruitment_year FROM squad LEFT JOIN " +
                    "speciality ON squad.fk_speciality = speciality.ID WHERE squad.delete_flag = 0";
            }
            if (this.role == "lecturer" || this.role == "head_chair")
            {
                cmd.CommandText = $"SELECT squad.ID, squad.name_squad,  speciality.name_speciality, squad.number_students, squad.course, squad.recruitment_year FROM squad LEFT JOIN " +
                    $"speciality ON squad.fk_speciality = speciality.ID  WHERE squad.delete_flag = 0";
            }

            NpgsqlDataReader dr = cmd.ExecuteReader();
            
            while (dr.Read())
            {
                int Idsquad = Convert.ToInt32(dr[0]);
                string nameSquad = Convert.ToString(dr[1]);

                string speciality = Convert.ToString(dr[2]);
                if (speciality == "")
                {
                    speciality = "null";
                }

                int numberStudents = Convert.ToInt32(dr[3]);
                dt.Rows.Add(Idsquad, nameSquad, speciality, numberStudents, dr["course"].ToString(), dr["recruitment_year"].ToString());
            }
            dataGridView1.DataSource = dt;
            cmd.Dispose();
            conn.Close();

        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            AddGroup addGroup = new AddGroup(this.role);
            addGroup.ShowDialog();
            ShowTable();
        }

        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            { 
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idSquad = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE squad SET delete_flag = 1 WHERE ID = {idSquad}";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%'", textBox1.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
            if (textBox2.Text.Trim() == "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Курс LIKE '%{1}%'", textBox1.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox2.Text.Trim() == "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Год LIKE '%{1}%'", textBox1.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Год LIKE '%{2}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox2.Text.Trim() == "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Курс LIKE '%{1}%' and Год LIKE '%{2}%'", textBox1.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%' and Год LIKE '%{3}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%'", textBox2.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Группа LIKE '%{1}%'", textBox2.Text.Trim(), textBox1.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Курс LIKE '%{1}%'", textBox2.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Год LIKE '%{1}%'", textBox2.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Год LIKE '%{2}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Курс LIKE '%{1}%' and Год LIKE '%{2}%'", textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%' and Год LIKE '%{3}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" && textBox2.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Курс LIKE '%{0}%'", textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() == "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Курс LIKE '%{0}%' and Группа LIKE '%{1}%'", textBox3.Text.Trim(), textBox1.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox2.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Курс LIKE '%{0}%' and Специальность LIKE '%{1}%'", textBox3.Text.Trim(), textBox2.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox2.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Курс LIKE '%{0}%' and Год LIKE '%{1}%'", textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "" && textBox4.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() == "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Курс LIKE '%{1}%' and Год LIKE '%{2}%'", textBox1.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox2.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Курс LIKE '%{1}%' and Год LIKE '%{2}%'", textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%' and Год LIKE '%{3}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() == "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Год LIKE '%{0}%'", textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() == "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Год LIKE '%{1}%'", textBox1.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() != "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Год LIKE '%{0}%' and Курс LIKE '%{1}%'", textBox4.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() == "" && textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Год LIKE '%{1}%'", textBox2.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Год LIKE '%{1}%' and Курс LIKE '%{2}%'", textBox1.Text.Trim(), textBox4.Text.Trim(), textBox3.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() == "" && textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Год LIKE '%{2}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() == "" && textBox3.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Курс LIKE '%{1}%' and Год LIKE '%{2}%'", textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Группа LIKE '%{0}%' and Специальность LIKE '%{1}%' and Курс LIKE '%{2}%' and Год LIKE '%{3}%'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                List<string> listWithValuesRow = new List<string>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    listWithValuesRow.Add(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Value.ToString().Trim());
                }

                AddGroup addGroup = new AddGroup(listWithValuesRow, this.role);
                addGroup.ShowDialog();
                ShowTable();
            }
        }
    }
}
