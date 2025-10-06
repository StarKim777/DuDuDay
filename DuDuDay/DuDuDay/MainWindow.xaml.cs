using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DuDuDay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("디버깅 로그: Dday 로딩 시작");
            LoadDdays();
        }
        
        private void LoadDdays()
        {            
            string filePath = "ddays.json";
            if (!File.Exists(filePath))
                return;
            string json = File.ReadAllText(filePath);
            var ddays = JsonSerializer.Deserialize<List<DdayItem>>(json) ?? new List<DdayItem>();


            
            // 가공된 데이터 만들기
            var activeItems = new List<DdayViewModel>();
            foreach (var d in ddays)
            {
                if (!d.IsActive) continue;

                int diff = (d.Date - DateTime.Today).Days;
                string ddayText = diff == 0 ? "D-Day" :
                                  diff > 0 ? $"D-{diff}" : $"D+{Math.Abs(diff)}";

                activeItems.Add(new DdayViewModel
                {
                    Name = d.Name,
                    Date = d.Date,
                    BackgroundColor = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(d.BackgroundColor),
                    FontColor = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(d.FontColor),
                    DdayText = ddayText
                });
            }

            DdayList.ItemsSource = activeItems;
        }
        /*
        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Visible;
            Storyboard sb = (Storyboard)FindResource("SlideInStoryboard");
            sb.Begin();
        }

        private void CloseSettings_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("SlideOutStoryboard");
            sb.Completed += (s, ev) =>
            {
                SettingsPanel.Visibility = Visibility.Collapsed;
            };
            sb.Begin();
        }
        */
    }
    // JSON 데이터 구조
    public class DdayItem
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string FontColor { get; set; } = "#000000";
    }

    // UI 바인딩용 ViewModel
    public class DdayViewModel
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string DdayText { get; set; } = string.Empty;
        public System.Windows.Media.Brush BackgroundColor { get; set; }
        public System.Windows.Media.Brush FontColor { get; set; }
    }


}