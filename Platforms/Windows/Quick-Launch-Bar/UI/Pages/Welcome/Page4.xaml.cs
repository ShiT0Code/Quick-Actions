using Microsoft.UI.Xaml.Controls;
using System;
using Windows.System;

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

        private async void NextButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            setting.SaveBoolSetting("IsWelcomed", true);

            await Launcher.LaunchUriAsync(new Uri("shi-qlb:"));

            // 关闭当前应用
            App.Current.Exit();
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

    }
}
