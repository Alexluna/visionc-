using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace ReciImagenes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }
        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread th1 = new Thread(new ThreadStart(getImages));
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
                //Thread th1 = new Thread(new ThreadStart(getImages));
            
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            sendAsincrona("arriba");
            
            
        }

        public void sendAsincrona(string texto)
        {
            // 1. to create a socket
            Socket sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2. Fill IP
            IPAddress IP = IPAddress.Parse("192.168.1.115");
            IPEndPoint IPE = new IPEndPoint(IP, 4322);

            // 3. binding
            sListen.Bind(IPE);

            // 4. Monitor
            //Console.WriteLine("Service is listening ...");
            sListen.Listen(2);

            // 5. loop to accept client connection requests

            Socket clientSocket;
            try
            {
                clientSocket = sListen.Accept();
            }
            catch
            {
                throw;
            }

            // send data to the client
            //clientSocket.Send (Encoding.Unicode.GetBytes ("I am a server, you there?? !!!!"));

            // send the file
            byte[] buffer = GetBytes(texto);
            clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
            sListen.Close();
            //Console.WriteLine("Send success!");}
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendAsincrona("izquierda");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sendAsincrona("derecha");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendAsincrona("abajo");
        }

        
    }
}
