using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace JSONExplorer.ViewModels
{
    // 拖曳功能
    // https://www.uj5u.com/net/6421.html
    // https://blog.csdn.net/wcc27857285/article/details/78051333

    public class WindowControl
    {
        private DependencyObject window;
        private HwndSource hwndSource;
        // WM_SYSCOMMAND 參數 : https://docs.microsoft.com/windows/win32/menurc/wm-syscommand
        private const int WM_SYSCOMMAND = 0x112;
        public enum SC
        {
            Move = 0xF012,
            Minimize = 0xF020,
            Maximize = 0xF030,
            Restore = 0xF120,
            Close = 0xF060
        }
        // Windows系統函式庫, User32.dll提供創建和管理Windows圖形界面的功能
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public WindowControl(DependencyObject window)
        {
            this.window = window;
        }

        public void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            hwndSource = HwndSource.FromVisual(Window.GetWindow(window)) as HwndSource;
            hwndSource.AddHook(new HwndSourceHook(HandleWindowMessage));
        }
        
        private IntPtr HandleWindowMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Debug.WriteLine("WndProc messages: " + msg.ToString());

            if (msg == WM_SYSCOMMAND)
            {
                //Debug.WriteLine("WndProc messages: " + msg.ToString());
            }

            return IntPtr.Zero;
        }

        public void ResizeWindow(SC wParam)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)wParam, IntPtr.Zero);
        }


        // 獲得滑鼠位置 https://www.cnblogs.com/sntetwt/p/11477939.html
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out IntPoint lpPoint);
        public void GetMousePoint(out IntPoint P)
        {
            GetCursorPos(out P);
        }
        public struct IntPoint
        {
            public int X, Y;
            public IntPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
