using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Connector : Form
    {
        public Connector()
        {
            InitializeComponent();
        }

        private void connectorbutton_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                tcpclnt.Connect("127.0.0.1", 80);

                Stream stm = tcpclnt.GetStream();

                byte[] ba = new ASCIIEncoding().GetBytes("Client1");
                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(bb[i]));

                tcpclnt.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error..... " + ex.Message);
            }

        }
    }
}
