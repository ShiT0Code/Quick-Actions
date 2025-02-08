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
                Title = "ȷ��Ҫ�˳���",
                Content = "���Ҫ�˳�Ӧ�� ���������� ��\n" +
                "��������´���",
                CloseButtonText = "ȡ��",
                PrimaryButtonText = "ȷ���˳�",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.Close();
                App.Current.Exit();
            }
            else
                this.Close();
        }
    }
}
