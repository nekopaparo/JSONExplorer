using JSONExplorer.ViewModels;
using JSONExplorer.Views.Controls;
using JSONExplorer.Views.Windows;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;

namespace JSONExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SettingWindow settingWindow = null;
        public MainWindow()
        {
            InitializeComponent();

            (DataContext as ViewModel).MainWindow = this;

            // <視窗控制>
            WindowControl windowControl = new WindowControl(this);
            this.SourceInitialized += new System.EventHandler(windowControl.Window_SourceInitialized); // 右下角拖曳?
            MyWindowState MyWindowState = new MyWindowState(windowControl);
            Main.Children.Add(MyWindowState);
            // </視窗控制>

            // 檔案(新增)
            // https://docs.microsoft.com/zh-tw/dotnet/desktop/winforms/controls/how-to-save-files-using-the-savefiledialog-component?view=netframeworkdesktop-4.8
            // https://stackoverflow.com/questions/45230880/json-files-arent-shown-in-open-file-dialog-while-in-the-filter
            NewFile.MouseLeftButtonDown += (sender, e) =>
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Json files (*.json) | *.json";
                saveFile.Title = "New Json File";
                if (saveFile.ShowDialog() == true)
                {
                    //System.Console.WriteLine(saveFile.FileName);
                    (DataContext as ViewModel).Path = "NewJson" + saveFile.FileName;
                }
            };
            // 檔案(開啟)
            // https://www.796t.com/content/1545279310.html
            OpenFile.MouseLeftButtonDown += (sneder, e) =>
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Json files (*.json) | *.json";
                openFile.Title = "Open Json File";
                if (openFile.ShowDialog() == true)
                {
                    //System.Console.WriteLine(openFile.FileName);
                    (DataContext as ViewModel).Path = openFile.FileName;
                }
            };
            // 另存檔案
            SaveAsFile.MouseLeftButtonDown += (sender, e) =>
            {
                if ((DataContext as ViewModel).JsonData == null)
                {
                    return;
                }
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Json files (*.json) | *.json";
                saveFile.Title = "New Json File";
                if (saveFile.ShowDialog() == true)
                {
                    //System.Console.WriteLine(saveFile.FileName);
                    string path = saveFile.FileName;
                    if (!Regex.Match(path, @"\w+\.json\b").Success)
                    {
                        path += ".json";
                    }
                    if ((DataContext as ViewModel).SaveJson(path))
                    {
                        MessageBox.Show("另存成功", "確認視窗");
                    }
                    else
                    {
                        MessageBox.Show("另存失敗", "確認視窗");
                    }
                }
            };
            // 齒輪
            SettingWindowOpenBtn.Click += (sender, e) =>
            {
                if (settingWindow == null)
                {
                    settingWindow = new SettingWindow(this, (DataContext as ViewModel).GetPosition());
                    settingWindow.Show();
                }
            };
            // 存檔 (Save)
            Btn_Save.Click += (sender, e) =>
            {
                if ((DataContext as ViewModel).JsonData == null)
                {
                    return;
                }
                if ((DataContext as ViewModel).SaveJson())
                {

                    MessageBox.Show("儲存成功", "確認視窗");
                }
                else
                {
                    MessageBox.Show("儲存失敗", "確認視窗");
                }
            };
            Btn_folder.Click += (sender, e) =>
            {
                if ((DataContext as ViewModel).Path == null)
                {
                    return;
                }
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", Regex.Replace((DataContext as ViewModel).Path, @"\\\w+.json\b", ""));
                }
                catch { }
            };
            // 水平(拖曳+滾輪)滑動匯入
            _ = new ScrollHorizontal(NavbarScroll);
        }
    }
}
