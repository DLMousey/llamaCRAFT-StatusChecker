﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for StatusPage.xaml
    /// </summary>
    public partial class StatusPage : Page
    {
        const ushort dataSize = 512;
        const ushort numFields = 6;

        protected String hostName = "llamatrain.llamatorials.com";
        protected int port = 25565;

        public ServerStatus StatusObject = new ServerStatus();

        public StatusPage()
        {
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

            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect(hostName, port);

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
            }
            catch (Exception)
            {
                this.StatusObject.Text = "Down!";
                this.StatusObject.Up = false;
            }

            if (rawServerData == null || rawServerData.Length == 0)
            {
                this.StatusObject.Text = "Down!";
                this.StatusObject.Up = false;
            }
            else
            {
                var serverData = Encoding.Unicode.GetString(rawServerData).Split("\u0000\u0000\u0000".ToCharArray());
                if (serverData != null && serverData.Length >= numFields)
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

        private void CheckModsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService NavService = NavigationService.GetNavigationService(this);
            NavService.Navigate(new Uri("ModsPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
