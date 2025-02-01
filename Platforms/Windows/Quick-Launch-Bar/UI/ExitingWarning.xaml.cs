using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExitingWarning : Window
    {
        public ExitingWarning()
        {
            this.InitializeComponent();

            this.SystemBackdrop = new WinUIEx.TransparentTintBackdrop();

            this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);

            var over_Presenter = this.AppWindow.Presenter as OverlappedPresenter;
            if (over_Presenter != null)
                over_Presenter.IsAlwaysOnTop = true;
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = grid.XamlRoot,
                Title = "确定要退出吗？",
                Content = "真的要退出应用 快速启动栏 吗？\n" +
                "你可以重新打开它",
                CloseButtonText = "取消",
                PrimaryButtonText = "重启应用",
                SecondaryButtonText = "确定退出",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.Close();
                await Launcher.LaunchUriAsync(new Uri("shi-qlb://none"));
                App.Current.Exit();
            }
            else if (result == ContentDialogResult.Secondary)
            {
                this.Close();
                App.Current.Exit();
            }
            else
                this.Close();
        }
    }
}
