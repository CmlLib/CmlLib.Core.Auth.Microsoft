namespace WinFormTest
{
    partial class AccountsForm
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
            label1 = new Label();
            lbAccounts = new ListBox();
            btnLogin = new Button();
            pgAccount = new PropertyGrid();
            groupBox1 = new GroupBox();
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
            groupBox4 = new GroupBox();
            txtAccountFilePath = new TextBox();
            label11 = new Label();
            btnClearAccount = new Button();
            btnAddAccount = new Button();
            btnRemoveAccount = new Button();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(217, 25);
            label1.TabIndex = 0;
            label1.Text = "Select account to login:";
            // 
            // lbAccounts
            // 
            lbAccounts.FormattingEnabled = true;
            lbAccounts.ItemHeight = 15;
            lbAccounts.Location = new Point(12, 50);
            lbAccounts.Name = "lbAccounts";
            lbAccounts.Size = new Size(425, 154);
            lbAccounts.TabIndex = 1;
            lbAccounts.SelectedIndexChanged += lbAccounts_SelectedIndexChanged;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(12, 540);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(425, 28);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            // 
            // pgAccount
            // 
            pgAccount.BackColor = Color.Silver;
            pgAccount.Location = new Point(12, 210);
            pgAccount.Name = "pgAccount";
            pgAccount.Size = new Size(425, 266);
            pgAccount.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbLoginMode);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(cbDevicePreset);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(443, 214);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(324, 354);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Login Setting";
            // 
            // cbLoginMode
            // 
            cbLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLoginMode.FormattingEnabled = true;
            cbLoginMode.Items.AddRange(new object[] { "JEAuthentication", "SilentJEAuthentication", "InteractiveJEAuthentication" });
            cbLoginMode.Location = new Point(114, 22);
            cbLoginMode.Name = "cbLoginMode";
            cbLoginMode.Size = new Size(184, 23);
            cbLoginMode.TabIndex = 16;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(37, 25);
            label10.Name = "label10";
            label10.Size = new Size(71, 15);
            label10.TabIndex = 15;
            label10.Text = "LoginMode:";
            // 
            // cbDevicePreset
            // 
            cbDevicePreset.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDevicePreset.FormattingEnabled = true;
            cbDevicePreset.Items.AddRange(new object[] { "MinecraftJavaEdition", "Nintendo", "iOS" });
            cbDevicePreset.Location = new Point(114, 51);
            cbDevicePreset.Name = "cbDevicePreset";
            cbDevicePreset.Size = new Size(184, 23);
            cbDevicePreset.TabIndex = 14;
            cbDevicePreset.SelectedIndexChanged += cbDevicePreset_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(30, 54);
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
            groupBox3.Location = new Point(6, 205);
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
            label8.Location = new Point(31, 83);
            label8.Name = "label8";
            label8.Size = new Size(71, 15);
            label8.TabIndex = 10;
            label8.Text = "DeviceType:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 112);
            label7.Name = "label7";
            label7.Size = new Size(86, 15);
            label7.TabIndex = 9;
            label7.Text = "DeviceVersion:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(26, 54);
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
            label5.Location = new Point(27, 25);
            label5.Name = "label5";
            label5.Size = new Size(75, 15);
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
            groupBox2.Location = new Point(6, 80);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(304, 119);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "OAuth";
            // 
            // txtScope
            // 
            txtScope.Location = new Point(108, 81);
            txtScope.Name = "txtScope";
            txtScope.Size = new Size(121, 23);
            txtScope.TabIndex = 6;
            // 
            // txtClientId
            // 
            txtClientId.Location = new Point(108, 52);
            txtClientId.Name = "txtClientId";
            txtClientId.Size = new Size(184, 23);
            txtClientId.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 26);
            label4.Name = "label4";
            label4.Size = new Size(79, 15);
            label4.TabIndex = 4;
            label4.Text = "Login Mode: ";
            // 
            // cbOAuthLoginMode
            // 
            cbOAuthLoginMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOAuthLoginMode.FormattingEnabled = true;
            cbOAuthLoginMode.Items.AddRange(new object[] { "(Default)", "InteractiveMicrosoftOAuth", "SilentMicrosoftOAuth", "InteractiveMsal", "DeviceCodeMsal", "SilentMsal" });
            cbOAuthLoginMode.Location = new Point(108, 23);
            cbOAuthLoginMode.Name = "cbOAuthLoginMode";
            cbOAuthLoginMode.Size = new Size(184, 23);
            cbOAuthLoginMode.TabIndex = 3;
            cbOAuthLoginMode.SelectedIndexChanged += cbOAuthLoginMode_SelectedIndexChanged;
            // 
            // cbAzure
            // 
            cbAzure.AutoSize = true;
            cbAzure.Location = new Point(235, 84);
            cbAzure.Name = "cbAzure";
            cbAzure.Size = new Size(57, 19);
            cbAzure.TabIndex = 2;
            cbAzure.Text = "Azure";
            cbAzure.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(51, 84);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 1;
            label3.Text = "Scope : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 55);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 0;
            label2.Text = "ClientID : ";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txtAccountFilePath);
            groupBox4.Controls.Add(label11);
            groupBox4.Controls.Add(btnClearAccount);
            groupBox4.Location = new Point(443, 50);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(324, 158);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "AccountManager";
            // 
            // txtAccountFilePath
            // 
            txtAccountFilePath.Location = new Point(114, 38);
            txtAccountFilePath.Name = "txtAccountFilePath";
            txtAccountFilePath.Size = new Size(184, 23);
            txtAccountFilePath.TabIndex = 8;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(11, 41);
            label11.Name = "label11";
            label11.Size = new Size(97, 15);
            label11.TabIndex = 7;
            label11.Text = "AccountFilePath:";
            // 
            // btnClearAccount
            // 
            btnClearAccount.Location = new Point(47, 97);
            btnClearAccount.Name = "btnClearAccount";
            btnClearAccount.Size = new Size(217, 23);
            btnClearAccount.TabIndex = 6;
            btnClearAccount.Text = "ClearAccount";
            btnClearAccount.UseVisualStyleBackColor = true;
            // 
            // btnAddAccount
            // 
            btnAddAccount.Location = new Point(12, 482);
            btnAddAccount.Name = "btnAddAccount";
            btnAddAccount.Size = new Size(425, 23);
            btnAddAccount.TabIndex = 2;
            btnAddAccount.Text = "AddAccount";
            btnAddAccount.UseVisualStyleBackColor = true;
            btnAddAccount.Click += btnAddAccount_Click;
            // 
            // btnRemoveAccount
            // 
            btnRemoveAccount.Location = new Point(12, 511);
            btnRemoveAccount.Name = "btnRemoveAccount";
            btnRemoveAccount.Size = new Size(425, 23);
            btnRemoveAccount.TabIndex = 7;
            btnRemoveAccount.Text = "RemoveAccount";
            btnRemoveAccount.UseVisualStyleBackColor = true;
            // 
            // AccountsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(781, 577);
            Controls.Add(btnRemoveAccount);
            Controls.Add(btnAddAccount);
            Controls.Add(groupBox4);
            Controls.Add(groupBox1);
            Controls.Add(pgAccount);
            Controls.Add(btnLogin);
            Controls.Add(lbAccounts);
            Controls.Add(label1);
            Name = "AccountsForm";
            Text = "AccountsForm";
            Load += AccountsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox lbAccounts;
        private Button btnLogin;
        private PropertyGrid pgAccount;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private CheckBox cbAzure;
        private Label label3;
        private Label label2;
        private GroupBox groupBox3;
        private Label label5;
        private TextBox txtScope;
        private TextBox txtClientId;
        private Label label4;
        private ComboBox cbOAuthLoginMode;
        private ComboBox cbXboxLoginMode;
        private ComboBox cbDevicePreset;
        private Label label9;
        private TextBox txtDeviceVersion;
        private TextBox txtDeviceType;
        private TextBox txtXboxRelyingParty;
        private Label label8;
        private Label label7;
        private Label label6;
        private ComboBox cbLoginMode;
        private Label label10;
        private GroupBox groupBox4;
        private TextBox txtAccountFilePath;
        private Label label11;
        private Button btnClearAccount;
        private Button btnAddAccount;
        private Button btnRemoveAccount;
    }
}