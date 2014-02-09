namespace tibbrExplorer
{
    partial class tibbrExplorerLogin
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
            this.tbClassMain = new System.Windows.Forms.TabControl();
            this.tbPageLogin = new System.Windows.Forms.TabPage();
            this.gboxLogin = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtClientKey = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblClientKey = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.cboEnvironment = new System.Windows.Forms.ComboBox();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.tbClassMain.SuspendLayout();
            this.tbPageLogin.SuspendLayout();
            this.gboxLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbClassMain
            // 
            this.tbClassMain.Controls.Add(this.tbPageLogin);
            this.tbClassMain.Location = new System.Drawing.Point(0, 0);
            this.tbClassMain.Name = "tbClassMain";
            this.tbClassMain.SelectedIndex = 0;
            this.tbClassMain.Size = new System.Drawing.Size(547, 483);
            this.tbClassMain.TabIndex = 0;
            // 
            // tbPageLogin
            // 
            this.tbPageLogin.Controls.Add(this.gboxLogin);
            this.tbPageLogin.Location = new System.Drawing.Point(4, 22);
            this.tbPageLogin.Name = "tbPageLogin";
            this.tbPageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tbPageLogin.Size = new System.Drawing.Size(539, 457);
            this.tbPageLogin.TabIndex = 0;
            this.tbPageLogin.Text = "Login";
            this.tbPageLogin.UseVisualStyleBackColor = true;
            // 
            // gboxLogin
            // 
            this.gboxLogin.Controls.Add(this.btnLogin);
            this.gboxLogin.Controls.Add(this.txtClientKey);
            this.gboxLogin.Controls.Add(this.txtPassword);
            this.gboxLogin.Controls.Add(this.txtUsername);
            this.gboxLogin.Controls.Add(this.lblClientKey);
            this.gboxLogin.Controls.Add(this.lblPassword);
            this.gboxLogin.Controls.Add(this.lblUsername);
            this.gboxLogin.Controls.Add(this.cboEnvironment);
            this.gboxLogin.Controls.Add(this.lblEnvironment);
            this.gboxLogin.Location = new System.Drawing.Point(31, 28);
            this.gboxLogin.Name = "gboxLogin";
            this.gboxLogin.Size = new System.Drawing.Size(457, 340);
            this.gboxLogin.TabIndex = 0;
            this.gboxLogin.TabStop = false;
            this.gboxLogin.Text = "Please enter your login credentials";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(215, 278);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 9;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtClientKey
            // 
            this.txtClientKey.Location = new System.Drawing.Point(139, 207);
            this.txtClientKey.Name = "txtClientKey";
            this.txtClientKey.Size = new System.Drawing.Size(276, 20);
            this.txtClientKey.TabIndex = 8;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(139, 160);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(276, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(139, 111);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(276, 20);
            this.txtUsername.TabIndex = 6;
            // 
            // lblClientKey
            // 
            this.lblClientKey.AutoSize = true;
            this.lblClientKey.Location = new System.Drawing.Point(47, 210);
            this.lblClientKey.Name = "lblClientKey";
            this.lblClientKey.Size = new System.Drawing.Size(54, 13);
            this.lblClientKey.TabIndex = 5;
            this.lblClientKey.Text = "Client Key";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(47, 163);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(47, 114);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            // 
            // cboEnvironment
            // 
            this.cboEnvironment.FormattingEnabled = true;
            this.cboEnvironment.Location = new System.Drawing.Point(139, 60);
            this.cboEnvironment.Name = "cboEnvironment";
            this.cboEnvironment.Size = new System.Drawing.Size(276, 21);
            this.cboEnvironment.TabIndex = 2;
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(47, 62);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(66, 13);
            this.lblEnvironment.TabIndex = 1;
            this.lblEnvironment.Text = "Environment";
            // 
            // tibbrExplorerLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 425);
            this.Controls.Add(this.tbClassMain);
            this.Name = "tibbrExplorerLogin";
            this.Text = "Welcome to tibbrExplorer";
            this.Load += new System.EventHandler(this.tibbrExplorerLogin_Load);
            this.tbClassMain.ResumeLayout(false);
            this.tbPageLogin.ResumeLayout(false);
            this.gboxLogin.ResumeLayout(false);
            this.gboxLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbClassMain;
        private System.Windows.Forms.TabPage tbPageLogin;
        private System.Windows.Forms.GroupBox gboxLogin;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.ComboBox cboEnvironment;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblClientKey;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtClientKey;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
    }
}