using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT.Interop;
using System.Runtime.InteropServices;
using Quick_Launch_Bar.UI.Pages.Settings;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;
using Windows.System;
using System.Diagnostics;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Quick_Launch_Bar.UI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SideBarWindow : Window
    {
        // 设置窗口样式
        public SideBarWindow()
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SystemBackdrop = new WinUIEx.TransparentTintBackdrop();
            
            this.AppWindow.IsShownInSwitchers = false;
        }

        bool IsLeft = true;
        double scFa = 1;
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置窗口
            this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);

            var over_Presenter = this.AppWindow.Presenter as OverlappedPresenter;
            if (over_Presenter != null)
            {
                over_Presenter.IsAlwaysOnTop = true;
                over_Presenter.IsMaximizable = false;
                over_Presenter.IsMinimizable = false;
                over_Presenter.IsResizable = false;
            }

            // 获取当前窗口的句柄
            IntPtr hWnd = WindowNative.GetWindowHandle(this);

            // 获取当前窗口的 DPI 缩放比例
            uint dpi = GetDpiForWindow(hWnd);

            // 计算缩放比例（百分比）
            double scalingFactor = (double)dpi / 96;
            scFa = scalingFactor;

            this.AppWindow.Resize(new Windows.Graphics.SizeInt32((int)(136 * scalingFactor), (int)(96 * scalingFactor)));

            // 禁用 Windows 11 的圆角效果
            int attribute = 33; // DWMWA_WINDOW_CORNER_PREFERENCE
            int preference = 1; // DWMWCP_DONOTROUND
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(int));

            // 获取当前窗口样式
            int windowStyle = GetWindowLong(hWnd, GWL_STYLE);

            // 移除窗口的标题栏样式（禁用阴影）
            SetWindowLong(hWnd, GWL_STYLE, windowStyle & ~WS_CAPTION);

            // 获取窗口所在的显示器句柄
            IntPtr hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY);

            // 获取显示器信息
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            GetMonitorInfo(hMonitor, ref monitorInfo);

            // 计算显示器的宽和高
            int screenWidth = monitorInfo.rcMonitor.Right - monitorInfo.rcMonitor.Left;
            int screenHeight = monitorInfo.rcMonitor.Bottom - monitorInfo.rcMonitor.Top;

            // 输出显示器信息
            float PerceOfHe = 1;
            float PerceOfWi = 1;

            if (screenWidth > 1920)
                PerceOfWi = screenWidth / 1920;
            if (screenHeight > 1080)
                PerceOfHe = screenHeight / 1080;

            double WinX = 0;
            double WinY = screenHeight / 2 + scalingFactor * -96;

            if (SideBarSetting.IsLoadedLeft)
            {
                IsLeft = false;
                this.Title = "Right";
                WinX = screenWidth - 6 * scalingFactor;
                Grid.SetColumn(NButton, 0);
            }
            else
            {
                this.Title = "Left";
                SideBarSetting.IsLoadedLeft = true;
                new SideBarWindow().Activate();
                IsLeft = true;
                WinX = -130 * scalingFactor;
                Grid.SetColumn(NButton, 2);
            }

            this.AppWindow.Move(new Windows.Graphics.PointInt32((int)WinX, (int)WinY));

            WinX = (int)WinX;
        }

        // Windows API 常量
        private const int MONITOR_DEFAULTTOPRIMARY = 1;

        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0x00C00000;

        // Windows API 结构体
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        // Windows API 函数
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("dwmapi.dll", CharSet = CharSet.Auto, PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);



        // 处理事件

        private void ShapeButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);
            var over_Presenter = this.AppWindow.Presenter as OverlappedPresenter;
            over_Presenter?.Restore();
        }

        private void NButton_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            PointInt32 point = this.AppWindow.Position;
            point.Y = (int)(point.Y - 250 * scFa);

            SizeInt32 size = this.AppWindow.Size;

            size.Width = size.Width + 250;
            size.Height = size.Height + 500;

            this.AppWindow.Resize(size);

            ChangeCol.Width = new GridLength(374);
            if (IsLeft)
            {
                Grid.SetColumn(NButton, 0);
                point.X = (int)(point.X + 130 * scFa);
            }
            else
            {
                Grid.SetColumn(NButton, 2);
                point.X = (int)(point.X - 380 * scFa);
            }
            

            this.AppWindow.Move(point);
            

            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Flyout_Closed(object sender, object e)
        {
            if (this.AppWindow != null)
            {
                PointInt32 point = this.AppWindow.Position;
                point.Y = (int)(point.Y + 250 * scFa);

                SizeInt32 size = this.AppWindow.Size;

                size.Width = (int)(136 * scFa);
                size.Height = (int)(96 * scFa);

                this.AppWindow.Resize(size);

                ChangeCol.Width = new GridLength(124);

                if (IsLeft)
                {
                    Grid.SetColumn(NButton, 2);
                    point.X = (int)(point.X - 130 * scFa);
                }
                else
                {
                    Grid.SetColumn(NButton, 0);
                    point.X = (int)(point.X + 380 * scFa);
                }
                this.AppWindow.Move(point);
            }
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().Activate();
        }

        private async void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("shi-qlb://settings/sidebar/edit"));
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            new ExitingWarning().Activate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\LuckyRandom\\LuckyRandom.exe", "随机抽选", false);
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\LuckyRandom\\LuckyRandom.exe", "随机抽选", true);
        }

        private void TimerButton(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopTimer\\DesktopTimer.exe", "希沃计时器", true);
        }

        private void RollCall_Button_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\RollCall\\RollCall.exe", "人数统计", false);
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\RollCall\\RollCall.exe", "人数统计", true);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopAnnotation\\DesktopAnnotation.exe", "桌面批注", true);
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("MS-SCREENCLIP://")); 
            this.AppWindow.Hide();
            await Task.Delay(3000);
            this.AppWindow.Show();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopCalendar\\DesktopCalendar.exe", "希沃日历", true);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopMagnifier\\DesktopMagnifier.exe", "希沃放大镜",true);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopScreenshot\\DesktopScreenshot.exe", "希沃截图",true);

        }

        private void SetUpApp(string executablePath, string Title,bool IsShow)
        {
            bool Result = true;
            string error = "";
            try
            {
                // 启动 Win32 程序
                Process.Start(executablePath);
                Result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Result = false;
            }

            var Notif = new AppNotificationBuilder();

            if (Result)
                Notif.AddText($"{Title} 启动成功！");
            else
            {
                Notif.AddText($"{Title} 启动失败！");
                Notif.AddText($"失败原因：{error}");
            }

            if(IsShow)
                AppNotificationManager.Default.Show(Notif.BuildNotification());
        }


        private void NextBu_Click(object sender, RoutedEventArgs e)
        {
            double set = ContentSV.HorizontalOffset + 288;

            ContentSV.ChangeView(set, null, null);
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            double set = ContentSV.HorizontalOffset - 288;

            ContentSV.ChangeView(set, null, null);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            flyout.Hide();
        }

        private void MoreHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
