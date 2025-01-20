using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Welcome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            this.InitializeComponent();
        }

        private void NextButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page3), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void LicensingTerms_HyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Reference_HyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Licenses_HyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
