﻿namespace CourseWork2
{
    partial class AddGroup
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
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBoxSpeciality = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(53, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название группы";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(53, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Специальность";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(53, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Количество студентов";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(334, 69);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 20);
            this.textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(334, 216);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(158, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // comboBoxSpeciality
            // 
            this.comboBoxSpeciality.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxSpeciality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpeciality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxSpeciality.FormattingEnabled = true;
            this.comboBoxSpeciality.Location = new System.Drawing.Point(334, 123);
            this.comboBoxSpeciality.Name = "comboBoxSpeciality";
            this.comboBoxSpeciality.Size = new System.Drawing.Size(158, 21);
            this.comboBoxSpeciality.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(195, 270);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 50);
            this.button1.TabIndex = 8;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(53, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 21);
            this.label5.TabIndex = 9;
            this.label5.Text = "Год набора";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(334, 168);
            this.textBox3.MaxLength = 4;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(158, 20);
            this.textBox3.TabIndex = 10;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox3_KeyPress);
            // 
            // AddGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 339);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxSpeciality);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(603, 378);
            this.Name = "AddGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddGroup";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBoxSpeciality;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
    }
}