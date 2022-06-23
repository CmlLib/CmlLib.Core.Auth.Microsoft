
namespace CmlLib.Core.Auth.Microsoft.UI.WinForm
{
    partial class MicrosoftLoginForm
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
            this.lbLoading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbLoading
            // 
            this.lbLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLoading.Location = new System.Drawing.Point(0, 0);
            this.lbLoading.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbLoading.Name = "lbLoading";
            this.lbLoading.Size = new System.Drawing.Size(702, 542);
            this.lbLoading.TabIndex = 0;
            this.lbLoading.Text = "Microsoft Login\r\nLoading";
            this.lbLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MicrosoftLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(702, 542);
            this.Controls.Add(this.lbLoading);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.Name = "MicrosoftLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MicrosoftLoginForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MicrosoftLoginForm_FormClosing);
            this.Load += new System.EventHandler(this.Window_Loaded);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbLoading;
    }
}