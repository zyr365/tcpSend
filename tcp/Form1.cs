using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Net.Sockets;
using System.Net;

namespace tcp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "101.91.224.228";
            textBox1.Text = "192.168.1.100";
            textBox2.Text = "8008";
        }
        FileStream fs = new FileStream(@"1.jpg", FileMode.Open);
        private void SendImage(IPAddress remoteIP,int Port)
        {
            //实例化socket               
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(remoteIP, Port);
            socket.Connect(ipep);
            long contentLength = fs.Length;
            //第一次发送数据包的大小           
            socket.Send(BitConverter.GetBytes(contentLength));
            while (true)            {
                //每次发送128字节               
                byte[] bits = new byte[128];
                int r = fs.Read(bits, 0, bits.Length);
                if (r <= 0) break;
                socket.Send(bits, r, SocketFlags.None);
            }
            socket.Close();
            fs.Position = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendImage(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
            MessageBox.Show("发送成功");
        }
    }
}
