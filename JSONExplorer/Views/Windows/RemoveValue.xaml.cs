using JSONExplorer.Views.Controls;
using System.Windows;

namespace JSONExplorer.Views.Windows
{
    /// <summary>
    /// RemoveValue.xaml 的互動邏輯
    /// </summary>
    public partial class RemoveValue : Window
    {
        public RemoveValue(KeyView keyView, object key, Point point)
        {
            InitializeComponent();

            this.Left = point.X;
            this.Top = point.Y;

            this.PreviewLostKeyboardFocus += (sender, e) =>
            {
                this.Close();
            };

            RemoveBtn.PreviewMouseLeftButtonDown += (sender, e) =>
            {
                keyView.RemoveKey(key);
                keyView.ViewModel.ValueView.Refresh();
            };
        }
    }
}
