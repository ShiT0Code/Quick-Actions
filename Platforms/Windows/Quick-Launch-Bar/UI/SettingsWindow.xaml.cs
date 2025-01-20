using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using Quick_Launch_Bar.UI.Pages.Settings;
using Quick_Launch_Bar.UI.Pages.Welcome;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            Nav.SelectedItem=Nav.MenuItems[0];
        }

        private void Nav_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
            string selectedItemTag = ((string)selectedItem.Tag);

            switch (selectedItemTag)
            {
                case "DashBoard":
                    ContentFrame.Navigate(typeof(DashBoard), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                    break;
                case "About":
                    ContentFrame.Navigate(typeof(About), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                    break;
            }
        }
    }
}
