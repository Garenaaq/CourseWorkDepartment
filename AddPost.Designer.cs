namespace CourseWork2
{
    partial class AddPost
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
            this.textBoxNamePost = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLectureHours = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPracticeHours = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLabHours = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(28, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Должность";
            // 
            // textBoxNamePost
            // 
            this.textBoxNamePost.Location = new System.Drawing.Point(317, 41);
            this.textBoxNamePost.Name = "textBoxNamePost";
            this.textBoxNamePost.Size = new System.Drawing.Size(157, 20);
            this.textBoxNamePost.TabIndex = 2;
            this.textBoxNamePost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNamePost_KeyPress);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAdd.Location = new System.Drawing.Point(183, 246);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(181, 35);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(28, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "Кол-во часов лекций";
            // 
            // textBoxLectureHours
            // 
            this.textBoxLectureHours.Location = new System.Drawing.Point(317, 89);
            this.textBoxLectureHours.Name = "textBoxLectureHours";
            this.textBoxLectureHours.Size = new System.Drawing.Size(157, 20);
            this.textBoxLectureHours.TabIndex = 6;
            this.textBoxLectureHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLectureHours_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(28, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "Кол-во часов практики";
            // 
            // textBoxPracticeHours
            // 
            this.textBoxPracticeHours.Location = new System.Drawing.Point(317, 136);
            this.textBoxPracticeHours.Name = "textBoxPracticeHours";
            this.textBoxPracticeHours.Size = new System.Drawing.Size(157, 20);
            this.textBoxPracticeHours.TabIndex = 8;
            this.textBoxPracticeHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPracticeHours_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(28, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(237, 21);
            this.label4.TabIndex = 9;
            this.label4.Text = "Кол-во часов лабораторных";
            // 
            // textBoxLabHours
            // 
            this.textBoxLabHours.Location = new System.Drawing.Point(317, 184);
            this.textBoxLabHours.Name = "textBoxLabHours";
            this.textBoxLabHours.Size = new System.Drawing.Size(157, 20);
            this.textBoxLabHours.TabIndex = 10;
            this.textBoxLabHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLabHours_KeyPress);
            // 
            // AddPost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 302);
            this.Controls.Add(this.textBoxLabHours);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPracticeHours);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLectureHours);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.textBoxNamePost);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPost";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddPost";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNamePost;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLectureHours;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPracticeHours;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLabHours;
    }
}