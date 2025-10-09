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
            InitializeComponent();
            LoadOverlayDdays();
            Console.WriteLine("DuDuDay_Sub 디버그 시작:");

            //communicator.OnMessageReceived += HandleMessage;
            //communicator.StartListening();

            // communicator 초기화 및 이벤트 등록
            communicator = new SubCommunicator();
            communicator.OnMessageReceived += HandleMessage;
            communicator.StartListening();
        }
        /*
        private void HandleMessage(DuDuDay_Core.MessagePacket msg)
        {
            Dispatcher.Invoke(() =>
            {
                if (msg.Command == "ReloadDdays")
                {
                    DdayOverlayList.Children.Clear();
                    LoadOverlayDdays();
                }
            });
        }
        */
        private void HandleMessage(MessagePacket msg)
        {
            Console.WriteLine($"[Sub] 메시지 수신: {msg.Command} / {msg.Payload}");

            // 테스트용 동작: "Test" 메시지 받으면 콘솔에 로그 남김
            if (msg.Command == "Test")
            {
                Console.WriteLine("[Sub] Main으로부터 테스트 메시지 수신 완료!");
            }
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
