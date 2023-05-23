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
            btnAddAccount = new Button();
            btnRemoveAccount = new Button();
            btnAdvOptions = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(24, 19);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(428, 51);
            label1.TabIndex = 0;
            label1.Text = "Select account to login:";
            // 
            // lbAccounts
            // 
            lbAccounts.FormattingEnabled = true;
            lbAccounts.ItemHeight = 32;
            lbAccounts.Location = new Point(24, 107);
            lbAccounts.Margin = new Padding(6);
            lbAccounts.Name = "lbAccounts";
            lbAccounts.Size = new Size(846, 324);
            lbAccounts.TabIndex = 1;
            lbAccounts.SelectedIndexChanged += lbAccounts_SelectedIndexChanged;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(24, 1232);
            btnLogin.Margin = new Padding(6);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(850, 60);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // pgAccount
            // 
            pgAccount.BackColor = Color.Silver;
            pgAccount.Location = new Point(24, 448);
            pgAccount.Margin = new Padding(6);
            pgAccount.Name = "pgAccount";
            pgAccount.Size = new Size(850, 567);
            pgAccount.TabIndex = 3;
            // 
            // btnAddAccount
            // 
            btnAddAccount.Location = new Point(24, 1028);
            btnAddAccount.Margin = new Padding(6);
            btnAddAccount.Name = "btnAddAccount";
            btnAddAccount.Size = new Size(850, 49);
            btnAddAccount.TabIndex = 2;
            btnAddAccount.Text = "AddAccount";
            btnAddAccount.UseVisualStyleBackColor = true;
            btnAddAccount.Click += btnAddAccount_Click;
            // 
            // btnRemoveAccount
            // 
            btnRemoveAccount.Location = new Point(24, 1090);
            btnRemoveAccount.Margin = new Padding(6);
            btnRemoveAccount.Name = "btnRemoveAccount";
            btnRemoveAccount.Size = new Size(850, 49);
            btnRemoveAccount.TabIndex = 7;
            btnRemoveAccount.Text = "RemoveAccount";
            btnRemoveAccount.UseVisualStyleBackColor = true;
            btnRemoveAccount.Click += btnRemoveAccount_Click;
            // 
            // btnAdvOptions
            // 
            btnAdvOptions.Location = new Point(23, 1160);
            btnAdvOptions.Margin = new Padding(6);
            btnAdvOptions.Name = "btnAdvOptions";
            btnAdvOptions.Size = new Size(850, 60);
            btnAdvOptions.TabIndex = 8;
            btnAdvOptions.Text = "Advanced Options";
            btnAdvOptions.UseVisualStyleBackColor = true;
            btnAdvOptions.Click += btnAdvOptions_Click;
            // 
            // AccountsForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 1329);
            Controls.Add(btnAdvOptions);
            Controls.Add(btnRemoveAccount);
            Controls.Add(btnAddAccount);
            Controls.Add(pgAccount);
            Controls.Add(btnLogin);
            Controls.Add(lbAccounts);
            Controls.Add(label1);
            Margin = new Padding(6);
            Name = "AccountsForm";
            Text = "AccountsForm";
            Load += AccountsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox lbAccounts;
        private Button btnLogin;
        private PropertyGrid pgAccount;
        private Button btnAddAccount;
        private Button btnRemoveAccount;
        private Button btnAdvOptions;
    }
}