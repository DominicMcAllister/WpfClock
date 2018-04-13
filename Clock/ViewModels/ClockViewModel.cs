using Clock.ConfigSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using TimeZoneNames;

namespace Clock.ViewModels
{
    public class ClockViewModel : INotifyPropertyChanged, IDisposable
    {
        private DispatcherTimer _timer;
        private string _date;
        private string _time;
        private string _selectedDateFormat;
        private string _selectedTimeFormat;
        private string _backgroundImagePath;

        public ClockViewModel()
        {
            //setup the preferences of the user.
            var dateFormat = ClockConfiguration.GetPreference(ClockConfiguration.Preferences.DateFormat);
            if(dateFormat == null)
            {
                dateFormat = DateFormats.First().Value;
                ClockConfiguration.SetPreference(ClockConfiguration.Preferences.DateFormat, dateFormat);
            }
            SelectedDateFormat = dateFormat;

            var timeFormat = ClockConfiguration.GetPreference(ClockConfiguration.Preferences.TimeFormat);
            if(timeFormat == null)
            {
                timeFormat= TimeFormats.First().Value;
                ClockConfiguration.SetPreference(ClockConfiguration.Preferences.TimeFormat, timeFormat);
            }

            SelectedTimeFormat = timeFormat;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        #region Properties

        public string Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<KeyValuePair<string,string>> DateFormats
        {
            get
            {
                return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("MM/DD/YYYY", "dddd MM/dd/yyyy"),
                    new KeyValuePair<string, string>("DD/MM/YYYY", "dddd dd/MM/yyyy"),
                    new KeyValuePair<string, string>("YYYY/MM/DD", "dddd yyyy/MM/dd"),
                    new KeyValuePair<string, string>("YYYY/DD/MM", "dddd yyyy/dd/MM"),
                };
            }
        }

        public string SelectedDateFormat
        {
            get { return _selectedDateFormat; }
            set
            {
                if(_selectedDateFormat != value)
                {
                    _selectedDateFormat = value;
                    NotifyPropertyChanged();

                    //update the user settings.
                    ClockConfiguration.SetPreference(ClockConfiguration.Preferences.DateFormat, value);
                }
            }
        }

        public List<KeyValuePair<string,string>> TimeFormats
        {
            get
            {
                return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("12-Hour", "hh:mm:ss tt"),
                    new KeyValuePair<string, string>("24-Hour", "HH:mm:ss")
                };
            }
        }

        public string SelectedTimeFormat
        {
            get { return _selectedTimeFormat; }
            set
            {
                if (_selectedTimeFormat != value)
                {
                    _selectedTimeFormat = value;
                    NotifyPropertyChanged();

                    //update the user settings
                    ClockConfiguration.SetPreference(ClockConfiguration.Preferences.TimeFormat, value);
                }
            }
        }

        public string BackgroundImagePath
        {
            get { return _backgroundImagePath; }
            set
            {
                if(_backgroundImagePath != value)
                {
                    _backgroundImagePath = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        private void timer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            var abbreviated = TZNames.GetAbbreviationsForTimeZone(TimeZoneInfo.Local.Id, "en-US");
            string timezone = TimeZoneInfo.Local.IsDaylightSavingTime(now) ? abbreviated.Daylight : abbreviated.Standard;

            Date = now.ToString(_selectedDateFormat);
            Time = string.Format("{0} {1}", now.ToString(_selectedTimeFormat), timezone);

            int hour = now.Hour;
            if(hour >= 6 && hour <= 16)
            {
                BackgroundImagePath = "Images/morning.jpeg";
            }
            else if (hour > 16 && hour < 21)
            {
                BackgroundImagePath = "Images/dusk.jpg";
            }
            else
            {
                BackgroundImagePath = "Images/night.jpg";
            }
        }

        internal void WindowKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D:
                case Key.F:
                    var indexOfCurrentDateFormat = this.DateFormats.IndexOf(this.DateFormats.FirstOrDefault(d => d.Value == this.SelectedDateFormat));
                    var newIndex = indexOfCurrentDateFormat == this.DateFormats.Count - 1 ? 0 : indexOfCurrentDateFormat + 1;
                    this.SelectedDateFormat = this.DateFormats[newIndex].Value;

                    break;
                case Key.T:
                case Key.H:
                    var indexOfCurrentTimeFormat = this.TimeFormats.IndexOf(this.TimeFormats.FirstOrDefault(d => d.Value == this.SelectedTimeFormat));
                    var newIndex2 = indexOfCurrentTimeFormat == this.TimeFormats.Count - 1 ? 0 : indexOfCurrentTimeFormat + 1;
                    this.SelectedTimeFormat = this.TimeFormats[newIndex2].Value;
                    break;
            }
            timer_Tick(this, null);
        }

        #endregion

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            //clean up the timer.
            if (disposing)
            {
                _timer.Stop();
                _timer = null;
            }

            disposed = true;
        }

        #endregion
    }
}