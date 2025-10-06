using System.Configuration;
using System.Data;
using System.Windows;

namespace DuDuDay_Sub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var overlay = new OverlayWindow();
            overlay.Show();
        }

    }

}
