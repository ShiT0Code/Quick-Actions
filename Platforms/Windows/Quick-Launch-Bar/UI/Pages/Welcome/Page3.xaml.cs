using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Welcome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        public Page3()
        {
            this.InitializeComponent();
        }

        private void NextButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page4), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

    }
}
