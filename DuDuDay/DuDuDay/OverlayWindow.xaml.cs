using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.InteropServices; // 
using System.Windows.Interop; //
using System.Windows.Forms;

namespace DuDuDay
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
            Loaded += OverlayWindow_Loaded;
        }

        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 모니터 크기 가져오기
            var screen = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.Left = screen.Right - this.Width - 10; // 우측 여백 10px
            this.Top = screen.Top + 10; // 상단 여백 10px

            // Win32 핸들 얻기
            var hwnd = new WindowInteropHelper(this).Handle;

            // 항상 맨 아래로 (바탕화면 위, 다른 창 밑)
            SetWindowPos(hwnd, HWND_BOTTOM, (int)this.Left, (int)this.Top,
                         (int)this.Width, (int)this.Height,
                         SWP_NOACTIVATE | SWP_SHOWWINDOW);
        }

        #region Win32 API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;
        #endregion 
        /*
        public OverlayWindow()
        {
            InitializeComponent();
            LoadOverlayDdays();
        }

        private void LoadOverlayDdays()
        {
            var ddays = DdayStorage.Load("ddays.json");

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
        */
    }
}
