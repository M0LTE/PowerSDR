using PowerSDR.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            npc = new NetworkPalClient();
            npc.Configure(new Dictionary<string, string> {
                { "server", "127.0.0.1" },
                { "port", "5555" },
            });
        }

        NetworkPalClient npc;

        private void Init_Click(object sender, EventArgs e)
        {
            MessageBox.Show(npc.Init().ToString());
        }

        private void setNotificationCallback_Click(object sender, EventArgs e)
        {
            npc.SetCallback(callback);
        }

        void callback(uint bitmap)
        {
            MessageBox.Show(bitmap.ToString());
        }
    }
}
