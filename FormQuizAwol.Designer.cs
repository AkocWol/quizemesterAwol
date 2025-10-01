namespace quizemesterAwol
{
    partial class FormQuizAwol
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
            this.components = new System.ComponentModel.Container();
            this.btnStartAwol = new System.Windows.Forms.Button();
            this.btnAawol = new System.Windows.Forms.Button();
            this.btnBawol = new System.Windows.Forms.Button();
            this.btnCawol = new System.Windows.Forms.Button();
            this.btnDawol = new System.Windows.Forms.Button();
            this.lblTitleAwol = new System.Windows.Forms.Label();
            this.lblScoreValueAwol = new System.Windows.Forms.Label();
            this.lblTimeTextAwol = new System.Windows.Forms.Label();
            this.lblScoreTextAwol = new System.Windows.Forms.Label();
            this.lblTimeValueAwol = new System.Windows.Forms.Label();
            this.lblQuestionsAwol = new System.Windows.Forms.Label();
            this.gameTimerAwol = new System.Windows.Forms.Timer(this.components);
            this.clbCategoriesAwol = new System.Windows.Forms.CheckedListBox();
            this.btnSkipAwol = new System.Windows.Forms.Button();
            this.btnScoresAwol = new System.Windows.Forms.Button();
            this.lblQtimeTextAwol = new System.Windows.Forms.Label();
            this.lblQtimeValueAwol = new System.Windows.Forms.Label();
            this.btnAdminAwol = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chkAdminModeAwol = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRoleAwol = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartAwol
            // 
            this.btnStartAwol.Location = new System.Drawing.Point(271, 75);
            this.btnStartAwol.Name = "btnStartAwol";
            this.btnStartAwol.Size = new System.Drawing.Size(188, 51);
            this.btnStartAwol.TabIndex = 0;
            this.btnStartAwol.Text = "Start";
            this.btnStartAwol.UseVisualStyleBackColor = true;
            this.btnStartAwol.Click += new System.EventHandler(this.btnStartAwol_Click);
            // 
            // btnAawol
            // 
            this.btnAawol.Location = new System.Drawing.Point(15, 322);
            this.btnAawol.Name = "btnAawol";
            this.btnAawol.Size = new System.Drawing.Size(170, 51);
            this.btnAawol.TabIndex = 1;
            this.btnAawol.Text = "A";
            this.btnAawol.UseVisualStyleBackColor = true;
            this.btnAawol.Click += new System.EventHandler(this.btnAawol_Click);
            // 
            // btnBawol
            // 
            this.btnBawol.Location = new System.Drawing.Point(216, 322);
            this.btnBawol.Name = "btnBawol";
            this.btnBawol.Size = new System.Drawing.Size(170, 51);
            this.btnBawol.TabIndex = 2;
            this.btnBawol.Text = "B";
            this.btnBawol.UseVisualStyleBackColor = true;
            this.btnBawol.Click += new System.EventHandler(this.btnBawol_Click);
            // 
            // btnCawol
            // 
            this.btnCawol.Location = new System.Drawing.Point(418, 322);
            this.btnCawol.Name = "btnCawol";
            this.btnCawol.Size = new System.Drawing.Size(170, 51);
            this.btnCawol.TabIndex = 3;
            this.btnCawol.Text = "C";
            this.btnCawol.UseVisualStyleBackColor = true;
            this.btnCawol.Click += new System.EventHandler(this.btnCawol_Click);
            // 
            // btnDawol
            // 
            this.btnDawol.Location = new System.Drawing.Point(618, 322);
            this.btnDawol.Name = "btnDawol";
            this.btnDawol.Size = new System.Drawing.Size(170, 51);
            this.btnDawol.TabIndex = 4;
            this.btnDawol.Text = "D";
            this.btnDawol.UseVisualStyleBackColor = true;
            this.btnDawol.Click += new System.EventHandler(this.btnDawol_Click);
            // 
            // lblTitleAwol
            // 
            this.lblTitleAwol.AutoSize = true;
            this.lblTitleAwol.Location = new System.Drawing.Point(311, 56);
            this.lblTitleAwol.Name = "lblTitleAwol";
            this.lblTitleAwol.Size = new System.Drawing.Size(107, 16);
            this.lblTitleAwol.TabIndex = 5;
            this.lblTitleAwol.Text = "Klik start to begin";
            // 
            // lblScoreValueAwol
            // 
            this.lblScoreValueAwol.AutoSize = true;
            this.lblScoreValueAwol.Location = new System.Drawing.Point(191, 273);
            this.lblScoreValueAwol.Name = "lblScoreValueAwol";
            this.lblScoreValueAwol.Size = new System.Drawing.Size(14, 16);
            this.lblScoreValueAwol.TabIndex = 6;
            this.lblScoreValueAwol.Text = "0";
            // 
            // lblTimeTextAwol
            // 
            this.lblTimeTextAwol.AutoSize = true;
            this.lblTimeTextAwol.Location = new System.Drawing.Point(311, 273);
            this.lblTimeTextAwol.Name = "lblTimeTextAwol";
            this.lblTimeTextAwol.Size = new System.Drawing.Size(175, 16);
            this.lblTimeTextAwol.TabIndex = 7;
            this.lblTimeTextAwol.Text = "Time before the game ends:";
            // 
            // lblScoreTextAwol
            // 
            this.lblScoreTextAwol.AutoSize = true;
            this.lblScoreTextAwol.Location = new System.Drawing.Point(139, 273);
            this.lblScoreTextAwol.Name = "lblScoreTextAwol";
            this.lblScoreTextAwol.Size = new System.Drawing.Size(46, 16);
            this.lblScoreTextAwol.TabIndex = 8;
            this.lblScoreTextAwol.Text = "Score:";
            // 
            // lblTimeValueAwol
            // 
            this.lblTimeValueAwol.AutoSize = true;
            this.lblTimeValueAwol.Location = new System.Drawing.Point(494, 273);
            this.lblTimeValueAwol.Name = "lblTimeValueAwol";
            this.lblTimeValueAwol.Size = new System.Drawing.Size(14, 16);
            this.lblTimeValueAwol.TabIndex = 9;
            this.lblTimeValueAwol.Text = "0";
            // 
            // lblQuestionsAwol
            // 
            this.lblQuestionsAwol.AutoSize = true;
            this.lblQuestionsAwol.Location = new System.Drawing.Point(330, 201);
            this.lblQuestionsAwol.Name = "lblQuestionsAwol";
            this.lblQuestionsAwol.Size = new System.Drawing.Size(67, 16);
            this.lblQuestionsAwol.TabIndex = 10;
            this.lblQuestionsAwol.Text = "Questions";
            // 
            // gameTimerAwol
            // 
            this.gameTimerAwol.Tick += new System.EventHandler(this.gameTimerAwol_Tick);
            // 
            // clbCategoriesAwol
            // 
            this.clbCategoriesAwol.FormattingEnabled = true;
            this.clbCategoriesAwol.Location = new System.Drawing.Point(34, 75);
            this.clbCategoriesAwol.Name = "clbCategoriesAwol";
            this.clbCategoriesAwol.Size = new System.Drawing.Size(140, 89);
            this.clbCategoriesAwol.TabIndex = 11;
            // 
            // btnSkipAwol
            // 
            this.btnSkipAwol.Location = new System.Drawing.Point(519, 138);
            this.btnSkipAwol.Name = "btnSkipAwol";
            this.btnSkipAwol.Size = new System.Drawing.Size(106, 46);
            this.btnSkipAwol.TabIndex = 12;
            this.btnSkipAwol.Text = "Skip (1x)";
            this.btnSkipAwol.UseVisualStyleBackColor = true;
            this.btnSkipAwol.Click += new System.EventHandler(this.btnSkipAwol_Click);
            // 
            // btnScoresAwol
            // 
            this.btnScoresAwol.Location = new System.Drawing.Point(519, 190);
            this.btnScoresAwol.Name = "btnScoresAwol";
            this.btnScoresAwol.Size = new System.Drawing.Size(106, 46);
            this.btnScoresAwol.TabIndex = 13;
            this.btnScoresAwol.Text = "Scoreboard";
            this.btnScoresAwol.UseVisualStyleBackColor = true;
            this.btnScoresAwol.Click += new System.EventHandler(this.btnScoresAwol_Click);
            // 
            // lblQtimeTextAwol
            // 
            this.lblQtimeTextAwol.AutoSize = true;
            this.lblQtimeTextAwol.Location = new System.Drawing.Point(213, 148);
            this.lblQtimeTextAwol.Name = "lblQtimeTextAwol";
            this.lblQtimeTextAwol.Size = new System.Drawing.Size(58, 16);
            this.lblQtimeTextAwol.TabIndex = 14;
            this.lblQtimeTextAwol.Text = "Q- Time:";
            // 
            // lblQtimeValueAwol
            // 
            this.lblQtimeValueAwol.AutoSize = true;
            this.lblQtimeValueAwol.Location = new System.Drawing.Point(277, 148);
            this.lblQtimeValueAwol.Name = "lblQtimeValueAwol";
            this.lblQtimeValueAwol.Size = new System.Drawing.Size(21, 16);
            this.lblQtimeValueAwol.TabIndex = 15;
            this.lblQtimeValueAwol.Text = "10";
            // 
            // btnAdminAwol
            // 
            this.btnAdminAwol.Location = new System.Drawing.Point(631, 138);
            this.btnAdminAwol.Name = "btnAdminAwol";
            this.btnAdminAwol.Size = new System.Drawing.Size(105, 46);
            this.btnAdminAwol.TabIndex = 16;
            this.btnAdminAwol.Text = "Admin";
            this.btnAdminAwol.UseVisualStyleBackColor = true;
            this.btnAdminAwol.Click += new System.EventHandler(this.btnAdminAwol_Click);
            // 
            // chkAdminModeAwol
            // 
            this.chkAdminModeAwol.AutoSize = true;
            this.chkAdminModeAwol.Location = new System.Drawing.Point(552, 29);
            this.chkAdminModeAwol.Name = "chkAdminModeAwol";
            this.chkAdminModeAwol.Size = new System.Drawing.Size(105, 20);
            this.chkAdminModeAwol.TabIndex = 17;
            this.chkAdminModeAwol.Text = "Admin mode";
            this.chkAdminModeAwol.UseVisualStyleBackColor = true;
            this.chkAdminModeAwol.CheckedChanged += new System.EventHandler(this.chkAdminModeAwol_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Role:";
            // 
            // lblRoleAwol
            // 
            this.lblRoleAwol.AutoSize = true;
            this.lblRoleAwol.Location = new System.Drawing.Point(122, 29);
            this.lblRoleAwol.Name = "lblRoleAwol";
            this.lblRoleAwol.Size = new System.Drawing.Size(33, 16);
            this.lblRoleAwol.TabIndex = 19;
            this.lblRoleAwol.Text = "user";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblRoleAwol);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkAdminModeAwol);
            this.Controls.Add(this.btnAdminAwol);
            this.Controls.Add(this.lblQtimeValueAwol);
            this.Controls.Add(this.lblQtimeTextAwol);
            this.Controls.Add(this.btnScoresAwol);
            this.Controls.Add(this.btnSkipAwol);
            this.Controls.Add(this.clbCategoriesAwol);
            this.Controls.Add(this.lblQuestionsAwol);
            this.Controls.Add(this.lblTimeValueAwol);
            this.Controls.Add(this.lblScoreTextAwol);
            this.Controls.Add(this.lblTimeTextAwol);
            this.Controls.Add(this.lblScoreValueAwol);
            this.Controls.Add(this.lblTitleAwol);
            this.Controls.Add(this.btnDawol);
            this.Controls.Add(this.btnCawol);
            this.Controls.Add(this.btnBawol);
            this.Controls.Add(this.btnAawol);
            this.Controls.Add(this.btnStartAwol);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartAwol;
        private System.Windows.Forms.Button btnAawol;
        private System.Windows.Forms.Button btnBawol;
        private System.Windows.Forms.Button btnCawol;
        private System.Windows.Forms.Button btnDawol;
        private System.Windows.Forms.Label lblTitleAwol;
        private System.Windows.Forms.Label lblScoreValueAwol;
        private System.Windows.Forms.Label lblTimeTextAwol;
        private System.Windows.Forms.Label lblScoreTextAwol;
        private System.Windows.Forms.Label lblTimeValueAwol;
        private System.Windows.Forms.Label lblQuestionsAwol;
        private System.Windows.Forms.Timer gameTimerAwol;
        private System.Windows.Forms.CheckedListBox clbCategoriesAwol;
        private System.Windows.Forms.Button btnSkipAwol;
        private System.Windows.Forms.Button btnScoresAwol;
        private System.Windows.Forms.Label lblQtimeTextAwol;
        private System.Windows.Forms.Label lblQtimeValueAwol;
        private System.Windows.Forms.Button btnAdminAwol;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chkAdminModeAwol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRoleAwol;
    }
}