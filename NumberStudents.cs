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
    public partial class NumberStudents : Form
    {
        string[] nameColumnSpeciality = { "Специальность", "Количество групп", "Количество студентов" };
        NpgsqlConnection conn;
        DataTable dt = new DataTable();
        public NumberStudents(string role, int idUser)
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            CreateNameColumn();
            if (role == "admin")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
                ShowTable();
            }
            if (role == "lecturer")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=lecturer;Password=lecturer");
                ShowTable();
                AddInComboBoxSpec();
            }
            if (role == "head_chair")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair"); // Зав кафедрой
                ShowTable();
                AddInComboBoxSpec();
            }
            if (role == "root")
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
                ShowTable();
            }
        }

        private void CreateNameColumn()
        {
            for (int i = 0; i < nameColumnSpeciality.Length; i++) dt.Columns.Add(nameColumnSpeciality[i]);
        }

        private void AddInComboBoxSpec()
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT name_speciality FROM speciality WHERE delete_flag = 0";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr[0].ToString());
                }
            }
            comboBox1.Items.Add("");
            conn.Close();
            cmd.Dispose();
            dr.Close();
        }

        private void ShowTable(string nameChair=null)
        {
            int rowsCount = dataGridView1.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
            }
            dt.Clear();
            
            //
            List<string> listSpeciality = new List<string>();
            Dictionary<string, long[]> dictSpeciality = new Dictionary<string, long[]>();
            var currentYear = DateTime.Now.Year;
            //

            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            conn.Open();

           cmd.CommandText = $"SELECT name_speciality FROM speciality WHERE delete_flag = 0";

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listSpeciality.Add(reader[0].ToString());
            }
            reader.Close();

            foreach (var speciality in listSpeciality)
            {
                long[] squad_num = new long[2];
                cmd.CommandText = $"SELECT * FROM select_num_group_speciality('{speciality}')";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    long emptyCheck = Convert.ToInt64(reader[0]);
                    if (emptyCheck != 0)
                    {
                        squad_num[0] = Convert.ToInt64(reader[0]);
                        squad_num[1] = Convert.ToInt64(reader[1]);
                        dictSpeciality.Add(speciality, squad_num);
                    }
                }          
                reader.Close();
            }

            conn.Close();
            reader.Close();
            cmd.Dispose();

            foreach (var name_spec in dictSpeciality)
            {
                dt.Rows.Add(name_spec.Key, name_spec.Value[0], name_spec.Value[1]);
            }
            dataGridView1.DataSource = dt;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() != "")
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%'", comboBox1.Text.Trim());
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Специальность LIKE '%{0}%'", "");
            }
        }

    }
}
