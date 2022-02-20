using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsalClientTest
{
    public partial class DeviceCodeForm : Form
    {
        private DeviceCodeResult? deviceCode;

        public DeviceCodeForm()
        {
            InitializeComponent();
        }

        public void SetDeviceCodeResult(DeviceCodeResult code)
        {
            deviceCode = code;

            timer1.Start();
            lbCode.Text = deviceCode.UserCode;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (deviceCode == null)
                return;

            lbExpire.Text = deviceCode.ExpiresOn.LocalDateTime.Subtract(DateTime.Now).ToString(@"mm\:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (deviceCode == null)
                return;

            Util.OpenUrl(deviceCode.VerificationUrl);
        }
    }
}
