namespace MsalClientTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnLoginInteractiveEmb = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAccessToken = new System.Windows.Forms.TextBox();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbFileName = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.btnCancelLogin = new System.Windows.Forms.Button();
            this.btnLoginDeviceCode = new System.Windows.Forms.Button();
            this.btnLoginInteractive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status: ";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbStatus.Location = new System.Drawing.Point(78, 17);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(27, 25);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "...";
            // 
            // btnLoginInteractiveEmb
            // 
            this.btnLoginInteractiveEmb.Location = new System.Drawing.Point(199, 64);
            this.btnLoginInteractiveEmb.Name = "btnLoginInteractiveEmb";
            this.btnLoginInteractiveEmb.Size = new System.Drawing.Size(157, 77);
            this.btnLoginInteractiveEmb.TabIndex = 2;
            this.btnLoginInteractiveEmb.Text = "Login\r\nInteractive\r\n(embeded)";
            this.btnLoginInteractiveEmb.UseVisualStyleBackColor = true;
            this.btnLoginInteractiveEmb.Click += new System.EventHandler(this.btnLoginInteractiveEmb_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(37, 147);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(354, 29);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "AccessToken: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "UUID: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 272);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Username: ";
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.Location = new System.Drawing.Point(168, 196);
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.ReadOnly = true;
            this.txtAccessToken.Size = new System.Drawing.Size(341, 27);
            this.txtAccessToken.TabIndex = 7;
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(168, 233);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.ReadOnly = true;
            this.txtUUID.Size = new System.Drawing.Size(341, 27);
            this.txtUUID.TabIndex = 8;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(168, 269);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.ReadOnly = true;
            this.txtUsername.Size = new System.Drawing.Size(341, 27);
            this.txtUsername.TabIndex = 9;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(168, 318);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(341, 29);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Game Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbFileName
            // 
            this.lbFileName.AutoSize = true;
            this.lbFileName.Location = new System.Drawing.Point(46, 352);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(66, 20);
            this.lbFileName.TabIndex = 11;
            this.lbFileName.Text = "progress";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(46, 385);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(481, 29);
            this.progressBar1.TabIndex = 12;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(46, 420);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(481, 29);
            this.progressBar2.TabIndex = 13;
            // 
            // btnCancelLogin
            // 
            this.btnCancelLogin.Location = new System.Drawing.Point(397, 147);
            this.btnCancelLogin.Name = "btnCancelLogin";
            this.btnCancelLogin.Size = new System.Drawing.Size(124, 29);
            this.btnCancelLogin.TabIndex = 14;
            this.btnCancelLogin.Text = "Cancel login";
            this.btnCancelLogin.UseVisualStyleBackColor = true;
            this.btnCancelLogin.Click += new System.EventHandler(this.btnCancelLogin_Click);
            // 
            // btnLoginDeviceCode
            // 
            this.btnLoginDeviceCode.Location = new System.Drawing.Point(361, 64);
            this.btnLoginDeviceCode.Name = "btnLoginDeviceCode";
            this.btnLoginDeviceCode.Size = new System.Drawing.Size(160, 77);
            this.btnLoginDeviceCode.TabIndex = 16;
            this.btnLoginDeviceCode.Text = "Login\r\nDevice Code";
            this.btnLoginDeviceCode.UseVisualStyleBackColor = true;
            this.btnLoginDeviceCode.Click += new System.EventHandler(this.btnLoginDeviceCode_Click);
            // 
            // btnLoginInteractive
            // 
            this.btnLoginInteractive.Location = new System.Drawing.Point(37, 64);
            this.btnLoginInteractive.Name = "btnLoginInteractive";
            this.btnLoginInteractive.Size = new System.Drawing.Size(157, 77);
            this.btnLoginInteractive.TabIndex = 17;
            this.btnLoginInteractive.Text = "Login\r\nInteractive";
            this.btnLoginInteractive.UseVisualStyleBackColor = true;
            this.btnLoginInteractive.Click += new System.EventHandler(this.btnLoginInteractive_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 465);
            this.Controls.Add(this.btnLoginInteractive);
            this.Controls.Add(this.btnLoginDeviceCode);
            this.Controls.Add(this.btnCancelLogin);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbFileName);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.txtAccessToken);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnLoginInteractiveEmb);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label lbStatus;
        private Button btnLoginInteractiveEmb;
        private Button btnLogout;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtAccessToken;
        private TextBox txtUUID;
        private TextBox txtUsername;
        private Button btnStart;
        private Label lbFileName;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private Button btnCancelLogin;
        private Button btnLoginDeviceCode;
        private Button btnLoginInteractive;
    }
}