using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DuDuDay_Core;

namespace DuDuDay_Sub
{
    public partial class OverlayWindow : Window
    {
        private readonly SubCommunicator communicator = new();
        public OverlayWindow()
        {
            Console.WriteLine("[Sub] 디버그 시작:");
            InitializeComponent();
            LoadOverlayDdays();

            communicator.OnMessageReceived += HandleMessage;
            communicator.StartListening();

            // communicator 초기화 및 이벤트 등록
            // communicator = new SubCommunicator();
            // communicator.OnMessageReceived += HandleMessage;
            // communicator.StartListening();
        }
        
        private void HandleMessage(DuDuDay_Core.MessagePacket msg)
        {
            Dispatcher.Invoke(() =>
            {
                Console.WriteLine($"[Sub] 메시지 수신: {msg.Command} / {msg.Payload}");
                if (msg.Command == "ReloadDdays") // UI 리로드
                {
                    DdayOverlayList.Children.Clear();
                    LoadOverlayDdays();
                }
                else if (msg.Command == "Test") // 테스트용 동작: "Test" 메시지 받으면 콘솔에 로그 남김
                {
                    Console.WriteLine("[Sub] Main으로부터 테스트 메시지 수신 완료!");
                }
                else
                {
                    Console.WriteLine("[Sub] 메세지에 할당된 동작 없음.");
                }
            });
        }
        
        private void LoadOverlayDdays()
        {
            var ddays = DdayStorage.Load();

            foreach (var item in ddays)
            {
                if (!item.IsActive) continue;

                // D-day 계산
                int diff = (item.Date - DateTime.Now.Date).Days;
                string ddayText = diff == 0 ? "D-Day" : (diff > 0 ? $"D-{diff}" : $"D+{-diff}");

                // 시각화 블록 생성
                var border = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.BackgroundColor)),
                    CornerRadius = new CornerRadius(8),
                    Margin = new Thickness(5),
                    Padding = new Thickness(8)
                };

                var stack = new StackPanel();

                var nameBlock = new TextBlock
                {
                    Text = item.Name,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.FontColor)),
                    FontWeight = FontWeights.Bold,
                    FontSize = 14
                };

                var dateBlock = new TextBlock
                {
                    Text = item.Date.ToString("yyyy-MM-dd"),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.FontColor)),
                    FontSize = 12
                };

                var ddayBlock = new TextBlock
                {
                    Text = ddayText,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.FontColor)),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };

                stack.Children.Add(nameBlock);
                stack.Children.Add(dateBlock);
                stack.Children.Add(ddayBlock);

                border.Child = stack;

                DdayOverlayList.Children.Add(border);
            }
        }
    }
}
