using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork2
{
    public partial class Admin : Form
    {
        NpgsqlConnection conn;
        MainApp mainAppAdmin;
        int idUser;
        string role;
        public Admin(int idUser, MainApp mainAppAdmin, string role)
        {
            if (role != "root")
            {
                this.mainAppAdmin = mainAppAdmin;
                mainAppAdmin.Visible = false;
                this.role = role;
                this.idUser = idUser;
                InitializeComponent();
                if (this.role == "admin")
                {
                    conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=admin;Password=admin");
                    labelExp.Visible = false;
                    labelRate.Visible = false;
                    labelPost.Visible = false;
                    button3.Visible = false;
                    button4.Visible = false;
                    btnRollback.Location = new Point(40, 250);
                }
                if (this.role == "lecturer")
                {
                    conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=lecturer;Password=lecturer");
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Location = new Point(40, 154);
                    button4.Location = new Point(40, 200);
                    InfoLecturer();
                    btnRollback.Visible = false;
                }
                if (this.role == "head_chair")
                {
                    conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=head_chair;Password=head_chair");
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Location = new Point(40, 154);
                    button4.Location = new Point(40, 200);
                    InfoLecturer();
                    btnRollback.Visible = false;
                }
                FillLabelFIO();
            }
            else
            {
                conn = new NpgsqlConnection($"Server=localhost;Port=5432;Database=department;User Id=postgres;Password=root2002!");
                this.mainAppAdmin = mainAppAdmin;
                mainAppAdmin.Visible = false;
                InitializeComponent();
                this.role = role;
                this.idUser = idUser;
            }
        }

        private void FillLabelFIO()
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT last_name_user, name_user, patronymic_user FROM authorization_info WHERE ID = {this.idUser}";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            label1.Text = Convert.ToString(reader[0]);
            label2.Text = Convert.ToString(reader[1]);
            label3.Text = Convert.ToString(reader[2]);
            reader.Close();
            conn.Close();
        }

        private void InfoLecturer()
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT lecturer.rate, lecturer.date_of_receipt, post.name_post FROM lecturer INNER JOIN post ON lecturer.fk_post = post.ID WHERE lecturer.fk_user_info = {this.idUser}";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            labelPost.Text = "Должность: " + Convert.ToString(reader[2]);
            labelExp.Text = "Дата приёма: " + Convert.ToString(reader[1]).Replace(":", "").Replace("00000", "");
            labelRate.Text = "Ставка: " + Convert.ToString(reader[0]);
            reader.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AllAdmin allAdmin = new AllAdmin(this.idUser);
            allAdmin.ShowDialog();
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainAppAdmin.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AllAdmin allAdmin = new AllAdmin(this.idUser, 1);
            allAdmin.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NumberStudents numStud = new NumberStudents(this.role, this.idUser);
            numStud.ShowDialog();
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            RollbackTable rollbackTable = new RollbackTable(this.role);
            rollbackTable.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StudyLoad load = new StudyLoad(this.idUser, this.role);
            load.ShowDialog();
        }
    }
}
