using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SideBarSetting : Page
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public SideBarSetting()
#pragma warning restore CS8618 // 按照运行逻辑是不会引发错误的！
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


        public static string EditMode { get; set; } = "";
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditMode = "添加项目";

            this.Frame.Navigate(typeof(SideBarEditAction));
        }

        private SideBarItem SelectItem;
        public void ItemsView_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
        {
            var SelItem = sender.SelectedItem;
            if (sender.SelectedItem != null)
            { 
                EditButton.IsEnabled = true;

                EditMode = "编辑项目";

                SelectItem = (SideBarItem)SelItem;
            }
            else
                EditButton.IsEnabled = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SideBarEditAction), SelectItem);
        }

        private void ToggleSwitch_Toggled_Item(object sender, RoutedEventArgs e)
        {

        }


        public static bool IsLoadedLeft { get; set; } = false;
    }


    public class SideBarItem
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
        public int Style { get; set; }
        public bool IsEnable { get; set; }
        public List<SideBarItemAction> Actions { get; set; } = [];
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
                    Name = "1",
                    Description = "No.",
                    Icon = "",
                    Style = 0,
                    IsEnable = true,
                    Actions =
                    [
                        new SideBarItemAction
                        {
                            Title = "Tit",
                            Description = "Yes.",
                            Icon = "",
                            IsEnable = true,
                            Action = ""
                        }
                    ]
                },
                new SideBarItem
                {
                    Name = "2",
                    Description = "Yes.",
                    Icon = "",
                    Style = 1,
                    IsEnable = false,
                    Actions =
                    [
                        new SideBarItemAction
                        {
                            Title = "1999",
                            Description = "Yes.",
                            Icon = "",
                            IsEnable = true,
                            Action = ""
                        },
                        new SideBarItemAction
                        {
                            Title = "4999",
                            Description = "15",
                            Icon = "",
                            IsEnable = false,
                            Action = ""
                        }
                    ]
                },
                new SideBarItem
                {
                    Name = "3",
                    Description = "???",
                    Icon = "",
                    Style = 3,
                    IsEnable = true,
                    Actions =
                    [
                        new SideBarItemAction
                        {
                            Title = "不想要了",
                            Description = "No!!!",
                            Icon = "",
                            IsEnable = true,
                            Action = ""
                        },
                        new SideBarItemAction
                        {
                            Title = "不想要了",
                            Description = "No!!!",
                            Icon = "",
                            IsEnable = true,
                            Action = ""
                        },
                        new SideBarItemAction
                        {
                            Title = "不想要了",
                            Description = "No!!!",
                            Icon = "",
                            IsEnable = true,
                            Action = ""
                        }
                    ]
                }
            };
        } 
    }
}