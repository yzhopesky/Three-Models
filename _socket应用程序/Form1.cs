using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace _socket应用程序
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtServer.Text);
            //网络断点  ip地址和端口号
            IPEndPoint point = new IPEndPoint(ip, int.Parse(txtPort.Text));
            //创建负责监听用的socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //因为不确定输出的端口号是否有人使用所以使用try catch
            try
            {
                //绑定网络断点
                socket.Bind(point);
                //监听 10 连接队列的长度
                socket.Listen(10);
                SetTxt("开始监听");
                Thread th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socket);
            }
            catch (Exception ex)
            {
                SetTxt(ex.Message);
            }

        }
        //创建通信用的socket  循环输出连接情况
        Dictionary<string, Socket> dic = new Dictionary<string, Socket>();
        //构建listen方法 使用负责监听用的socket
        void Listen(object o)
        {
            //负责监听用的socket
            Socket socket = o as Socket;
            while (true)
            {
                try
                {
                    //创建负责通信用的socket
                    Socket connSocket = socket.Accept();
                    //获取ip地址和端口号
                    string ipStr = connSocket.RemoteEndPoint.ToString();
                    SetTxt(ipStr + ":连接成功！");
                    //填充下拉框
                    cboUsers.Items.Add(ipStr);

                    //
                    dic.Add(ipStr, connSocket);
                    //开启一个线程接收消息
                    Thread th = new Thread(RecMsg);
                    th.IsBackground = true;
                    th.Start(connSocket);
                }
                catch (Exception ex)
                {
                    SetTxt(ex.Message);
                }

            }
        }
        //一个方法封装 负责通信用的socket
        void RecMsg(object o)
        {
            Socket connSocket = o as Socket;
            byte[] buffer = new byte[1024 * 1024 * 5];
            while (true)
            {
                try
                {
                    //实际收到的字节个数 count
                    int count = connSocket.Receive(buffer);
                    string ipStr = connSocket.RemoteEndPoint.ToString();
                    if (count == 0)
                    {
                        SetTxt(ipStr + ":断开连接！");
                        connSocket.Shutdown(SocketShutdown.Both);
                        connSocket.Close();
                        break;
                    }
                    string msg = Encoding.UTF8.GetString(buffer, 0, count);
                    SetTxt(ipStr + ":" + msg);
                }
                catch (Exception ex)
                {
                    SetTxt(ex.Message);
                }

            }

        }
        //只是一个方法 封装起来 方便使用 减少代码重复量
        void SetTxt(string t)
        {
            txtLog.AppendText(t + "\r\n");
        }
        //发送消息
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedIndex > -1)
            {
                string key = cboUsers.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(txtMsg.Text);

                List<byte> list = new List<byte>();
                list.Add(1);  //协议  1代表文字
                list.AddRange(buffer);
                dic[key].Send(list.ToArray());

            }
            else
            {
                MessageBox.Show("请选择客户端");
            }
        }
        //选择文件
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = ofd.FileName;
            }
        }
        //发送文件
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedIndex > -1)
            {
                string key = cboUsers.Text;
                //dic[key]   

                if (txtPath.Text.Length > 0)
                {
                    using (FileStream fs = new FileStream(txtPath.Text, FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        List<byte> list = new List<byte>();
                        list.Add(2);//协议 2 文件

                        list.AddRange(buffer);
                        dic[key].Send(list.ToArray());
                    }
                }
                else
                {
                    MessageBox.Show("请选择文件");
                }
            }
            else
            {
                MessageBox.Show("请选择客户端");
            }
        }
        //震动
        private void btnZD_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedIndex > -1)
            {
                string key = cboUsers.Text;
                byte[] buffer = new byte[1];
                buffer[0] = 3;
                dic[key].Send(buffer);
            }
            else
            {
                MessageBox.Show("请选择客户端");
            }
        }
    }
}
