using Microsoft.UI.Xaml;
using Quick_Launch_Bar.UI.Pages.Welcome;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;

        }

        private void Content_Loaded(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(Page1));
        }
    }
}
