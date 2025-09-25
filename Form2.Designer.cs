namespace quizemesterAwol
{
    partial class Form2
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
            this.SuspendLayout();
            // 
            // btnStartAwol
            // 
            this.btnStartAwol.Location = new System.Drawing.Point(304, 94);
            this.btnStartAwol.Name = "btnStartAwol";
            this.btnStartAwol.Size = new System.Drawing.Size(188, 51);
            this.btnStartAwol.TabIndex = 0;
            this.btnStartAwol.Text = "Start";
            this.btnStartAwol.UseVisualStyleBackColor = true;
            this.btnStartAwol.Click += new System.EventHandler(this.btnStartAwol_Click);
            // 
            // btnAawol
            // 
            this.btnAawol.Location = new System.Drawing.Point(12, 322);
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
            this.lblTitleAwol.Location = new System.Drawing.Point(346, 69);
            this.lblTitleAwol.Name = "lblTitleAwol";
            this.lblTitleAwol.Size = new System.Drawing.Size(107, 16);
            this.lblTitleAwol.TabIndex = 5;
            this.lblTitleAwol.Text = "Klik start to begin";
            // 
            // lblScoreValueAwol
            // 
            this.lblScoreValueAwol.AutoSize = true;
            this.lblScoreValueAwol.Location = new System.Drawing.Point(265, 257);
            this.lblScoreValueAwol.Name = "lblScoreValueAwol";
            this.lblScoreValueAwol.Size = new System.Drawing.Size(14, 16);
            this.lblScoreValueAwol.TabIndex = 6;
            this.lblScoreValueAwol.Text = "0";
            // 
            // lblTimeTextAwol
            // 
            this.lblTimeTextAwol.AutoSize = true;
            this.lblTimeTextAwol.Location = new System.Drawing.Point(527, 257);
            this.lblTimeTextAwol.Name = "lblTimeTextAwol";
            this.lblTimeTextAwol.Size = new System.Drawing.Size(41, 16);
            this.lblTimeTextAwol.TabIndex = 7;
            this.lblTimeTextAwol.Text = "Time:";
            // 
            // lblScoreTextAwol
            // 
            this.lblScoreTextAwol.AutoSize = true;
            this.lblScoreTextAwol.Location = new System.Drawing.Point(213, 257);
            this.lblScoreTextAwol.Name = "lblScoreTextAwol";
            this.lblScoreTextAwol.Size = new System.Drawing.Size(46, 16);
            this.lblScoreTextAwol.TabIndex = 8;
            this.lblScoreTextAwol.Text = "Score:";
            // 
            // lblTimeValueAwol
            // 
            this.lblTimeValueAwol.AutoSize = true;
            this.lblTimeValueAwol.Location = new System.Drawing.Point(574, 257);
            this.lblTimeValueAwol.Name = "lblTimeValueAwol";
            this.lblTimeValueAwol.Size = new System.Drawing.Size(14, 16);
            this.lblTimeValueAwol.TabIndex = 9;
            this.lblTimeValueAwol.Text = "0";
            // 
            // lblQuestionsAwol
            // 
            this.lblQuestionsAwol.AutoSize = true;
            this.lblQuestionsAwol.Location = new System.Drawing.Point(365, 205);
            this.lblQuestionsAwol.Name = "lblQuestionsAwol";
            this.lblQuestionsAwol.Size = new System.Drawing.Size(67, 16);
            this.lblQuestionsAwol.TabIndex = 10;
            this.lblQuestionsAwol.Text = "Questions";
            // 
            // gameTimerAwol
            // 
            this.gameTimerAwol.Tick += new System.EventHandler(this.gameTimerAwol_Tick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}