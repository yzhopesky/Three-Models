using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        Socket socket = null;
        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtServer.Text);
            IPEndPoint point = new IPEndPoint(ip,int.Parse(txtPort.Text));
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //连接服务器
                socket.Connect(point);
                SetTxt("连接成功");
                //接收消息
                Thread th = new Thread(RecMsg);
                th.IsBackground = true;
                th.Start(socket);
            }
            catch (Exception ex)
            {
                SetTxt(ex.Message);
            }


        }
        //客户端接收服务器发送的消息
        void RecMsg(object o)
        {
            Socket socket = o as Socket;
            byte[] buffer = new byte[1024 * 1024 * 5];
            while (true)
            {
                try
                {
                    int count = socket.Receive(buffer);
                    string ipCon = socket.RemoteEndPoint.ToString();
                    int flag = buffer[0];//协议
                    if (flag == 1)//文字
                    {

                        string msg = Encoding.UTF8.GetString(buffer, 1, count - 1);
                        SetTxt(ipCon + ":" + msg);
                    }
                    else if (flag == 2)//文件
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                fs.Write(buffer, 1, count - 1);
                            }
                        }
                    }
                    else if (flag == 3)
                    {
                        Test();
                    }
                }
                catch (Exception ex)
                {
                    socket.Close();
                    SetTxt(ex.Message);
                    break;
                }

            }
        }
        void Test()
        {
            this.TopMost = true;//调用在顶端的
            int n = 1;
            for (int i = 0; i < 20; i++)
            {
                n = i % 2 == 0 ? 1 : -1;
                this.Location = new Point(this.Location.X - 10 * n, this.Location.Y - 10 * n);
                System.Threading.Thread.Sleep(80);
            }
        }
        void SetTxt(string msg)
        {
            txtLog.AppendText(msg + "\r\n");
        }
        //发送消息
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (socket != null && socket.Connected)
            {
                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(txtMsg.Text);
                    socket.Send(buffer);
                    SetTxt("消息发送完成！");
                }
                catch (Exception ex)
                {
                    SetTxt(ex.Message);

                }

            }
        }
    }
}
