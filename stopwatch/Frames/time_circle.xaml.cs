using System.Windows.Controls;

namespace stopwatch.Frames
{
    public partial class time_circle : UserControl
    {
        public time_circle()
        {
            InitializeComponent();
            
            DataContext = this;
        }
        
        public string Title { get; set; }

        public string TimeFirst { get; set; }
        public string TimeLast { get; set; }
    }
}