using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Drawing.Imaging;
using System.Threading;

namespace MandaImagenes
{
    public partial class Form1 : Form
    {
        private bool ExisteDispositivo = false;
        private FilterInfoCollection DispositivoDeVideo;
        private VideoCaptureDevice FuenteDeVideo = null;

        public Form1()
        {
            InitializeComponent();
            BuscarDispositivos();//busca lascamaras
            
        }
        //public void prueba()
        //{
        //    // 1. to create a socket
        //    Socket sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    // 2. Fill IP
        //    IPAddress IP = IPAddress.Parse("192.168.1.107");
        //    IPEndPoint IPE = new IPEndPoint(IP, 4321);

        //    // 3. binding
        //    sListen.Bind(IPE);

        //    // 4. Monitor
        //    //Console.WriteLine("Service is listening ...");
        //    sListen.Listen(2);

        //    // 5. loop to accept client connection requests
        //    while (true)
        //    {
        //        Socket clientSocket;
        //        try
        //        {
        //            clientSocket = sListen.Accept();
        //        }
        //        catch
        //        {
        //            throw;
        //        }

        //        // send data to the client
        //        //clientSocket.Send (Encoding.Unicode.GetBytes ("I am a server, you there?? !!!!"));

        //        // send the file
        //        byte[] buffer = convertPicBoxImageToByte(EspacioCamara);//GetBytes("hola");
        //        clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
        //        //Console.WriteLine("Send success!");
        //    }
        //}

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private byte[] convertPicBoxImageToByte(System.Windows.Forms.PictureBox pbImage)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            pbImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private static byte[] ReadImageFile(String img)
        {
            FileInfo fileinfo = new FileInfo(img);
            byte[] buf = new byte[fileinfo.Length];
            FileStream fs = new FileStream(img, FileMode.Open, FileAccess.Read);
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            //fileInfo.Delete ();
            GC.ReRegisterForFinalize(fileinfo);
            GC.ReRegisterForFinalize(fs);
            return buf;
        }



        /// <summary>
        /// carga los dispositivos a un combobos
        /// </summary>
        /// <param name="Dispositivos"></param>
        public void CargarDispositivos(FilterInfoCollection Dispositivos)
        {
            for (int i = 0; i < Dispositivos.Count; i++) ;

            cbxDispositivos.Items.Add(Dispositivos[0].Name.ToString());
            cbxDispositivos.Text = cbxDispositivos.Items[0].ToString();

        }


        /// <summary>
        /// busca todas las camaras que esten ligadas a la computadora
        /// </summary>
        public void BuscarDispositivos()
        {
            DispositivoDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (DispositivoDeVideo.Count == 0)
            {
                ExisteDispositivo = false;
            }

            else
            {
                ExisteDispositivo = true;
                CargarDispositivos(DispositivoDeVideo);

            }
        }

        /// <summary>
        /// para el video
        /// </summary>
        public void TerminarFuenteDeVideo()
        {
            if (!(FuenteDeVideo == null))
                if (FuenteDeVideo.IsRunning)
                {
                    FuenteDeVideo.SignalToStop();
                    FuenteDeVideo = null;
                }

        }

        /// <summary>
        /// va a estar actualizando las imagenes para que paresca video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void Video_NuevoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //obten la imagen de la camara
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            //muestra la imagen en el picturebox
            EspacioCamara.Image = Imagen;

        }


        /// <summary>
        /// al dar click se debe de inicial la camara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Iniciar")
            {//checa si existen dispositivos
                if (ExisteDispositivo)
                {
                    //activa la camara para que actualize la imagen del picturebox
                    FuenteDeVideo = new VideoCaptureDevice(DispositivoDeVideo[cbxDispositivos.SelectedIndex].MonikerString);
                    FuenteDeVideo.NewFrame += new NewFrameEventHandler(Video_NuevoFrame);
                    FuenteDeVideo.Start();
                    Estado.Text = "Ejecutando Dispositivo…";
                    button1.Text = "Detener";
                    cbxDispositivos.Enabled = false;
                    mandaimagen.Enabled = true;
                    //groupBox1.Text = DispositivoDeVideo[cbxDispositivos.SelectedIndex].Name.ToString();
                }
                else
                    Estado.Text = "Error: No se encuenta el Dispositivo";

            }
            else
            {
                if (FuenteDeVideo.IsRunning)//si esta corriendo la macara
                {//termina de enviar la imagenes al comobobox
                    TerminarFuenteDeVideo();
                    Estado.Text = "Dispositivo Detenido…";
                    button1.Text = "Iniciar";
                    cbxDispositivos.Enabled = true;
                    mandaimagen.Enabled = false;
                }
            }
        }

        private void mandaimagen_Tick(object sender, EventArgs e)
        {
            //Thread th1 = new Thread(new ThreadStart(sendAsincrona));
            sendAsincrona();
        }

        public void sendAsincrona()
        {
            // 1. to create a socket
            Socket sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2. Fill IP
            IPAddress IP = IPAddress.Parse("192.168.1.115");
            IPEndPoint IPE = new IPEndPoint(IP, 4321);

            // 3. binding
            sListen.Bind(IPE);

            // 4. Monitor
            //Console.WriteLine("Service is listening ...");
            sListen.Listen(2);

            // 5. loop to accept client connection requests
            while (true)
            {
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
                byte[] buffer = convertPicBoxImageToByte(EspacioCamara);//GetBytes("hola");
                clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
                //Console.WriteLine("Send success!");
            }
        }

    }
}
