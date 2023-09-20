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
    public partial class Speciality : Form
    {
        string[] nameColumnSpeciality = { "ID", "Код", "Специальность"};
        NpgsqlConnection conn;
        DataTable dt = new DataTable();
        int idUser;
        string role;
        public Speciality(int idUser, string role)
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
                btnAddSpeciality.Visible = false;
                btnDeleteSpecialty.Visible = false;
                btnChange.Visible = false;
                label1.Location = new Point(715, 24);
                textBox1.Location = new Point(695, 50);
                label2.Location = new Point(695, 85);
                textBox2.Location = new Point(695, 110);
               
            }
            if (this.role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
                btnAddSpeciality.Visible = false;
                btnDeleteSpecialty.Visible = false;
                btnChange.Visible = false;
                label1.Location = new Point(715, 24);
                textBox1.Location = new Point(695, 50);
                label2.Location = new Point(695, 85);
                textBox2.Location = new Point(695, 110);
              
            }
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            ShowTable();
            dataGridView1.Columns[0].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumnSpeciality.Length; i++) dt.Columns.Add(nameColumnSpeciality[i]);
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
            cmd.CommandText = $"SELECT speciality.ID, speciality.name_speciality, speciality.code_speciality FROM speciality WHERE speciality.delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(Convert.ToInt32(dr[0]), Convert.ToString(dr[2]), Convert.ToString(dr[1]));
            }
            dataGridView1.DataSource = dt;
            cmd.Dispose();
            conn.Close();
        }


        private void btnAddSpeciality_Click(object sender, EventArgs e)
        {
            AddSpeciality addSpeciality = new AddSpeciality(this.role);
            addSpeciality.ShowDialog();
            ShowTable();
        }

        private void btnDeleteSpecialty_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    int indRowSelected = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
                    int idSpeciality = Convert.ToInt32(dataGridView1[0, indRowSelected].Value);
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = $"UPDATE speciality SET delete_flag = 1 WHERE ID = {idSpeciality}";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    conn.Close();
                    ShowTable();
                }
            } 
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Код LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
            if (textBox2.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%'", textBox1.Text.Trim());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%' and Код LIKE '%{1}%'", textBox1.Text.Trim(), textBox2.Text.Trim());
            }
            if (textBox1.Text.Trim() == "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Код LIKE '%{0}%'", textBox2.Text.Trim());
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

                AddSpeciality addSpeciality = new AddSpeciality(listWithValuesRow, this.role);
                addSpeciality.ShowDialog();
                ShowTable();
            }
        }
    }
}
