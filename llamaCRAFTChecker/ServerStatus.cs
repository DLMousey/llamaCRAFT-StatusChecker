using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace llamaCRAFTChecker
{
    public class ServerStatus : INotifyPropertyChanged
    {
        private string _text = "Unknown";
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Boolean Up { get; set; } = false;
        public String Text
        {
            get { return this._text; }
            set
            {
                this._text = value;
                NotifyPropertyChanged();
            }
        }

        public String Version { get; set; } = "Unknown";
        public String Motd { get; set; } = "Unknown";
        public String CurrentPlayers { get; set; } = "Unknown";
        public String MaximumPlayers { get; set; } = "Unknown";
    }
}
