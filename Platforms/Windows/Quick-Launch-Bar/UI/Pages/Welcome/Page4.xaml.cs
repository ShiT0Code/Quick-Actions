using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Welcome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page4 : Page
    {
        public Page4()
        {
            this.InitializeComponent();
        }

        private void NextButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var next = new SettingsWindow();
            next.Activate();

            var setting=new SettingsManager();
            bool a = setting.Welcomed();
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

    }
}
