namespace quizemesterAwol
{
    partial class FormAdminAwol
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
            this.tabControlAwol = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveQuestionsAwol = new System.Windows.Forms.Button();
            this.dgvQuestionsAwol = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSaveUsersAwol = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControlAwol.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuestionsAwol)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlAwol
            // 
            this.tabControlAwol.Controls.Add(this.tabPage1);
            this.tabControlAwol.Controls.Add(this.tabPage2);
            this.tabControlAwol.Location = new System.Drawing.Point(12, 12);
            this.tabControlAwol.Name = "tabControlAwol";
            this.tabControlAwol.SelectedIndex = 0;
            this.tabControlAwol.Size = new System.Drawing.Size(776, 426);
            this.tabControlAwol.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSaveQuestionsAwol);
            this.tabPage1.Controls.Add(this.dgvQuestionsAwol);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 397);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Questions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSaveQuestionsAwol
            // 
            this.btnSaveQuestionsAwol.Location = new System.Drawing.Point(637, 6);
            this.btnSaveQuestionsAwol.Name = "btnSaveQuestionsAwol";
            this.btnSaveQuestionsAwol.Size = new System.Drawing.Size(125, 50);
            this.btnSaveQuestionsAwol.TabIndex = 1;
            this.btnSaveQuestionsAwol.Text = "Save";
            this.btnSaveQuestionsAwol.UseVisualStyleBackColor = true;
            this.btnSaveQuestionsAwol.Click += new System.EventHandler(this.btnSaveQuestionsAwol_Click);
            // 
            // dgvQuestionsAwol
            // 
            this.dgvQuestionsAwol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuestionsAwol.Location = new System.Drawing.Point(6, 6);
            this.dgvQuestionsAwol.Name = "dgvQuestionsAwol";
            this.dgvQuestionsAwol.RowHeadersWidth = 51;
            this.dgvQuestionsAwol.RowTemplate.Height = 24;
            this.dgvQuestionsAwol.Size = new System.Drawing.Size(625, 385);
            this.dgvQuestionsAwol.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSaveUsersAwol);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 397);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Users";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSaveUsersAwol
            // 
            this.btnSaveUsersAwol.Location = new System.Drawing.Point(637, 6);
            this.btnSaveUsersAwol.Name = "btnSaveUsersAwol";
            this.btnSaveUsersAwol.Size = new System.Drawing.Size(125, 50);
            this.btnSaveUsersAwol.TabIndex = 2;
            this.btnSaveUsersAwol.Text = "Save";
            this.btnSaveUsersAwol.UseVisualStyleBackColor = true;
            this.btnSaveUsersAwol.Click += new System.EventHandler(this.btnSaveUsersAwol_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(625, 385);
            this.dataGridView1.TabIndex = 1;
            // 
            // FormAdminAwol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControlAwol);
            this.Name = "FormAdminAwol";
            this.Text = "FormAdminAwol";
            this.Load += new System.EventHandler(this.FormAdminAwol_Load);
            this.tabControlAwol.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuestionsAwol)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlAwol;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvQuestionsAwol;
        private System.Windows.Forms.Button btnSaveQuestionsAwol;
        private System.Windows.Forms.Button btnSaveUsersAwol;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}