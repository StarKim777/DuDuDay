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

            var brushConverter = new BrushConverter();
            var viewModels = new List<DdayViewModel>();

            foreach (var d in ddays)
            {
                int diff = (d.Date - DateTime.Today).Days;
                string ddayText = diff == 0 ? "D-Day" :
                                  diff > 0 ? $"D-{diff}" :
                                  $"D+{Math.Abs(diff)}";

                viewModels.Add(new DdayViewModel
                {
                    Name = d.Name,
                    Date = d.Date,
                    DdayText = ddayText,
                    IsActive = d.IsActive,
                    BackgroundColor = (Brush)brushConverter.ConvertFromString(d.BackgroundColor),
                    FontColor = (Brush)brushConverter.ConvertFromString(d.FontColor)
                });
            }

            DdayList.ItemsSource = viewModels;
        }

        private void ToggleActive(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk && chk.DataContext is DdayViewModel vm)
            {
                // ViewModel의 IsActive 변경
                vm.IsActive = chk.IsChecked == true;

                // 원본 DdayItem도 동기화
                var item = ddays.Find(x => x.Name == vm.Name && x.Date == vm.Date);
                if (item != null)
                    item.IsActive = vm.IsActive;

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