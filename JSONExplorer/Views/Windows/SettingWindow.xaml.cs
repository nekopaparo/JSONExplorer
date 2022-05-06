using JSONExplorer.Models;
using JSONExplorer.ViewModels;
using JSONExplorer.Views.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static JSONExplorer.ViewModels.WindowControl;

namespace JSONExplorer.Views.Windows
{
    /// <summary>
    /// Setter.xaml 的互動邏輯
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow(MainWindow M, Point point)
        {
            InitializeComponent();

            this.Left = point.X;
            this.Top = point.Y;
            
            // <視窗控制>
            WindowControl windowControl = new WindowControl(this);
            this.SourceInitialized += new System.EventHandler(windowControl.Window_SourceInitialized); // 右下角拖曳?
            MyWindowState myWindowState = new MyWindowState(windowControl, "設定");
            Main.Children.Add(myWindowState);
            Main.MouseLeftButtonDown += (sender, e) =>
            {
                windowControl.ResizeWindow(SC.Move);
            };
            myWindowState.Btn_SizeMin.Visibility = Visibility.Collapsed;
            // </視窗控制>


            BtnReSet.Click += (sender, e) =>
            {
                ((Setting)DataContext).ReSet();
            };
            
            this.MouseDown += (sender, e) =>
            {
                if (Keyboard.FocusedElement != null && Keyboard.FocusedElement.GetType() == typeof(TextBox))
                {
                    // https://stackoverflow.com/questions/5569768/textbox-binding-twoway-doesnt-update-until-focus-lost-wp7
                    var binding = (Keyboard.FocusedElement as TextBox).GetBindingExpression(TextBox.TextProperty);
                    binding.UpdateSource();

                    Keyboard.ClearFocus();
                }
            };
            this.Closed += (sender, e) =>
            {
                M.settingWindow = null;
            };
        }
    }
}
