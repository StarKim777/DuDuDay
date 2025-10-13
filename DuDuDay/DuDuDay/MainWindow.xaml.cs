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
        private List<DdayItem> ddays = new List<DdayItem>();
        public MainWindow()
        {
            Console.WriteLine("[Main] 디버깅 로그: Dday 로딩 시작");
            InitializeComponent();          
            LoadDdays();
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
            ddays = DdayStorage.Load();
            DdayList.ItemsSource = ddays; // 모든 아이템 표시
            /*
            // Core 프로젝트의 DdayStorage를 사용
            var ddays = DdayStorage.Load();

            // 가공된 데이터 만들기
            var activeItems = new List<DdayViewModel>();
            foreach (var d in ddays)
            {
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

            DdayList.ItemsSource = activeItems;*/

        }

        private void ToggleActive(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk && chk.DataContext is DdayItem item)
            {
                item.IsActive = chk.IsChecked == true;

                // 저장
                DdayStorage.Save(ddays);

                // Sub프로그램으로 알림 전송
                OnDdayChanged();

                Console.WriteLine($"[Main] {item.Name} IsActive={item.IsActive}");
            }
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
}