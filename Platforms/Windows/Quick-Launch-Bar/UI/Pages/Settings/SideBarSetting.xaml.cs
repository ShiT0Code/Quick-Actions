using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SideBarSetting : Page
    {
        public SideBarSetting()
        {
            this.InitializeComponent();

            SwitchViewModel = new SwitchViewModel();

            viewModel = new SideBarSettingsViewModel();
        }

        public SideBarSettingsViewModel viewModel { get; set; }

        public SwitchViewModel SwitchViewModel { get; set; }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            setting.SaveBoolSetting("IsSideBarOn", Tog.IsOn);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }


    public class SideBarItem
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
        public int Style { get; set; }
        public bool IsEnable { get; set; }
        public List<SideBarItemAction> Actions { get; set; } =
            new List<SideBarItemAction>();
    }

    public class SideBarItemAction
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Action { get; set; } = "";
        public bool IsEnable { get; set; }
        public string Icon { get; set; } = "";
    }

    public class SideBarSettingsViewModel
    {
        public List<SideBarItem> Items { get; set; }

        public SideBarSettingsViewModel() 
        {
            Items = new List<SideBarItem>
            {
                new SideBarItem
                {
                    Name="1",
                    Description="No.",
                    Icon="",
                    Style=0,
                    IsEnable=true,
                    Actions=new List<SideBarItemAction>()
                    {
                        new SideBarItemAction
                        {
                            Title="Tit",
                            Description="Yes.",
                            Icon="",
                            IsEnable=true,
                            Action=""
                        }
                    }
                },
                                new SideBarItem
                {
                    Name="2",
                    Description="Yes.",
                    Icon="",
                    Style=1,
                    IsEnable=false,
                    Actions=new List<SideBarItemAction>()
                    {
                        new SideBarItemAction
                        {
                            Title="1999",
                            Description="Yes.",
                            Icon="",
                            IsEnable=true,
                            Action=""
                        },
                        new SideBarItemAction
                        {
                            Title="4999",
                            Description="15",
                            Icon="",
                            IsEnable=false,
                            Action=""
                        }
                    }
                }

            };
        }
    }
}