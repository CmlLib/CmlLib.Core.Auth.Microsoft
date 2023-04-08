
namespace WinFormTest
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAccessToken = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbPresets = new System.Windows.Forms.ComboBox();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.txtDeviceType = new System.Windows.Forms.TextBox();
            this.txtDeviceVersion = new System.Windows.Forms.TextBox();
            this.cbSisuAuth = new System.Windows.Forms.CheckBox();
            this.cbAzure = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 80);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(498, 69);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(12, 155);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(498, 69);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "AccessToken : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Username : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "UUID : ";
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.Location = new System.Drawing.Point(115, 243);
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.ReadOnly = true;
            this.txtAccessToken.Size = new System.Drawing.Size(396, 23);
            this.txtAccessToken.TabIndex = 5;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(115, 275);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.ReadOnly = true;
            this.txtUsername.Size = new System.Drawing.Size(396, 23);
            this.txtUsername.TabIndex = 6;
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(115, 304);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.ReadOnly = true;
            this.txtUUID.Size = new System.Drawing.Size(396, 23);
            this.txtUUID.TabIndex = 7;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 347);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(498, 63);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start game";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 416);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(498, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(12, 445);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(498, 23);
            this.progressBar2.TabIndex = 10;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(12, 478);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(39, 15);
            this.lbStatus.TabIndex = 11;
            this.lbStatus.Text = "label4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "ClientID :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(293, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "preset: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "DeviceType :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(199, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "DeviceVersion :";
            // 
            // cbPresets
            // 
            this.cbPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPresets.FormattingEnabled = true;
            this.cbPresets.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbPresets.Items.AddRange(new object[] {
            "Win32",
            "Nintendo",
            "iOS"});
            this.cbPresets.Location = new System.Drawing.Point(335, 19);
            this.cbPresets.Name = "cbPresets";
            this.cbPresets.Size = new System.Drawing.Size(92, 23);
            this.cbPresets.TabIndex = 16;
            this.cbPresets.SelectedIndexChanged += new System.EventHandler(this.cbPresets_SelectedIndexChanged);
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(75, 19);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(214, 23);
            this.txtClientId.TabIndex = 17;
            // 
            // txtDeviceType
            // 
            this.txtDeviceType.Location = new System.Drawing.Point(93, 51);
            this.txtDeviceType.Name = "txtDeviceType";
            this.txtDeviceType.Size = new System.Drawing.Size(100, 23);
            this.txtDeviceType.TabIndex = 18;
            // 
            // txtDeviceVersion
            // 
            this.txtDeviceVersion.Location = new System.Drawing.Point(295, 48);
            this.txtDeviceVersion.Name = "txtDeviceVersion";
            this.txtDeviceVersion.Size = new System.Drawing.Size(132, 23);
            this.txtDeviceVersion.TabIndex = 19;
            // 
            // cbSisuAuth
            // 
            this.cbSisuAuth.AutoSize = true;
            this.cbSisuAuth.Location = new System.Drawing.Point(433, 50);
            this.cbSisuAuth.Name = "cbSisuAuth";
            this.cbSisuAuth.Size = new System.Drawing.Size(74, 19);
            this.cbSisuAuth.TabIndex = 20;
            this.cbSisuAuth.Text = "SisuAuth";
            this.cbSisuAuth.UseVisualStyleBackColor = true;
            // 
            // cbAzure
            // 
            this.cbAzure.AutoSize = true;
            this.cbAzure.Location = new System.Drawing.Point(433, 23);
            this.cbAzure.Name = "cbAzure";
            this.cbAzure.Size = new System.Drawing.Size(57, 19);
            this.cbAzure.TabIndex = 21;
            this.cbAzure.Text = "Azure";
            this.cbAzure.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 537);
            this.Controls.Add(this.cbAzure);
            this.Controls.Add(this.cbSisuAuth);
            this.Controls.Add(this.txtDeviceVersion);
            this.Controls.Add(this.txtDeviceType);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.cbPresets);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtAccessToken);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnLogin);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAccessToken;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbPresets;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.TextBox txtDeviceType;
        private System.Windows.Forms.TextBox txtDeviceVersion;
        private System.Windows.Forms.CheckBox cbSisuAuth;
        private System.Windows.Forms.CheckBox cbAzure;
    }
}

