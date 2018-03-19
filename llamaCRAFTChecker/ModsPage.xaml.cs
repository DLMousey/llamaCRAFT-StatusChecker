using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Forms;
using llamaCRAFTChecker.Models;

namespace llamaCRAFTChecker
{
    /// <summary>
    /// Interaction logic for ModsPage.xaml
    /// </summary>
    public partial class ModsPage : Page
    {
        public string ModsPath;
        public List<Mod> ModMap;
        protected String ModMapUrl = "http://llamacraft.deadlyllama.com/api/mods";

        public ModsPage()
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(ModMapUrl);

            var Response = (HttpWebResponse)Request.GetResponse();
            var ResponseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();

            this.ModMap = JsonConvert.DeserializeObject<List<Mod>>(ResponseString);

            InitializeComponent();
            this.DataContext = ModMap;
        }

        private void SyncModsButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog FolderDialog = new FolderBrowserDialog();

            FolderDialog.ShowNewFolderButton = false;
            FolderDialog.Description = "Select llamaCRAFT Mods Directory";
            FolderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;

            DialogResult Result = FolderDialog.ShowDialog();

            if(Result == System.Windows.Forms.DialogResult.OK)
            {
                String SelectedPath = FolderDialog.SelectedPath;
                this.ModsPath = SelectedPath;

                List<String> InstalledMods = new List<string>();
                List<String> MissingMods = new List<string>();
                DirectoryInfo Folder = new DirectoryInfo(this.ModsPath);

                foreach(FileInfo File in Folder.GetFiles())
                {
                    InstalledMods.Add(File.Name);
                }

                foreach(var Mod in this.ModMap)
                {
                    Boolean IsInstalled = InstalledMods.Exists(m => m == Mod.Name);

                    if(!IsInstalled)
                    {
                        MissingMods.Add(Mod.Name);

                        using (WebClient Client = new WebClient())
                        {
                            String FilePath = this.ModsPath + "\\" + Mod.File_Name;
                            Client.DownloadFile(Mod.Url, FilePath);
                        }
                    }
                }

                System.Windows.MessageBox.Show("Successfully installed " + MissingMods.Count + " missing mods!");
            }
        }
    }
}
