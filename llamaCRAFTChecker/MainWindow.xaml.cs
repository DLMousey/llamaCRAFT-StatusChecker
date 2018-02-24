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
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using llamaCRAFTChecker.Models;

namespace llamaCRAFTChecker
{
    public partial class MainWindow : Window
    {
        const ushort dataSize = 512;
        const ushort numFields = 6;

        protected String hostName = "llamatrain.llamatorials.com";
        protected String ModMapUrl = "http://localhost:8085";
        public List<Mod> ModMap;
        public String ModsPath;
        protected int port = 25565;

        public ServerStatus StatusObject = new ServerStatus();

        public MainWindow()
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(ModMapUrl);

            var Response = (HttpWebResponse) Request.GetResponse();
            var ResponseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();

            this.ModMap = JsonConvert.DeserializeObject<List<Mod>>(ResponseString);

            InitializeComponent();
            GetServerStatus();

            this.DataContext = StatusObject;
        }

        /**
         * @TODO : Move this into a dedicated helper
         * class because it's a tad chunky and shouldn't really
         * be stored in the same class as the presentation logic.
         */
        public void GetServerStatus()
        {
            var rawServerData = new byte[dataSize];

            Stopwatch stopwatch = new Stopwatch();
            TcpClient tcpClient = new TcpClient();

            try
            {
                /**
                 * Start a stopwatch to track the number
                 * of milliseconds taken to query the server
                 * status, Currently only stored in local vars.
                 */
                stopwatch.Start();
                tcpClient.Connect(hostName, port);
                stopwatch.Stop();

                NetworkStream stream = tcpClient.GetStream();

                /**
                 * Process the payload we got back from the server
                 */
                var payload = new byte[] { 0xFE, 0x01 };
                stream.Write(payload, 0, payload.Length);
                stream.Read(rawServerData, 0, dataSize);

                tcpClient.Close();

                /**
                 * Bump the number of checks by 1
                 * and set the last checked datetime to now
                 */
                this.StatusObject.NumberOfChecks += 1;
                this.StatusObject.LastChecked = DateTime.Now;

                /**
                 * Store the number of milliseconds it took to
                 * run the check, Could this potentially be used
                 * to calculate latency?
                 */
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

        /**
         * When the "Check Again" button is clicked
         * re-query the server status
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetServerStatus();
        }

        private void FolderPicker_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.Description = "Select llamaCRAFT Mods Folder";
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            DialogResult result = folderDialog.ShowDialog();

            if(result == System.Windows.Forms.DialogResult.OK)
            {
                String SelectedPath = folderDialog.SelectedPath;
                this.ModsPath = SelectedPath;

                List<String> InstalledMods = new List<String>();
                DirectoryInfo Folder = new DirectoryInfo(this.ModsPath);

                foreach(FileInfo File in Folder.GetFiles())
                {
                    InstalledMods.Add(File.Name);
                }

                foreach(var Mod in this.ModMap)
                {
                    var IsInstalled = InstalledMods.Exists(m => m == Mod.Name);

                    if(!IsInstalled)
                    {
                        using (WebClient client = new WebClient())
                        {
                            String FilePath = this.ModsPath + "\\" + Mod.Name;
                            client.DownloadFile(Mod.Url, FilePath);
                        }
                    }
                }

                System.Windows.MessageBox.Show("Located " + InstalledMods.Count + " Mods");
            }

        }
    }
}
