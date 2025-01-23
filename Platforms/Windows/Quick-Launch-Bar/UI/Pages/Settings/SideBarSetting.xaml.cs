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
            ViewModels = new SiderBarItemsViewModels();
        }

        public SwitchViewModel SwitchViewModel { get; set; }
        public SiderBarItemsViewModels ViewModels { get; set; }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var setting = new SettingsManager();
            setting.SaveBoolSetting("IsSideBarOn", Tog.IsOn);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var frame = new Frame();
            frame.Navigate(typeof(SideBarAddAction));

            ContentDialog Dialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "修改操作、快捷方式",
                Content = frame,
                CloseButtonText = "取消",
                PrimaryButtonText = "保存",
                DefaultButton = ContentDialogButton.Primary,
                MinWidth = 500
            };

            await Dialog.ShowAsync();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var frame=new Frame();
            frame.Navigate(typeof(SideBarAddAction));

            ContentDialog Dialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "添加操作、快捷方式",
                Content = frame,
                CloseButtonText = "取消",
                PrimaryButtonText = "添加",
                DefaultButton = ContentDialogButton.Primary,
                MinWidth = 500
            };

            await Dialog.ShowAsync();
        }

        private void ItemsView_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
        {
            EditButton.IsEnabled= true;

        }
    }




    public class SiderBarItems
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsEnable { get; set; } = false;
        public int Style { get; set; } = 0;
        public string IconPath { get; set; } = "";

        //public string ActionName1 { get; set; } = "";
        //public string ActionDo1 { get; set; } = "";

        //public string ActionName2 { get; set; } = "";
        //public string ActionDo2 { get; set; } = "";

        //public string ActionName3 { get; set; } = "";
        //public string ActionDo3 { get; set; } = "";

        //public string ActionName4 { get; set; } = "";
        //public string ActionDo4 { get; set; } = "";

        //public string ActionName5 { get; set; } = "";
        //public string ActionDo5 { get; set; } = "";
    }

    public class SiderBarItemsViewModels
    {
        private ObservableCollection<SiderBarItems> items=new ObservableCollection<SiderBarItems>();
        public ObservableCollection<SiderBarItems> Items { get { return items; } }

        public SiderBarItemsViewModels() 
        {
            items.Add(new SiderBarItems() { Name="Name", IsEnable=true, Description="No.", Style=0, IconPath=""});
            items.Add(new SiderBarItems() { Name = "Name1", IsEnable = true, Description = "Yes.", Style = 1, IconPath = "" });
            items.Add(new SiderBarItems() { Name = "Name3", IsEnable = true, Description = "Yes.", Style = 2, IconPath = "" });

        }
    }
}
