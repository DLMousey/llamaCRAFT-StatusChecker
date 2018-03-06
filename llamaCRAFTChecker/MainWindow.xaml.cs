using MahApps.Metro.Controls;
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
    public partial class MainWindow : MetroWindow
    {
        public String CurrentPage;

        public MainWindow()
        {            
            InitializeComponent();
            this.DataContext = CurrentPage;
        }

        private void ModsPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentPage = "Mods";
            ContentFrame.Navigate(new ModsPage());
        }

        private void StatusPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentPage = "Status";
            ContentFrame.Navigate(new StatusPage());
        }
    }
}   
