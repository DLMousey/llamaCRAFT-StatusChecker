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
        public Boolean _up = false;
        private string _version = "Unknown";
        private string _motd = "Unknown";
        private string _currentPlayers = "Unknown";
        private string _maximumPlayers = "Unknown";
        private DateTime _lastChecked = DateTime.Now;
        private int _numberOfChecks = 0;

        public String Text
        {
            get { return this._text; }
            set
            {
                this._text = value;
                NotifyPropertyChanged();
            }
        }

        public Boolean Up
        {
            get { return this._up; }
            set
            {
                this._up = value;
                NotifyPropertyChanged();
            }
        }

        public String Version
        {
            get { return this._version; }
            set
            {
                this._version = value;
                NotifyPropertyChanged();
            }
        }

        public String Motd
        {
            get { return this._motd; }
            set
            {
                this._motd = value;
                NotifyPropertyChanged();
            }
        }

        public String CurrentPlayers
        {
            get { return this._currentPlayers; }
            set
            {
                this._currentPlayers = value;
                NotifyPropertyChanged();
            }
        }

        public String MaximumPlayers
        {
            get { return this._maximumPlayers; }
            set
            {
                this._maximumPlayers = value;
                NotifyPropertyChanged();
            }
        }

        public String CurrentMax
        {
            get { return this._currentPlayers + '/' + this._maximumPlayers; }
        }

        public DateTime LastChecked
        {
            get { return this._lastChecked; }
            set
            {
                this._lastChecked = value;
                NotifyPropertyChanged();
            }
        }

        public int NumberOfChecks
        {
            get { return this._numberOfChecks; }
            set
            {
                this._numberOfChecks = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
