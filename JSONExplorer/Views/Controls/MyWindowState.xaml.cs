using JSONExplorer.ViewModels;
using System.Windows.Controls;
using static JSONExplorer.ViewModels.WindowControl;

namespace JSONExplorer.Views.Controls
{
    /// <summary>
    /// WindowSysCommandControl.xaml 的互動邏輯
    /// </summary>
    public partial class MyWindowState : UserControl
    {
        /* USE
        // <視窗控制>
        WindowControl windowControl = new WindowControl(this);
        this.SourceInitialized += new System.EventHandler(windowControl.Window_SourceInitialized); // 右下角拖曳?
        MyWindowState myWindowState = new MyWindowState(windowControl, "設定");
        Main.Children.Add(myWindowState);
        // </視窗控制>
        */
        public MyWindowState(WindowControl windowSyscommand, string title = "JSONExplorer")
        {
            InitializeComponent();

            Lab_title.Content = title;
            // <視窗控制>
            // 標題拖曳
            bool isMax = false;
            Lab_title.PreviewMouseLeftButtonDown += (sender, e) =>
            {
                windowSyscommand.ResizeWindow(SC.Move);
            };
            // 放大 / 回復原本大小
            Lab_title.MouseDoubleClick += (sender, e) =>
            {
                if (!isMax)
                {
                    windowSyscommand.ResizeWindow(SC.Maximize);
                    Btn_SizeMax.Content = '曰';
                }
                else
                {
                    windowSyscommand.ResizeWindow(SC.Restore);
                    Btn_SizeMax.Content = '口';
                }
                isMax = !isMax;
            };
            // 縮小
            Btn_SizeMin.Click += (sender, e) =>
            {
                windowSyscommand.ResizeWindow(SC.Minimize);
            };
            // 放大 / 回復原本大小
            Btn_SizeMax.Click += (sender, e) =>
            {
                if (!isMax)
                {
                    windowSyscommand.ResizeWindow(SC.Maximize);
                    Btn_SizeMax.Content = '曰';
                }
                else
                {
                    windowSyscommand.ResizeWindow(SC.Restore);
                    Btn_SizeMax.Content = '口';
                }
                isMax = !isMax;
            };
            // 關閉
            Btn_Close.Click += (sender, e) =>
            {
                windowSyscommand.ResizeWindow(SC.Close);
            };
            // </視窗控制>
        }
    }
}
