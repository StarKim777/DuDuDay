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
using System.Diagnostics;
using DuDuDay_Core;

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

            // 프로그램 시작 시 Sub에게 테스트 메시지 전송
            try
            {
                var msg = new MessagePacket
                {
                    Command = "Test",
                    Payload = "Hello from Main"
                };
                MainCommunicator.SendMessage(msg);
                Console.WriteLine("디버깅 로그: test 메시지 전송 완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"디버깅 로그: 메시지 전송 실패 - {ex.Message}");
            }
        }

        private void OnDdayChanged()
        {
            // D-day 데이터가 변경되었을 때 Sub에게 갱신 요청
            var msg = new MessagePacket
            {
                Command = "ReloadDdays"
            };

            MainCommunicator.SendMessage(msg);
        }


        private void LoadDdays()
        {
            // Core 프로젝트의 DdayStorage를 사용
            var ddays = DdayStorage.Load();

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