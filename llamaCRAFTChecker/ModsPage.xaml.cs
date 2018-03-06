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
using System.Windows.Navigation;
using System.Windows.Shapes;
using llamaCRAFTChecker.Models;

namespace llamaCRAFTChecker
{
    /// <summary>
    /// Interaction logic for ModsPage.xaml
    /// </summary>
    public partial class ModsPage : Page
    {
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
    }
}
