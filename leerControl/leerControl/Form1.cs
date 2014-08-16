using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace leerControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //while (true)
            timer1.Enabled = true;
        }



        public void getImages()
        {
            try
            {
                System.Net.Sockets.Socket s = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

                // 2. fill in the remote IP
                System.Net.IPAddress IP = System.Net.IPAddress.Parse("192.168.1.115");
                System.Net.IPEndPoint IPE = new System.Net.IPEndPoint(IP, 4322);

                Console.WriteLine("started connection service ....");
                // 3. connect to the server
                s.Connect(IPE);

                // 4. receive data
                byte[] buffer = new byte[1000000];
                s.Receive(buffer, buffer.Length, System.Net.Sockets.SocketFlags.None);
                //var Msg = Encoding.Unicode.GetString (buffer);
                //Console.WriteLine ("received message: (0)", msg);
                Console.WriteLine("Receive success");
                //MessageBox.Show("");
                System.IO.FileStream fs = System.IO.File.Create("1.jpg");
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();


                //BitmapImage image = new BitmapImage();
                //image.BeginInit();
                //image.StreamSource = new System.IO.MemoryStream(buffer);
                //image.EndInit();


                //image1.Source = image;
                //MessageBox.Show(GetString(buffer));
                string datos_control = GetString(buffer);
                //aqui ya obtienes arriba abajo derecha o izquierda
                


                label1.Text = datos_control;
                s.Close();
                //pictureBox1.Image = ByteToImage(buffer);
                //Console.ReadKey();}
            }catch(Exception){}
        }


        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getImages();
        }
    }
}
