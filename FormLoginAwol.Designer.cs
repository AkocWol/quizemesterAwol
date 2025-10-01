namespace quizemesterAwol
{
    partial class FormLoginAwol
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
            this.txtUsernameAwol = new System.Windows.Forms.TextBox();
            this.btnLoginAwol = new System.Windows.Forms.Button();
            this.btnRegisterAwol = new System.Windows.Forms.Button();
            this.txtPasswordAwol = new System.Windows.Forms.TextBox();
            this.lblMessageAwol = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(186, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username";
            // 
            // txtUsernameAwol
            // 
            this.txtUsernameAwol.Location = new System.Drawing.Point(293, 124);
            this.txtUsernameAwol.Name = "txtUsernameAwol";
            this.txtUsernameAwol.Size = new System.Drawing.Size(100, 22);
            this.txtUsernameAwol.TabIndex = 3;
            // 
            // btnLoginAwol
            // 
            this.btnLoginAwol.Location = new System.Drawing.Point(200, 255);
            this.btnLoginAwol.Name = "btnLoginAwol";
            this.btnLoginAwol.Size = new System.Drawing.Size(75, 23);
            this.btnLoginAwol.TabIndex = 4;
            this.btnLoginAwol.Text = "Login";
            this.btnLoginAwol.UseVisualStyleBackColor = true;
            this.btnLoginAwol.Click += new System.EventHandler(this.btnLoginAwol_Click);
            // 
            // btnRegisterAwol
            // 
            this.btnRegisterAwol.Location = new System.Drawing.Point(318, 255);
            this.btnRegisterAwol.Name = "btnRegisterAwol";
            this.btnRegisterAwol.Size = new System.Drawing.Size(75, 23);
            this.btnRegisterAwol.TabIndex = 5;
            this.btnRegisterAwol.Text = "Register";
            this.btnRegisterAwol.UseVisualStyleBackColor = true;
            this.btnRegisterAwol.Click += new System.EventHandler(this.btnRegisterAwol_Click);
            // 
            // txtPasswordAwol
            // 
            this.txtPasswordAwol.Location = new System.Drawing.Point(293, 186);
            this.txtPasswordAwol.Name = "txtPasswordAwol";
            this.txtPasswordAwol.Size = new System.Drawing.Size(100, 22);
            this.txtPasswordAwol.TabIndex = 6;
            // 
            // lblMessageAwol
            // 
            this.lblMessageAwol.AutoSize = true;
            this.lblMessageAwol.Location = new System.Drawing.Point(249, 312);
            this.lblMessageAwol.Name = "lblMessageAwol";
            this.lblMessageAwol.Size = new System.Drawing.Size(64, 16);
            this.lblMessageAwol.TabIndex = 7;
            this.lblMessageAwol.Text = "Message";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblMessageAwol);
            this.Controls.Add(this.txtPasswordAwol);
            this.Controls.Add(this.btnRegisterAwol);
            this.Controls.Add(this.btnLoginAwol);
            this.Controls.Add(this.txtUsernameAwol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormLoginAwol_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsernameAwol;
        private System.Windows.Forms.Button btnLoginAwol;
        private System.Windows.Forms.Button btnRegisterAwol;
        private System.Windows.Forms.TextBox txtPasswordAwol;
        private System.Windows.Forms.Label lblMessageAwol;
    }
}

