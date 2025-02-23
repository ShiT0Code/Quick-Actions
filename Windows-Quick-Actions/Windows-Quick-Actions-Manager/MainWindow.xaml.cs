using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Windows_Quick_Actions_Manager
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // 启用自定义标题栏
            this.ExtendsContentIntoTitleBar = true;

            // 启用高标题栏
            if (ExtendsContentIntoTitleBar == true)
            {
                this.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            }
        }

        private void navigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {

        }
    }
}
