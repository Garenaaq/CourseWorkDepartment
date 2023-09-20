namespace CourseWork2
{
    partial class AddSyllabus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.textBoxSemestr = new System.Windows.Forms.TextBox();
            this.comboBoxSpeciality = new System.Windows.Forms.ComboBox();
            this.textBoxLecture = new System.Windows.Forms.TextBox();
            this.textBoxPractice = new System.Windows.Forms.TextBox();
            this.textBoxLaboratory = new System.Windows.Forms.TextBox();
            this.comboBoxSubject = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(22, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дисциплина";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(22, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Семестр";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(22, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Специальность";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(22, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Кол-во часов лекций";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(22, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 19);
            this.label5.TabIndex = 4;
            this.label5.Text = "Кол-во часов практики";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(22, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(199, 19);
            this.label6.TabIndex = 5;
            this.label6.Text = "Кол-во часов лабораторных";
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAdd.Location = new System.Drawing.Point(139, 289);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(180, 45);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // textBoxSemestr
            // 
            this.textBoxSemestr.Location = new System.Drawing.Point(270, 78);
            this.textBoxSemestr.Name = "textBoxSemestr";
            this.textBoxSemestr.Size = new System.Drawing.Size(147, 20);
            this.textBoxSemestr.TabIndex = 8;
            this.textBoxSemestr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSemestr_KeyPress);
            // 
            // comboBoxSpeciality
            // 
            this.comboBoxSpeciality.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxSpeciality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpeciality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxSpeciality.FormattingEnabled = true;
            this.comboBoxSpeciality.Location = new System.Drawing.Point(270, 116);
            this.comboBoxSpeciality.Name = "comboBoxSpeciality";
            this.comboBoxSpeciality.Size = new System.Drawing.Size(147, 21);
            this.comboBoxSpeciality.TabIndex = 9;
            // 
            // textBoxLecture
            // 
            this.textBoxLecture.Location = new System.Drawing.Point(270, 161);
            this.textBoxLecture.Name = "textBoxLecture";
            this.textBoxLecture.Size = new System.Drawing.Size(147, 20);
            this.textBoxLecture.TabIndex = 10;
            this.textBoxLecture.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLecture_KeyPress);
            // 
            // textBoxPractice
            // 
            this.textBoxPractice.Location = new System.Drawing.Point(270, 201);
            this.textBoxPractice.Name = "textBoxPractice";
            this.textBoxPractice.Size = new System.Drawing.Size(147, 20);
            this.textBoxPractice.TabIndex = 11;
            this.textBoxPractice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPractice_KeyPress);
            // 
            // textBoxLaboratory
            // 
            this.textBoxLaboratory.Location = new System.Drawing.Point(270, 236);
            this.textBoxLaboratory.Name = "textBoxLaboratory";
            this.textBoxLaboratory.Size = new System.Drawing.Size(147, 20);
            this.textBoxLaboratory.TabIndex = 12;
            this.textBoxLaboratory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLaboratory_KeyPress);
            // 
            // comboBoxSubject
            // 
            this.comboBoxSubject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxSubject.FormattingEnabled = true;
            this.comboBoxSubject.Location = new System.Drawing.Point(270, 36);
            this.comboBoxSubject.Name = "comboBoxSubject";
            this.comboBoxSubject.Size = new System.Drawing.Size(147, 21);
            this.comboBoxSubject.TabIndex = 13;
            // 
            // AddSyllabus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 354);
            this.Controls.Add(this.comboBoxSubject);
            this.Controls.Add(this.textBoxLaboratory);
            this.Controls.Add(this.textBoxPractice);
            this.Controls.Add(this.textBoxLecture);
            this.Controls.Add(this.comboBoxSpeciality);
            this.Controls.Add(this.textBoxSemestr);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSyllabus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Учебный план";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox textBoxSemestr;
        private System.Windows.Forms.ComboBox comboBoxSpeciality;
        private System.Windows.Forms.TextBox textBoxLecture;
        private System.Windows.Forms.TextBox textBoxPractice;
        private System.Windows.Forms.TextBox textBoxLaboratory;
        private System.Windows.Forms.ComboBox comboBoxSubject;
    }
}