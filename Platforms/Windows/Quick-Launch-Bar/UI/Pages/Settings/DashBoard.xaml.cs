using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoard : Page
    {
        public DashBoard()
        {
            this.InitializeComponent();

            SwitchViewModel =new SwitchViewModel();
        }

        public SwitchViewModel SwitchViewModel { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow.Index = 1;
            Frame.Navigate(typeof(SideBarSetting), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            setting.SaveBoolSetting("IsSideBarOn", SiTo.IsOn);
        }

        private void ConTo_Toggled(object sender, RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            //setting.SaveBoolSetting("IsControlCOn", Tog.IsOn);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SettingsWindow.Index = 2;
            Frame.Navigate(typeof(ContorlCSetting), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }

    public class Switchs
    {
        public bool SideBar { get; set; }
        public bool ControlCenter { get; set; }


        public Switchs()
        {
            var Checking = new SettingsManager();
            SideBar = Checking.CheckBoolSetting("IsSideBarOn");
            ControlCenter = Checking.CheckBoolSetting("IsControlCOn");
        }
    }

    public class SwitchViewModel
    {
        private Switchs defaultSettings = new Switchs();
        public Switchs DefaultSettings { get { return defaultSettings; } }
    }
}
