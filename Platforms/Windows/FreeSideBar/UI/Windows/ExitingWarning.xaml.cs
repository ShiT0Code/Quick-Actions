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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FreeSideBar.UI.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExitingWarning : Window
    {
        public ExitingWarning()
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;

            SizeInt32 size = new SizeInt32(300, 180);
            this.AppWindow.Resize(size);

            this.AppWindow.IsShownInSwitchers = false;
            this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);

            var overlappedPresenter = this.AppWindow.Presenter as OverlappedPresenter;
            if (overlappedPresenter != null)
            {
                overlappedPresenter.IsAlwaysOnTop = true;
                overlappedPresenter.IsMaximizable = false;
                overlappedPresenter.IsMinimizable = false;
                overlappedPresenter.IsResizable = false;
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            App.Current.Exit();
        }
    }
}
