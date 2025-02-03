using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContorlCSetting : Page
    {
        public ContorlCSetting()
        {
            this.InitializeComponent();

            SwitchViewModel = new SwitchViewModel();
        }

        public SwitchViewModel SwitchViewModel { get; set; }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            setting.SaveBoolSetting("IsControlCOn", Tog.IsOn);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "添加操作、快捷方式",
                Content = "Check connection and try again.",
                CloseButtonText = "取消",
                PrimaryButtonText = "添加",
                DefaultButton = ContentDialogButton.Primary
            };

            await Dialog.ShowAsync();
        }
    }

    class ContorlCData
    {

        public ContorlCData()
        {

        }
    }
}
