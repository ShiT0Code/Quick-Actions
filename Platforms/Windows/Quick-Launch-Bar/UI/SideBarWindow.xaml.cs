using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT.Interop;
using System.Runtime.InteropServices;
using Quick_Launch_Bar.UI.Pages.Settings;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;
using System.Diagnostics;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;
using Microsoft.UI.Xaml.Media;

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

            ViewModel = new SideBarSettingsViewModel();
        }
        public SideBarSettingsViewModel ViewModel { get; set; }


        bool IsLeft = true;
        double scFa = 1;
        int scw = 1920;
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

            scw = screenWidth;

            // Ӧ����ʾ����Ϣ
            float PerceOfWi = 1;

            if (screenWidth > 1920)
                PerceOfWi = screenWidth / 1920;

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

            size.Width = (int)(size.Width + 300 * scFa);
            size.Height = (int)(size.Height + 500 * scFa);

            this.AppWindow.Resize(size);

            ChangeCol.Width = new GridLength(407);
            if (IsLeft)
            {
                Grid.SetColumn(NButton, 0);
                point.X = (int)(point.X + 130 * scFa);
            }
            else
            {
                Grid.SetColumn(NButton, 3);
                point.X = (int)(point.X - 419 * scFa);
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
                    point.X = (int)(-130 * scFa);
                }
                else
                {
                    Grid.SetColumn(NButton, 0);
                    point.X = (int)(scw - 6 * scFa);
                }
                this.AppWindow.Move(point);
            }
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            new AllSettingsWindow().Activate();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            new ExitingWarning().Activate();
        }

        private void SetUpApp(string executablePath, string Title,bool IsShow, int times)
        {
            bool Result = true;
            string error = "";
            try
            {
                for (int i = 0; i < times; i++)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = executablePath,
                        UseShellExecute = true
                    });
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Result = false;
            }

            var Notif = new AppNotificationBuilder();

            if (Result)
                Notif.AddText($"{Title} �����ɹ���")
                     .AddText($"���{executablePath}");
            else
            {
                IsShow = true;
                Notif.AddText($"{Title} ����ʧ�ܣ�");
                Notif.AddText($"ʧ��ԭ��{error}");
            }

            if(IsShow)
                AppNotificationManager.Default.Show(Notif.BuildNotification());
            flyout.Hide();
        }


        private void NextBu_Click(object sender, RoutedEventArgs e)
        {
            double set = ContentSV.HorizontalOffset + 320;

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


        SideBarItem barItem = new SideBarItem();
        private async void ActionItems_Loaded(object sender, RoutedEventArgs e)
        {
            SideBarSettingsViewModel sidebarSettingsViewModel = await new SettingsManager().LoadFromJsonFileAsync<SideBarSettingsViewModel>("SideBarItem.json");

            if (sidebarSettingsViewModel != null)
            {
                ViewModel.Items.Clear();
                foreach (var item in sidebarSettingsViewModel.Items)
                {
                    ViewModel.Items.Add(item);
                }

                ActionItems.Children.Clear();
                foreach (var item in ViewModel.Items)
                {
                    Grid grid = new Grid()
                    {
                        Height = 80,
                        Width = 80
                    };
                    TextBlock textBlock = new TextBlock()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Text = item.name
                    };
                    grid.Children.Add(textBlock);
                    Image image = new()
                    {
                        Width = 40,
                        Height = 30,
                        Stretch = Stretch.Uniform
                    };
                    if (File.Exists(item.iconPath))
                        image.Source = new BitmapImage(new Uri(item.iconPath));

                    switch (item.style)
                    {
                        case 1:
                            HyperlinkButton hyperlinkButton1 = new HyperlinkButton()
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Top,
                                Height = 50,
                                Width = 50,
                                Tag = GetButtonContentTag(),
                                Content =image
                            };
                            hyperlinkButton1.Click += Only_HyButton_Click;
                            grid.Children.Add(hyperlinkButton1);
                            break;
                        case 2:
                            DropDownButton dropDownButton = new()
                            {
                                Width = 65,
                                Height = 50,
                                VerticalAlignment = VerticalAlignment.Top,
                                Content = image
                            };
                            image.Width = 23;
                            MenuFlyout menuFlyout = new MenuFlyout();

                            foreach (var ac_i in item.actions)
                            {
                                string Tag = "";
                                Tag = $"\"{ac_i.action1}\",{ac_i.title1}," +
                                    $"{ac_i.isShowNot},{ac_i.actionTimes}," +
                                    $"{ac_i.isEnable1}";

                                var menu1 = new MenuFlyoutItem()
                                {
                                    Text = ac_i.title1,
                                    Tag = Tag
                                };
                                menu1.Click += MenuFlyoutItem_Click;

                                menuFlyout.Items.Add(menu1);
                            }

                            dropDownButton.Flyout = menuFlyout;

                            grid.Children.Add(dropDownButton);
                            break;
                        case 3:
                            SplitButton splitButton = new()
                            {
                                Width = 80,
                                Height = 50,
                                VerticalAlignment = VerticalAlignment.Top,
                                Tag = GetButtonContentTag(),
                                Content = image
                            };
                            image.Width = 25;
                            splitButton.Click += SplitButton_Click;
                            grid.Children.Add(splitButton);

                            MenuFlyout menuFlyoutList = new MenuFlyout();

                            foreach (var ac_i in item.actions)
                            {
                                string Tag = "";
                                Tag = $"\"{ac_i.action1}\",{ac_i.title1}," +
                                    $"{ac_i.isShowNot},{ac_i.actionTimes}," +
                                    $"{ac_i.isEnable1}";

                                var menu1 = new MenuFlyoutItem()
                                {
                                    Text = ac_i.title1,
                                    Tag = Tag
                                };
                                menu1.Click += MenuFlyoutItem_Click;

                                menuFlyoutList.Items.Add(menu1);
                            }
                            if (menuFlyoutList.Items.Count > 0)
                            {
                                // �Ƴ���һ��
                                menuFlyoutList.Items.RemoveAt(0);
                            }

                            splitButton.Flyout = menuFlyoutList;
                            break;
                        default:
                            Button button = new Button()
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Top,
                                Height = 50,
                                Width = 50,
                                Tag = GetButtonContentTag(),
                                Content = image
                            };
                            button.Click += Only_Button_Click;
                            grid.Children.Add(button);
                            break;
                    }
                    ActionItems.Children.Add(grid);

                    string GetButtonContentTag()
                    {
                        string tag = "";

                        foreach(var ac_item in item.actions)
                        {
                            if(ac_item.isEnable1)
                                tag = $"\"{ac_item.action1}\",{ac_item.title1}," +
                                    $"{ac_item.isShowNot},{ac_item.actionTimes}," +
                                    $"{ac_item.isEnable1}";
                            break;
                        }
                        return tag;
                    }
                }
            }
            else if (new SettingsManager().CheckBoolSetting("IsNoneItem"))
            {
                ;
            }
            else
            {
                var builder = new AppNotificationBuilder()
                    .AddText("�����ļ�����ʧ�� ��")
                    .AddText("�����޷���ȷ�ļ�����������ļ����⽫�����޷������޷��ڲ��������ʾ�����������");
                AppNotificationManager.Default.Show(builder.BuildNotification());
            }
        }

        private void SplitButton_Click(SplitButton sender, SplitButtonClickEventArgs args)
        {
            SplitButton spbutton = (SplitButton)sender;
            string Tag = (string)spbutton.Tag;

            if (Tag != "" || Tag != null)
            {
                List<string> parts = Tag.Split(',').ToList();

                if (parts.Count > 0 || bool.TryParse(parts[4], out bool isE))
                {
                    bool.TryParse(parts[2], out bool IsShow);
                    int.TryParse(parts[3], out int times);

                    SetUpApp(parts[0], parts[1], IsShow, times);
                }
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyout = (MenuFlyoutItem)sender;
            string Tag = (string)menuFlyout.Tag;

            if (Tag != "" || Tag != null)
            {
                List<string> parts = Tag.Split(',').ToList();

                if (parts.Count > 0 || bool.TryParse(parts[4], out bool isE))
                {
                    bool.TryParse(parts[2], out bool IsShow);
                    int.TryParse(parts[3], out int times);

                    SetUpApp(parts[0], parts[1], IsShow, times);
                }
            }
        }

        private void Only_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string Tag = (string)button.Tag;

            if(Tag!=""||Tag!=null)
            {
                List<string> parts = Tag.Split(',').ToList();

                if (parts.Count > 0 || bool.TryParse(parts[4],out bool isE))
                {
                    bool.TryParse(parts[2], out bool IsShow);
                    int.TryParse(parts[3], out int times);

                    SetUpApp(parts[0], parts[1], IsShow, times);
                }
            }
        }

        private void Only_HyButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton button = (HyperlinkButton)sender;
            string Tag = (string)button.Tag;

            if (Tag != "" || Tag != null)
            {
                List<string> parts = Tag.Split(',').ToList();

                if (parts.Count > 0 || bool.TryParse(parts[4], out bool isE))
                {
                    bool.TryParse(parts[2], out bool IsShow);
                    int.TryParse(parts[3], out int times);

                    SetUpApp(parts[0], parts[1], IsShow, times);
                }
            }
        }

    }
}
