using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI.Pages.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SideBarEditAction : Page
    {
        public SideBarEditAction()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            EditTit.Text=SideBarSetting.EditMode;

            base.OnNavigatedTo(e);
            if (e.Parameter is SideBarItem sideBarItem)
            {
                // ��ʾѡ�е��������
                name.Text = sideBarItem.Name;
                des.Text = sideBarItem.Description;
                isEnable.IsOn = sideBarItem.IsEnable;
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "ȷ��Ҫ�����룿",
                Content = "������ʱ�뿪�ᶪʧ��ı༭���ݡ�",
                CloseButtonText = "����",
                PrimaryButtonText = "����",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await Dialog.ShowAsync();

            if(result==ContentDialogResult.Primary)
            {
                this.Frame.GoBack();
            }
        }

        private void IsEnable_ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
