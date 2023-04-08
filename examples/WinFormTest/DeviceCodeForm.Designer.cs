namespace WinFormTest
{
    partial class DeviceCodeForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCode = new System.Windows.Forms.Label();
            this.lbExpire = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(689, 79);
            this.label1.TabIndex = 0;
            this.label1.Text = "To sign in, use a web browser to open the page\r\nhttps://www.microsoft.com/link an" +
    "d enter below code to authenticate.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbCode
            // 
            this.lbCode.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbCode.Location = new System.Drawing.Point(12, 159);
            this.lbCode.Name = "lbCode";
            this.lbCode.Size = new System.Drawing.Size(689, 56);
            this.lbCode.TabIndex = 1;
            this.lbCode.Text = "ABCDEFGH";
            this.lbCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbExpire
            // 
            this.lbExpire.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbExpire.Location = new System.Drawing.Point(12, 215);
            this.lbExpire.Name = "lbExpire";
            this.lbExpire.Size = new System.Drawing.Size(689, 56);
            this.lbExpire.TabIndex = 2;
            this.lbExpire.Text = "00:00";
            this.lbExpire.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(266, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 45);
            this.button1.TabIndex = 3;
            this.button1.Text = "Open page";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DeviceCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 299);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbExpire);
            this.Controls.Add(this.lbCode);
            this.Controls.Add(this.label1);
            this.Name = "DeviceCodeForm";
            this.Text = "DeviceCodeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private Label lbCode;
        private Label lbExpire;
        private Button button1;
        private System.Windows.Forms.Timer timer1;
    }
}