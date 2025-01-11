using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FreeSideBar.UI.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SideBarMenu : Window
    {
        public SideBarMenu()
        {
            this.InitializeComponent();

            RectInt32 Bounds = new RectInt32(15, 460, 200, 300);

            if (App.IsRight)
                Bounds = new RectInt32(1225, 460, 200, 300);

            this.AppWindow.MoveAndResize(Bounds);

            this.ExtendsContentIntoTitleBar = true;

            this.AppWindow.IsShownInSwitchers = false;
            this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);

            var overlappedPresenter = this.AppWindow.Presenter as OverlappedPresenter;
            if (overlappedPresenter != null)
            {
                overlappedPresenter.IsAlwaysOnTop = true;
                overlappedPresenter.IsMaximizable = false;
                overlappedPresenter.IsMinimizable = false;
                overlappedPresenter.IsResizable = false;
                overlappedPresenter.SetBorderAndTitleBar(true, false);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            settings.Show();

            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            var exiting = new ExitingWarning();
            exiting.Show();
        }
    }
}
