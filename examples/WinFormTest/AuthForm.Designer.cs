﻿namespace WinFormTest
{
    partial class AuthForm
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
            groupBox1 = new GroupBox();
            cbAccounts = new ComboBox();
            label1 = new Label();
            cbLoginMode = new ComboBox();
            label10 = new Label();
            cbDevicePreset = new ComboBox();
            label9 = new Label();
            groupBox3 = new GroupBox();
            txtDeviceVersion = new TextBox();
            txtDeviceType = new TextBox();
            txtXboxRelyingParty = new TextBox();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            cbXboxLoginMode = new ComboBox();
            label5 = new Label();
            groupBox2 = new GroupBox();
            txtScope = new TextBox();
            txtClientId = new TextBox();
            label4 = new Label();
            cbOAuthLoginMode = new ComboBox();
            cbAzure = new CheckBox();
            label3 = new Label();
            label2 = new Label();
            btnLogin = new Button();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbAccounts);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cbLoginMode);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(cbDevicePreset);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(15, 15);
            groupBox1.Margin = new Padding(6);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(6);
            groupBox1.Size = new Size(648, 813);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Login Setting";
            // 
            // cbAccounts
            // 
            cbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAccounts.FormattingEnabled = true;
            cbAccounts.Items.AddRange(new object[] { "MinecraftJavaEdition", "Nintendo", "iOS" });
            cbAccounts.Location = new Point(228, 166);
            cbAccounts.Margin = new Padding(6);
            cbAccounts.Name = "cbAccounts";
            cbAccounts.Size = new Size(364, 40);
            cbAccounts.TabIndex = 18;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(114, 170);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(115, 32);
            label1.TabIndex = 17;
            label1.Text = "Account: ";
            // 
            // cbLoginMode
            // 
            cbLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLoginMode.FormattingEnabled = true;
            cbLoginMode.Items.AddRange(new object[] { "JEAuthentication", "SilentJEAuthentication", "InteractiveJEAuthentication" });
            cbLoginMode.Location = new Point(228, 47);
            cbLoginMode.Margin = new Padding(6);
            cbLoginMode.Name = "cbLoginMode";
            cbLoginMode.Size = new Size(364, 40);
            cbLoginMode.TabIndex = 16;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(77, 56);
            label10.Margin = new Padding(6, 0, 6, 0);
            label10.Name = "label10";
            label10.Size = new Size(142, 32);
            label10.TabIndex = 15;
            label10.Text = "LoginMode:";
            // 
            // cbDevicePreset
            // 
            cbDevicePreset.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDevicePreset.FormattingEnabled = true;
            cbDevicePreset.Items.AddRange(new object[] { "MinecraftJavaEdition", "Nintendo", "iOS" });
            cbDevicePreset.Location = new Point(228, 109);
            cbDevicePreset.Margin = new Padding(6);
            cbDevicePreset.Name = "cbDevicePreset";
            cbDevicePreset.Size = new Size(364, 40);
            cbDevicePreset.TabIndex = 14;
            cbDevicePreset.SelectedIndexChanged += cbDevicePreset_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(63, 118);
            label9.Margin = new Padding(6, 0, 6, 0);
            label9.Name = "label9";
            label9.Size = new Size(157, 32);
            label9.TabIndex = 13;
            label9.Text = "DevicePreset:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtDeviceVersion);
            groupBox3.Controls.Add(txtDeviceType);
            groupBox3.Controls.Add(txtXboxRelyingParty);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(cbXboxLoginMode);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(12, 492);
            groupBox3.Margin = new Padding(6);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(6);
            groupBox3.Size = new Size(608, 301);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "XboxAuth";
            // 
            // txtDeviceVersion
            // 
            txtDeviceVersion.Location = new Point(216, 233);
            txtDeviceVersion.Margin = new Padding(6);
            txtDeviceVersion.Name = "txtDeviceVersion";
            txtDeviceVersion.Size = new Size(364, 39);
            txtDeviceVersion.TabIndex = 12;
            // 
            // txtDeviceType
            // 
            txtDeviceType.Location = new Point(216, 171);
            txtDeviceType.Margin = new Padding(6);
            txtDeviceType.Name = "txtDeviceType";
            txtDeviceType.Size = new Size(364, 39);
            txtDeviceType.TabIndex = 11;
            // 
            // txtXboxRelyingParty
            // 
            txtXboxRelyingParty.Location = new Point(216, 109);
            txtXboxRelyingParty.Margin = new Padding(6);
            txtXboxRelyingParty.Name = "txtXboxRelyingParty";
            txtXboxRelyingParty.Size = new Size(364, 39);
            txtXboxRelyingParty.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(65, 180);
            label8.Margin = new Padding(6, 0, 6, 0);
            label8.Name = "label8";
            label8.Size = new Size(143, 32);
            label8.TabIndex = 10;
            label8.Text = "DeviceType:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(35, 242);
            label7.Margin = new Padding(6, 0, 6, 0);
            label7.Name = "label7";
            label7.Size = new Size(171, 32);
            label7.TabIndex = 9;
            label7.Text = "DeviceVersion:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(55, 118);
            label6.Margin = new Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Size = new Size(153, 32);
            label6.TabIndex = 8;
            label6.Text = "RelyingParty:";
            // 
            // cbXboxLoginMode
            // 
            cbXboxLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbXboxLoginMode.FormattingEnabled = true;
            cbXboxLoginMode.Items.AddRange(new object[] { "Basic", "Full", "Sisu" });
            cbXboxLoginMode.Location = new Point(216, 47);
            cbXboxLoginMode.Margin = new Padding(6);
            cbXboxLoginMode.Name = "cbXboxLoginMode";
            cbXboxLoginMode.Size = new Size(364, 40);
            cbXboxLoginMode.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(57, 56);
            label5.Margin = new Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Size = new Size(150, 32);
            label5.TabIndex = 7;
            label5.Text = "Login Mode:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtScope);
            groupBox2.Controls.Add(txtClientId);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(cbOAuthLoginMode);
            groupBox2.Controls.Add(cbAzure);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(12, 221);
            groupBox2.Margin = new Padding(6);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(6);
            groupBox2.Size = new Size(608, 254);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "OAuth";
            // 
            // txtScope
            // 
            txtScope.Location = new Point(216, 173);
            txtScope.Margin = new Padding(6);
            txtScope.Name = "txtScope";
            txtScope.Size = new Size(238, 39);
            txtScope.TabIndex = 6;
            // 
            // txtClientId
            // 
            txtClientId.Location = new Point(216, 111);
            txtClientId.Margin = new Padding(6);
            txtClientId.Name = "txtClientId";
            txtClientId.Size = new Size(364, 39);
            txtClientId.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(49, 58);
            label4.Margin = new Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new Size(158, 32);
            label4.TabIndex = 4;
            label4.Text = "Login Mode: ";
            // 
            // cbOAuthLoginMode
            // 
            cbOAuthLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOAuthLoginMode.FormattingEnabled = true;
            cbOAuthLoginMode.Items.AddRange(new object[] { "(Default)", "InteractiveMicrosoftOAuth", "SilentMicrosoftOAuth", "InteractiveMsal", "DeviceCodeMsal", "SilentMsal" });
            cbOAuthLoginMode.Location = new Point(216, 49);
            cbOAuthLoginMode.Margin = new Padding(6);
            cbOAuthLoginMode.Name = "cbOAuthLoginMode";
            cbOAuthLoginMode.Size = new Size(364, 40);
            cbOAuthLoginMode.TabIndex = 3;
            cbOAuthLoginMode.SelectedIndexChanged += cbOAuthLoginMode_SelectedIndexChanged;
            // 
            // cbAzure
            // 
            cbAzure.AutoSize = true;
            cbAzure.Location = new Point(473, 182);
            cbAzure.Margin = new Padding(6);
            cbAzure.Name = "cbAzure";
            cbAzure.Size = new Size(108, 36);
            cbAzure.TabIndex = 2;
            cbAzure.Text = "Azure";
            cbAzure.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(105, 182);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 32);
            label3.TabIndex = 1;
            label3.Text = "Scope : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(85, 120);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(120, 32);
            label2.TabIndex = 0;
            label2.Text = "ClientID : ";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(15, 856);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(648, 84);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 952);
            Controls.Add(btnLogin);
            Controls.Add(groupBox1);
            Name = "AuthForm";
            Text = "AuthForm";
            Load += AuthForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbLoginMode;
        private Label label10;
        private ComboBox cbDevicePreset;
        private Label label9;
        private GroupBox groupBox3;
        private TextBox txtDeviceVersion;
        private TextBox txtDeviceType;
        private TextBox txtXboxRelyingParty;
        private Label label8;
        private Label label7;
        private Label label6;
        private ComboBox cbXboxLoginMode;
        private Label label5;
        private GroupBox groupBox2;
        private TextBox txtScope;
        private TextBox txtClientId;
        private Label label4;
        private ComboBox cbOAuthLoginMode;
        private CheckBox cbAzure;
        private Label label3;
        private Label label2;
        private ComboBox cbAccounts;
        private Label label1;
        private Button btnLogin;
    }
}