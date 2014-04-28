using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerApplication
{
    public partial class MainServerForm : Form
    {
        Server myserver;

        public MainServerForm()
        {
            InitializeComponent();
        }

        private void serverstartbutton_Click(object sender, EventArgs e)
        {
            myserver = new Server();
            myserver.needlog += myserver_needlog;
            myserver.stratserver("127.0.0.1",80); // get from input

        }

        private void serverstopbutton_Click(object sender, EventArgs e)
        {
            myserver.stopserver();
        }

        private void myserver_needlog(object sender, ServerInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, ServerInfoEventArgs>(myserver_needlog), new object[] { sender, e });
                return;
            }
            consoletextbox.AppendText(e.consoleinfo);
        }
    }
}
