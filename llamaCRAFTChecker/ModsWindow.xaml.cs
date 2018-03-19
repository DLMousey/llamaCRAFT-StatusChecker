using llamaCRAFTChecker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace llamaCRAFTChecker
{
    /// <summary>
    /// Interaction logic for ModsWindow.xaml
    /// </summary>
    public partial class ModsWindow : Window
    {
        protected String ModMapUrl = "http://localhost:8085";

        public ModMap ModMap = new ModMap();
        public String ModsPath;

        public ModsWindow()
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(ModMapUrl);

            try
            {
                InitializeComponent();

                var Response = (HttpWebResponse)Request.GetResponse();
                var ResponseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();

                List<Mod> ModList = JsonConvert.DeserializeObject<List<Mod>>(ResponseString);
                AllocateMods(ModList);
                
            }
            catch (System.Net.WebException ex)
            {
                System.Windows.MessageBox.Show("Unable to connect to llamaCRAFT Update Servers");
                Close();
            }
        }

        public void AllocateMods(List<Mod> ModsList)
        {
            this.ModMap.Mods = new List<Mod>();

            foreach(Mod mod in ModsList)
            {
                this.ModMap.Mods.Add(mod);
            }
        }
    }
}
