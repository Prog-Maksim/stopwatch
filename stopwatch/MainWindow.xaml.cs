using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using stopwatch.Frames;

namespace stopwatch
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartStopWatch(object sender, MouseButtonEventArgs e)
        {
            if (state == false)
            {
                state = true;

                startTimeAsync();
                
                ButtonImage.Source = new BitmapImage(new Uri("Resource/Images/pause.png", UriKind.Relative));
            }
            else
            {
                state = false;
                ButtonImage.Source = new BitmapImage(new Uri("Resource/Images/play-button.png", UriKind.Relative));
            }
        }

        private void RefreshStopWatch(object sender, MouseButtonEventArgs e)
        {
            state = false;

            hour = 0;
            minute = 0;
            second = 0;
            millisecond = 0;

            UpdateText();

            stackpanel.Children.Clear();
            ButtonImage.Source = new BitmapImage(new Uri("Resource/Images/play-button.png", UriKind.Relative));
        }

        private List<time_circle> frames = new List<time_circle>();
        private List<string> timeStrings = new List<string>();
        
        private void FlagStopWatch(object sender, MouseButtonEventArgs e)
        {
            if (state == false) return;

            time_circle timeCircleControl = new time_circle();

            timeCircleControl.Title = $"{frames.Count + 1}";
            timeCircleControl.TimeFirst = $"{hour:D2}:{minute:D2}:{second:D2}";
            timeStrings.Add($"{hour:D2}:{minute:D2}:{second:D2}");
            timeCircleControl.TimeLast = sumTimes();
            stackpanel.Children.Insert(0, timeCircleControl);
            
            frames.Add(timeCircleControl);
        }

        private string sumTimes()
        {
            TimeSpan total = TimeSpan.Zero;

            foreach (string timeString in timeStrings)
            {
                if (TimeSpan.TryParseExact(timeString, "h\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan time))
                {
                    total += time;
                }
            }

            return string.Format("{0:00}:{1:00}:{2:00}", (int)total.TotalHours, total.Minutes, total.Seconds);
        }

        private bool state;

        private int hour;
        private int minute;
        private int second;
        private int millisecond;

        async Task startTimeAsync()
        {
            while (state)
            {
                millisecond++;

                if (millisecond == 60)
                {
                    millisecond = 0;
                    second++;
                }
                else if (second == 60)
                {
                    second = 0;
                    minute++;
                }
                else if (minute == 60)
                {
                    minute = 0;
                    hour++;
                }

                UpdateText();
                await Task.Delay(1);
            }
        }

        private void UpdateText()
        {
            MainTime.Text = $"{hour:D2}:{minute:D2}:{second:D2}";
            Time.Text = $",{millisecond:D2}";
        }

        private void Border_MouseDows(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void ExitWindow(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void MinimizeWindow(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}