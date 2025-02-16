using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System;
using System.IO;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

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


        private void LoadInfo(bool IsEe, string name, string des, string ic_path,string ic_Path_dark, int style)
        {
            ViewGrid.Children.Clear();

            ToggleSwitch toggleSwitch = new ToggleSwitch()
            {
                IsOn = IsEe,
                OnContent = "����",
                OffContent = "����"
            };
            toggleSwitch.Toggled += ItemIsEnable_ToggleSwitch_Toggled;

            TextBox textBox1 = new TextBox()
            {
                Text = name,
                Header = "���ƣ�"
            };
            textBox1.LostFocus += TextBox1_LostFocus;

            TextBox textBox2 = new TextBox()
            {
                Text = des,
                Header = "������"
            };
            textBox2.LostFocus += TextBox2_LostFocus;

            ComboBox comboBox = new()
            {
                Header = "ѡ����ʾ��ʽ",
                SelectedIndex = style
            };
            comboBox.Items.Add(new ComboBoxItem() { Content = "��ͨ��ť" });
            comboBox.Items.Add(new ComboBoxItem() { Content = "���Ӱ�ť" });
            comboBox.Items.Add(new ComboBoxItem() { Content = "������ť" });
            comboBox.Items.Add(new ComboBoxItem() { Content = "��ְ�ť" });

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
                Header = "ͼ��·��",
                MinWidth = 250
            };
            textBox3.LostFocus += TextBox3_LostFocus;

            Button button = new Button()
            {
                Content = "���",
                VerticalAlignment = VerticalAlignment.Bottom,
                IsEnabled = false
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



            Microsoft.UI.Xaml.Controls.Image image_dark = new()
            {
                Width = 48,
                Height = 48
            };
            if (File.Exists(ic_Path_dark))
                image_dark.Source = new BitmapImage(new Uri(ic_Path_dark));

            TextBox textBox3_dark = new TextBox()
            {
                Text = ic_Path_dark,
                Header = "��ɫģʽͼ��·��",
                MinWidth = 250
            };
            textBox3_dark.LostFocus += TextBox3_dark_LostFocus;

            Button button_dark = new Button()
            {
                Content = "���",
                VerticalAlignment = VerticalAlignment.Bottom,
                IsEnabled = false
            };
            button_dark.Click += Select_Button_dark_Click;

            var IconEditSP_dark = new StackPanel()
            {
                Spacing = 12,
                Orientation = Orientation.Horizontal
            };
            IconEditSP_dark.Children.Add(image_dark);
            IconEditSP_dark.Children.Add(textBox3_dark);
            IconEditSP_dark.Children.Add(button_dark);


            var BasicStackPanel = new StackPanel()
            {
                Spacing = 8,
                MaxWidth = 850,
                Orientation=Orientation.Vertical
            };
            BasicStackPanel.Children.Add(toggleSwitch);
            BasicStackPanel.Children.Add(textBox1);
            BasicStackPanel.Children.Add(textBox2);
            BasicStackPanel.Children.Add(comboBox);
            BasicStackPanel.Children.Add(IconEditSP);
            BasicStackPanel.Children.Add(IconEditSP_dark);

            ViewGrid.Children.Add(BasicStackPanel);
        }

        private void Style_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            item.style = comboBox.SelectedIndex;
        }

        private void Select_Button_dark_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void TextBox3_dark_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.iconPathDark = textBox.Text;
        }

        private void TextBox3_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.iconPath = textBox.Text;
        }

        private void TextBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            item.description = textBox.Text;
        }

        private void TextBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (item.name != textBox.Text)
            {
                item.name = textBox.Text;

                SideBarList.ItemsSource = null;
                SideBarList.ItemsSource = ViewModel.Items;
                SideBarList.SelectedItem = item;
            }
        }

        private void ItemIsEnable_ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            item.isEnable = toggleSwitch.IsOn;
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
                string ic_path_dark = selectedItem.iconPathDark;
                int style = selectedItem.style;

                add_A.IsEnabled = ActionList.IsEnabled = del_I.IsEnabled = true;

                ActionList.ItemsSource = selectedItem.actions;

                LoadInfo(IsEe, name, des, ic_path,ic_path_dark, style);

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
                Title = "ȷ��ɾ�����",
                Content = "��ȷ��Ҫɾ��������",
                CloseButtonText = "ȡ��",
                PrimaryButtonText = "ȷ��ɾ��",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Items.Remove(item);

                ActionList.IsEnabled = add_A.IsEnabled = false;

                ViewGrid.Children.Clear();

                if(ViewModel.Items.Count==0)
                    new SettingsManager().SaveBoolSetting("IsNoneItem", true);
            }
        }

        private void AddItem_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SideBarItem sideBarItem = new SideBarItem()
            {
                name = "����"
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
                Title = "ȷ��ɾ���ò�����",
                Content = "��ȷ��Ҫɾ���ò�����",
                CloseButtonText = "ȡ��",
                PrimaryButtonText = "ȷ��ɾ��",
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
                title1 = "����"
            };
            item.actions.Add(action);
        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
        }



        private void SideBarList_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async void LoadData()
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
            else if(new SettingsManager().CheckBoolSetting("IsNoneItem"))
            {
                ;
            }
            else
            {
                await new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "����",
                    Content = "�����޷���ȷ���أ�",
                    CloseButtonText = "ȷ��"
                }.ShowAsync();
            }
        }

        private async void SaveAll_Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Task.Delay(250);
            await new SettingsManager().SaveViewModelToJsonFileAsync(ViewModel, "SideBarItem.json");
        }

        private void GiveUp_Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewGrid.Children.Clear();

            ActionList.IsEnabled = add_A.IsEnabled = del_I.IsEnabled = del_I_A.IsEnabled = false;

            SideBarList.ItemsSource = null;

            LoadData();
            SideBarList.ItemsSource = ViewModel.Items;
        }

        private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {

        }
    }


    public class SideBarItem
    {
        public string name { get; set; } = "";

        public string description { get; set; } = "";

        public int style { get; set; } = 0;

        public bool isEnable { get; set; } = true;
        public string iconPath { get; set; } = "";
        public string iconPathDark { get; set; } = "";

        public ObservableCollection<SideBarItemAction> actions { get; set; } = new ObservableCollection<SideBarItemAction>();
    }

    public class SideBarItemAction
    {
        public string title1 { get; set; } = "";

        public string description1 { get; set; } = "";

        public string action1 { get; set; } = "";

        public bool isEnable1 { get; set; } = true;

        public bool isShowNot { get; set; } = true;

        public int actionTimes { get; set; } = 1;

        public string actionIcon { get; set; } = "";
        public string actionIconDark { get; set; } = "";
    }
    public class SideBarSettingsViewModel
    {
        public ObservableCollection<SideBarItem> Items { get; set; }=new ObservableCollection<SideBarItem>();
    }
}