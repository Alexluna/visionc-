using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisionRobot
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0,300);
            dispatcherTimer.Start();
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            getImages();
        }
        public void getImages()
        {
            System.Net.Sockets.Socket s = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            // 2. fill in the remote IP
            System.Net.IPAddress IP = System.Net.IPAddress.Parse("192.168.1.115");
            System.Net.IPEndPoint IPE = new System.Net.IPEndPoint(IP, 4321);

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


            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(buffer);
            image.EndInit();


            image1.Source = image;
            //MessageBox.Show(GetString(buffer));

            //pictureBox1.Image = ByteToImage(buffer);
            //Console.ReadKey();}
        }

        public static System.Drawing.Bitmap ByteToImage(byte[] blob)
        {
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }
    }
}
