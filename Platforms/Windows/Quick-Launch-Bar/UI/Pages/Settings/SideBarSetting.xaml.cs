using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System;
using System.IO;
using Microsoft.UI.Xaml.Media.Imaging;

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

            ViewModel = new SideBarSettingsViewModel();
        }

        public SideBarSettingsViewModel ViewModel { get; set; }

        public SwitchViewModel SwitchViewModel { get; set; }

        private void IsEnable_ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            setting.SaveBoolSetting("IsSideBarOn", Tog.IsOn);
        }

        public static bool IsLoadedLeft { get; set; } = false;


        private void LoadInfo(bool IsEe, string name, string des, string ic_path, int style)
        {
            ViewGrid.Children.Clear();

            ToggleSwitch toggleSwitch = new ToggleSwitch()
            {
                IsOn = IsEe,
                OnContent = "启用",
                OffContent = "禁用"
            };
            toggleSwitch.Toggled += ItemIsEnable_ToggleSwitch_Toggled;

            TextBox textBox1 = new TextBox()
            {
                Text = name,
                Header = "名称："
            };
            textBox1.LostFocus += TextBox1_LostFocus;

            TextBox textBox2 = new TextBox()
            {
                Text = des,
                Header = "描述："
            };
            textBox2.LostFocus += TextBox2_LostFocus;

            ComboBox comboBox = new ComboBox()
            {
                Header = "选择显示样式"
            };
            comboBox.SelectionChanged += Style_ComboBox_SelectionChanged;


            Microsoft.UI.Xaml.Controls.Image image = new()
            {
                Width = 48,
                Height = 48
            };
            if(File.Exists(ic_path))
                image.Source = new BitmapImage(new Uri(ic_path));

            TextBox textBox3 = new TextBox()
            {
                Text = ic_path,
                Header = "图标路径",
                MinWidth = 250
            };
            textBox3.LostFocus += TextBox3_LostFocus;

            Button button = new Button()
            {
                Content = "浏览",
                VerticalAlignment = VerticalAlignment.Bottom
            };
            button.Click += Selected_Button_Click;

            var IconEditSP = new StackPanel()
            {
                Spacing = 12,
                Orientation = Orientation.Horizontal
            };
            IconEditSP.Children.Add(image);
            IconEditSP.Children.Add(textBox3);
            IconEditSP.Children.Add(button);


            var BasicStackPanel = new StackPanel()
            {
                Spacing = 8,
                MaxWidth = 850
            };
            BasicStackPanel.Children.Add(toggleSwitch);
            BasicStackPanel.Children.Add(textBox1);
            BasicStackPanel.Children.Add(textBox2);
            BasicStackPanel.Children.Add(comboBox);
            BasicStackPanel.Children.Add(IconEditSP);

            ViewGrid.Children.Add(BasicStackPanel);
        }

        private async void TextBox3_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.iconPath = textBox.Text;

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private async void TextBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.description = textBox.Text;

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private async void TextBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (item.name != textBox.Text)
            {
                item.name = textBox.Text;

                SideBarList.ItemsSource = null;
                SideBarList.ItemsSource = ViewModel.Items;
                SideBarList.SelectedItem = item;
            }

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private async void ItemIsEnable_ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            item.isEnable = toggleSwitch.IsOn;

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private async void Style_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            item.style = comboBox.SelectedIndex;

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }


        SideBarItem item = new SideBarItem();
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SideBarList.SelectedItem is SideBarItem selectedItem)
            {
                bool IsEe = selectedItem.isEnable;
                string name = selectedItem.name;
                string des = selectedItem.description;
                string ic_path = selectedItem.iconPath;
                int style = selectedItem.style;

                add_A.IsEnabled = ActionList.IsEnabled = del_I.IsEnabled = true;

                ActionList.ItemsSource = selectedItem.actions;

                LoadInfo(IsEe, name, des, ic_path, style);

                item = selectedItem;
            }
        }

        private async void Selected_Button_Click(object sender, RoutedEventArgs e)
        {
            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private async void DelItem_Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "确认删除该项？",
                Content = "你确定要删除该项吗？",
                CloseButtonText = "取消",
                PrimaryButtonText = "确认删除",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Items.Remove(item);

                ActionList.IsEnabled = add_A.IsEnabled = false;

                ViewGrid.Children.Clear();

                await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
            }
        }

        private async void AddItem_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SideBarItem sideBarItem = new SideBarItem()
            {
                name = "新项"
            };
            ViewModel.Items.Add(sideBarItem);

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }


        SideBarItemAction action = new SideBarItemAction();
        private void ActionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActionList.SelectedItem is SideBarItemAction selectedItem)
            {
                action = selectedItem;

                del_I_A.IsEnabled = true;
            }
        }
        private async void DelAction_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "确认删除该操作？",
                Content = "你确定要删除该操作吗？",
                CloseButtonText = "取消",
                PrimaryButtonText = "确认删除",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                item.actions.Remove(action);

                del_I_A.IsEnabled = false;

                await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
            }
        }

        private async void AddAction_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SideBarItemAction action = new SideBarItemAction()
            {
                title1 = "操作"
            };
            item.actions.Add(action);

            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }



        private async void SideBarList_Loaded(object sender, RoutedEventArgs e)
        {
            SideBarSettingsViewModel sidebarSettingsViewModel = await new SettingsManager().LoadFromJsonFileAsync<SideBarSettingsViewModel>("SideBarItem.json");

            if (sidebarSettingsViewModel != null)
            {
                ViewModel.Items.Clear();
                foreach (var item in sidebarSettingsViewModel.Items)
                {
                    ViewModel.Items.Add(item);
                }
            }
            else
            {
                await new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "警告",
                    Content = "设置无法正确加载！",
                    CloseButtonText = "确定"
                }.ShowAsync();
            }
        }
    }


    public partial class SideBarItem
    {
        public string name { get; set; } = "";

        public string description { get; set; } = "";

        public int style { get; set; } = -1;

        public bool isEnable { get; set; } = true;
        public string iconPath { get; set; } = "";

        public ObservableCollection<SideBarItemAction> actions { get; set; } = new ObservableCollection<SideBarItemAction>();
    }

    public partial class SideBarItemAction
    {
        public string title1 { get; set; } = "";

        public string description1 { get; set; } = "";

        public string action1 { get; set; } = "";

        public bool isEnable1 { get; set; } = true;

        public string actionIcon { get; set; } = "";
    }
    public class SideBarSettingsViewModel
    {
        public ObservableCollection<SideBarItem> Items { get; set; }=new ObservableCollection<SideBarItem>();
    }
}