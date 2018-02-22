using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace llamaCRAFTChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const ushort dataSize = 512;
        const ushort numFields = 6;

        protected String hostName = "llamatrain.llamatorials.com";
        protected int port = 25565;

        public ServerStatus StatusObject = new ServerStatus();

        public MainWindow()
        {
            InitializeComponent();
            GetServerStatus();

            this.DataContext = StatusObject;
        }

        public void GetServerStatus()
        {
            var rawServerData = new byte[dataSize];

            Stopwatch stopwatch = new Stopwatch();
            TcpClient tcpClient = new TcpClient();

            try
            {
                stopwatch.Start();
                tcpClient.Connect(hostName, port);
                stopwatch.Stop();

                NetworkStream stream = tcpClient.GetStream();

                var payload = new byte[] { 0xFE, 0x01 };
                stream.Write(payload, 0, payload.Length);
                stream.Read(rawServerData, 0, dataSize);

                tcpClient.Close();
                long Delay = stopwatch.ElapsedMilliseconds;
            }
            catch(Exception)
            {
                this.StatusObject.Text = "Down!";
                this.StatusObject.Up = false;
            }

            if(rawServerData == null || rawServerData.Length == 0)
            {
                this.StatusObject.Text = "Down!";
                this.StatusObject.Up = false;
            }
            else
            {
                var serverData = Encoding.Unicode.GetString(rawServerData).Split("\u0000\u0000\u0000".ToCharArray());
                if(serverData != null && serverData.Length >= numFields)
                {
                    this.StatusObject.Up = true;
                    this.StatusObject.Text = "Up!";
                    this.StatusObject.Version = serverData[2];
                    this.StatusObject.Motd = serverData[3];
                    this.StatusObject.CurrentPlayers = serverData[4];
                    this.StatusObject.MaximumPlayers = serverData[5];
                }
                else
                {
                    this.StatusObject.Up = false;
                }
            }
        }
    }
}
