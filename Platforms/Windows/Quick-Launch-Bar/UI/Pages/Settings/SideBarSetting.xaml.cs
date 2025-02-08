using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System;

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


        private void LoadInfo(bool IsEe, string name, string des, int ic_path, int style)
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
            textBox1.TextChanged += Name_TextBox1_TextChanged;
            textBox1.LosingFocus += ChangedName_TextBox1_LosingFocus;

            TextBox textBox2 = new TextBox()
            {
                Text = des,
                Header = "描述："
            };
            textBox2.TextChanged += Des_TextBox2_TextChanged;

            ComboBox comboBox = new ComboBox()
            {
                Header = "选择一个样式"
            };
            comboBox.SelectionChanged += Style_ComboBox_SelectionChanged;

            var BasicStackPanel = new StackPanel()
            {
                Spacing = 8
            };
            BasicStackPanel.Children.Add(toggleSwitch);
            BasicStackPanel.Children.Add(textBox1);
            BasicStackPanel.Children.Add(textBox2);
            BasicStackPanel.Children.Add(comboBox);

            ViewGrid.Children.Add(BasicStackPanel);
        }


        private void ItemIsEnable_ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            item.isEnable = toggleSwitch.IsOn;
        }

        private void Name_TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.name = textBox.Text;
        }

        private void ChangedName_TextBox1_LosingFocus(UIElement sender, Microsoft.UI.Xaml.Input.LosingFocusEventArgs args)
        {
            SideBarList.ItemsSource = null;
            SideBarList.ItemsSource = ViewModel.Items;
        }

        private void Des_TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.description = textBox.Text;
        }

        private void Style_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            item.style = comboBox.SelectedIndex;
        }


        SideBarItem item = new SideBarItem();
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SideBarList.SelectedItem is SideBarItem selectedItem)
            {
                bool IsEe = selectedItem.isEnable;
                string name = selectedItem.name;
                string des = selectedItem.description;
                int ic_path = selectedItem.icon;
                int style = selectedItem.style;

                add_A.IsEnabled = ActionList.IsEnabled = del_I.IsEnabled = true;

                ActionList.ItemsSource = selectedItem.actions;

                LoadInfo(IsEe, name, des, ic_path, style);

                item = selectedItem;
            }
        }

        private void Selected_Button_Click(object sender, RoutedEventArgs e)
        {

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
            }
        }

        private void AddItem_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SideBarItem sideBarItem = new SideBarItem()
            {
                name = "New",
                description = "",
                icon = 0,
                style = 0,
                isEnable = true,
                actions = new ObservableCollection<SideBarItemAction>()
            };
            ViewModel.Items.Add(sideBarItem);
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
            }
        }

        private void AddAction_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SideBarItemAction action = new SideBarItemAction()
            {
                title1 = "New Action"
            };
            item.actions.Add(action);
        }
    }


    public partial class SideBarItem
    {
        public string name { get; set; } = "";

        public string description { get; set; } = "";

        public int icon { get; set; } = 0;

        public int style { get; set; } = -1;

        public bool isEnable { get; set; } = true;

        public ObservableCollection<SideBarItemAction> actions = new ObservableCollection<SideBarItemAction>();
    }

    public partial class SideBarItemAction
    {
        public string title1 { get; set; } = "";

        public string description1 { get; set; } = "";

        public string action1 { get; set; } = "";

        public bool isEnable1 { get; set; } = true;

        public int icon1 { get; set; } = 0;
    }
    public class SideBarSettingsViewModel
    {
        public ObservableCollection<SideBarItem> Items { get; set; }=new ObservableCollection<SideBarItem>();
    }
}