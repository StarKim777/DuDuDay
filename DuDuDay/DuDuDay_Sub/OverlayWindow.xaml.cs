using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DuDuDay_Core;

namespace DuDuDay_Sub
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();



            LoadOverlayDdays();

            
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
