namespace WinFormTest
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
            groupBox4 = new GroupBox();
            cbJEGameOwnershipChecker = new CheckBox();
            cbJEValidatation = new CheckBox();
            cbLoginPreset = new ComboBox();
            label10 = new Label();
            cbAccounts = new ComboBox();
            label1 = new Label();
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
            cbOAuthValidation = new CheckBox();
            txtScope = new TextBox();
            txtClientId = new TextBox();
            label4 = new Label();
            cbOAuthLoginMode = new ComboBox();
            cbAzure = new CheckBox();
            label3 = new Label();
            label2 = new Label();
            btnLogin = new Button();
            groupBox1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(cbLoginPreset);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(cbAccounts);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cbDevicePreset);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(8, 7);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(324, 472);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Login Setting";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(cbJEGameOwnershipChecker);
            groupBox4.Controls.Add(cbJEValidatation);
            groupBox4.Location = new Point(6, 376);
            groupBox4.Margin = new Padding(2, 1, 2, 1);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(2, 1, 2, 1);
            groupBox4.Size = new Size(304, 94);
            groupBox4.TabIndex = 21;
            groupBox4.TabStop = false;
            groupBox4.Text = "JEAuth";
            // 
            // cbJEGameOwnershipChecker
            // 
            cbJEGameOwnershipChecker.AutoSize = true;
            cbJEGameOwnershipChecker.Location = new Point(42, 45);
            cbJEGameOwnershipChecker.Margin = new Padding(2, 1, 2, 1);
            cbJEGameOwnershipChecker.Name = "cbJEGameOwnershipChecker";
            cbJEGameOwnershipChecker.Size = new Size(180, 19);
            cbJEGameOwnershipChecker.TabIndex = 10;
            cbJEGameOwnershipChecker.Text = "Use GameOwnershipChecker";
            cbJEGameOwnershipChecker.UseVisualStyleBackColor = true;
            // 
            // cbJEValidatation
            // 
            cbJEValidatation.AutoSize = true;
            cbJEValidatation.Location = new Point(42, 25);
            cbJEValidatation.Margin = new Padding(2, 1, 2, 1);
            cbJEValidatation.Name = "cbJEValidatation";
            cbJEValidatation.Size = new Size(230, 19);
            cbJEValidatation.TabIndex = 9;
            cbJEValidatation.Text = "Validate session before authentication";
            cbJEValidatation.UseVisualStyleBackColor = true;
            // 
            // cbLoginPreset
            // 
            cbLoginPreset.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLoginPreset.FormattingEnabled = true;
            cbLoginPreset.Items.AddRange(new object[] { "Interactive (for adding new account)", "Silent (for authentication with cached account)" });
            cbLoginPreset.Location = new Point(114, 14);
            cbLoginPreset.Name = "cbLoginPreset";
            cbLoginPreset.Size = new Size(184, 23);
            cbLoginPreset.TabIndex = 20;
            cbLoginPreset.SelectedIndexChanged += cbLoginPreset_SelectedIndexChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(32, 18);
            label10.Name = "label10";
            label10.Size = new Size(80, 15);
            label10.TabIndex = 19;
            label10.Text = "Login Preset: ";
            // 
            // cbAccounts
            // 
            cbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAccounts.FormattingEnabled = true;
            cbAccounts.Items.AddRange(new object[] { "MinecraftJavaEdition", "Nintendo", "iOS" });
            cbAccounts.Location = new Point(114, 65);
            cbAccounts.Name = "cbAccounts";
            cbAccounts.Size = new Size(184, 23);
            cbAccounts.TabIndex = 18;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 67);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 17;
            label1.Text = "Account: ";
            // 
            // cbDevicePreset
            // 
            cbDevicePreset.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDevicePreset.FormattingEnabled = true;
            cbDevicePreset.Items.AddRange(new object[] { "MinecraftJavaEdition", "Nintendo", "iOS" });
            cbDevicePreset.Location = new Point(114, 38);
            cbDevicePreset.Name = "cbDevicePreset";
            cbDevicePreset.Size = new Size(184, 23);
            cbDevicePreset.TabIndex = 14;
            cbDevicePreset.SelectedIndexChanged += cbDevicePreset_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(32, 43);
            label9.Name = "label9";
            label9.Size = new Size(78, 15);
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
            groupBox3.Location = new Point(6, 231);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(304, 141);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "XboxAuth";
            // 
            // txtDeviceVersion
            // 
            txtDeviceVersion.Location = new Point(108, 109);
            txtDeviceVersion.Name = "txtDeviceVersion";
            txtDeviceVersion.Size = new Size(184, 23);
            txtDeviceVersion.TabIndex = 12;
            // 
            // txtDeviceType
            // 
            txtDeviceType.Location = new Point(108, 80);
            txtDeviceType.Name = "txtDeviceType";
            txtDeviceType.Size = new Size(184, 23);
            txtDeviceType.TabIndex = 11;
            // 
            // txtXboxRelyingParty
            // 
            txtXboxRelyingParty.Location = new Point(108, 51);
            txtXboxRelyingParty.Name = "txtXboxRelyingParty";
            txtXboxRelyingParty.Size = new Size(184, 23);
            txtXboxRelyingParty.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(32, 84);
            label8.Name = "label8";
            label8.Size = new Size(71, 15);
            label8.TabIndex = 10;
            label8.Text = "DeviceType:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 113);
            label7.Name = "label7";
            label7.Size = new Size(86, 15);
            label7.TabIndex = 9;
            label7.Text = "DeviceVersion:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(28, 55);
            label6.Name = "label6";
            label6.Size = new Size(76, 15);
            label6.TabIndex = 8;
            label6.Text = "RelyingParty:";
            // 
            // cbXboxLoginMode
            // 
            cbXboxLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbXboxLoginMode.FormattingEnabled = true;
            cbXboxLoginMode.Items.AddRange(new object[] { "Basic", "Full", "Sisu" });
            cbXboxLoginMode.Location = new Point(108, 22);
            cbXboxLoginMode.Name = "cbXboxLoginMode";
            cbXboxLoginMode.Size = new Size(184, 23);
            cbXboxLoginMode.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(28, 26);
            label5.Name = "label5";
            label5.Size = new Size(75, 15);
            label5.TabIndex = 7;
            label5.Text = "Login Mode:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cbOAuthValidation);
            groupBox2.Controls.Add(txtScope);
            groupBox2.Controls.Add(txtClientId);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(cbOAuthLoginMode);
            groupBox2.Controls.Add(cbAzure);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(6, 88);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(304, 135);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "OAuth";
            // 
            // cbOAuthValidation
            // 
            cbOAuthValidation.AutoSize = true;
            cbOAuthValidation.Location = new Point(46, 22);
            cbOAuthValidation.Margin = new Padding(2, 1, 2, 1);
            cbOAuthValidation.Name = "cbOAuthValidation";
            cbOAuthValidation.Size = new Size(230, 19);
            cbOAuthValidation.TabIndex = 7;
            cbOAuthValidation.Text = "Validate session before authentication";
            cbOAuthValidation.UseVisualStyleBackColor = true;
            // 
            // txtScope
            // 
            txtScope.Location = new Point(108, 101);
            txtScope.Name = "txtScope";
            txtScope.Size = new Size(121, 23);
            txtScope.TabIndex = 6;
            // 
            // txtClientId
            // 
            txtClientId.Location = new Point(108, 72);
            txtClientId.Name = "txtClientId";
            txtClientId.Size = new Size(184, 23);
            txtClientId.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 47);
            label4.Name = "label4";
            label4.Size = new Size(79, 15);
            label4.TabIndex = 4;
            label4.Text = "Login Mode: ";
            // 
            // cbOAuthLoginMode
            // 
            cbOAuthLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOAuthLoginMode.FormattingEnabled = true;
            cbOAuthLoginMode.Items.AddRange(new object[] { "InteractiveMicrosoftOAuth", "SilentMicrosoftOAuth", "InteractiveMsal", "DeviceCodeMsal", "SilentMsal" });
            cbOAuthLoginMode.Location = new Point(108, 43);
            cbOAuthLoginMode.Name = "cbOAuthLoginMode";
            cbOAuthLoginMode.Size = new Size(184, 23);
            cbOAuthLoginMode.TabIndex = 3;
            cbOAuthLoginMode.SelectedIndexChanged += cbOAuthLoginMode_SelectedIndexChanged;
            // 
            // cbAzure
            // 
            cbAzure.AutoSize = true;
            cbAzure.Location = new Point(236, 105);
            cbAzure.Name = "cbAzure";
            cbAzure.Size = new Size(57, 19);
            cbAzure.TabIndex = 2;
            cbAzure.Text = "Azure";
            cbAzure.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(52, 105);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 1;
            label3.Text = "Scope : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 76);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 0;
            label2.Text = "ClientID : ";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(8, 484);
            btnLogin.Margin = new Padding(2, 1, 2, 1);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(324, 39);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 534);
            Controls.Add(btnLogin);
            Controls.Add(groupBox1);
            Margin = new Padding(2, 1, 2, 1);
            Name = "AuthForm";
            Text = "AuthForm";
            Load += AuthForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
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
        private CheckBox cbOAuthValidation;
        private ComboBox cbLoginPreset;
        private Label label10;
        private GroupBox groupBox4;
        private CheckBox cbJEGameOwnershipChecker;
        private CheckBox cbJEValidatation;
    }
}