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
    public partial class MainApp : Form
    {
        Autorization autorization;
        bool btnExitAccClick;
        string role;
        int idUser;
        public MainApp(Autorization autorization, string role, int idUser)
        {
            InitializeComponent();
            this.role = role;
            this.autorization = autorization;
            if (this.role != "admin")
            {
                btnAdmin.Text = "Личный кабинет";
                btnLoad.Visible = false;
                btnSubject.Visible = false; 
                btnSyllabus.Location = new Point(300, 310);
            }
            autorization.Hide();
            btnExitAccClick = false;
            this.idUser = idUser;
        }

        private void MainApp_Load(object sender, EventArgs e)
        {

        }

        private void btnExitAcc_Click(object sender, EventArgs e)
        {
            btnExitAccClick = true;
            autorization.Show();
            this.Close();
        }

        private void btnAddLecturer_Click(object sender, EventArgs e)
        {
            Lecturer lecturer = new Lecturer(this.idUser, this.role);
            lecturer.Show();
        }

        private void btnSpeciality_Click(object sender, EventArgs e)
        {
            Speciality speciality = new Speciality(this.idUser, this.role);
            speciality.Show();
        }

        private void btnGroups_Click(object sender, EventArgs e)
        {
            Groups groups = new Groups(this.idUser, this.role);
            groups.Show();
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            Post post = new Post(this.idUser, this.role);
            post.Show();
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            Subject subject = new Subject(this.idUser, this.role);
            subject.Show();
        }

        private void btnSyllabus_Click(object sender, EventArgs e)
        {
            Syllabus syllabus = new Syllabus(this.idUser, this.role);
            syllabus.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StudyLoad studyLoad = new StudyLoad(this.idUser, this.role);
            studyLoad.Show();
        }

        private void MainAppAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnExitAccClick)
            {
                if (MessageBox.Show("Вы действительно хотите выйти из программы?", "Завершение программы", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    autorization.Close();
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin(this.idUser, this, this.role);
            admin.Show();
        }
    }
}
