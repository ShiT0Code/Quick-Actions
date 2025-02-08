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
        // ���ô�����ʽ
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
            // ���ô���
            this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);

            var over_Presenter = this.AppWindow.Presenter as OverlappedPresenter;
            if (over_Presenter != null)
            {
                over_Presenter.IsAlwaysOnTop = true;
                over_Presenter.IsMaximizable = false;
                over_Presenter.IsMinimizable = false;
                over_Presenter.IsResizable = false;
            }

            // ��ȡ��ǰ���ڵľ��
            IntPtr hWnd = WindowNative.GetWindowHandle(this);

            // ��ȡ��ǰ���ڵ� DPI ���ű���
            uint dpi = GetDpiForWindow(hWnd);

            // �������ű������ٷֱȣ�
            double scalingFactor = (double)dpi / 96;
            scFa = scalingFactor;

            this.AppWindow.Resize(new Windows.Graphics.SizeInt32((int)(136 * scalingFactor), (int)(96 * scalingFactor)));

            // ���� Windows 11 ��Բ��Ч��
            int attribute = 33; // DWMWA_WINDOW_CORNER_PREFERENCE
            int preference = 1; // DWMWCP_DONOTROUND
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(int));

            // ��ȡ��ǰ������ʽ
            int windowStyle = GetWindowLong(hWnd, GWL_STYLE);

            // �Ƴ����ڵı�������ʽ��������Ӱ��
            SetWindowLong(hWnd, GWL_STYLE, windowStyle & ~WS_CAPTION);

            // ��ȡ�������ڵ���ʾ�����
            IntPtr hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY);

            // ��ȡ��ʾ����Ϣ
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            GetMonitorInfo(hMonitor, ref monitorInfo);

            // ������ʾ���Ŀ�͸�
            int screenWidth = monitorInfo.rcMonitor.Right - monitorInfo.rcMonitor.Left;
            int screenHeight = monitorInfo.rcMonitor.Bottom - monitorInfo.rcMonitor.Top;

            // �����ʾ����Ϣ
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

        // Windows API ����
        private const int MONITOR_DEFAULTTOPRIMARY = 1;

        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0x00C00000;

        // Windows API �ṹ��
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

        // Windows API ����
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



        // �����¼�

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
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\LuckyRandom\\LuckyRandom.exe", "�����ѡ", false);
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\LuckyRandom\\LuckyRandom.exe", "�����ѡ", true);
        }

        private void TimerButton(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopTimer\\DesktopTimer.exe", "ϣ�ּ�ʱ��", true);
        }

        private void RollCall_Button_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\RollCall\\RollCall.exe", "����ͳ��", false);
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\RollCall\\RollCall.exe", "����ͳ��", true);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopAnnotation\\DesktopAnnotation.exe", "������ע", true);
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
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopCalendar\\DesktopCalendar.exe", "ϣ������", true);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopMagnifier\\DesktopMagnifier.exe", "ϣ�ַŴ�",true);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetUpApp("C:\\Program Files\\Seewo\\MiniApps\\DesktopScreenshot\\DesktopScreenshot.exe", "ϣ�ֽ�ͼ",true);

        }

        private void SetUpApp(string executablePath, string Title,bool IsShow)
        {
            bool Result = true;
            string error = "";
            try
            {
                // ���� Win32 ����
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
                Notif.AddText($"{Title} �����ɹ���");
            else
            {
                Notif.AddText($"{Title} ����ʧ�ܣ�");
                Notif.AddText($"ʧ��ԭ��{error}");
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
