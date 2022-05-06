using JSONExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSONExplorer.Views.Controls
{
    /// <summary>
    /// JsonView.xaml 的互動邏輯
    /// </summary>
    public partial class JsonView : UserControl, IViews
    {
        public List<UIElement> Items { get; } = new List<UIElement>();

        public ViewModel ViewModel { get; }

        public JsonView()
        {

            InitializeComponent();

            ViewModel = DataContext as ViewModel;
            Navbar.DataContext = this;

        }

        public void AddNavbar(string title, Detail detail)
        {
            // 滑鼠左鍵連按2下觸發
            // https://stackoverflow.com/questions/1293530/how-to-bind-a-command-in-wpf-to-a-double-click-event-handler-of-a-control
            // https://docs.microsoft.com/dotnet/api/system.windows.input.mousebinding.gesture?view=windowsdesktop-6.0
            MouseBinding mouseBinding = new MouseBinding
            {
                Gesture = new MouseGesture(MouseAction.LeftDoubleClick),
                Command = new NavbarSelectCommand(ViewModel, Items.Count, detail)
            };

            // 選單
            MenuItem item = new MenuItem()
            {
                Header = String.Format("{0}", title),
                Cursor = Cursors.Hand
            };
            item.InputBindings.Add(mouseBinding); // 套用連點設定

            AddItem(item);
            Refresh();

            ViewModel.KeyView = new KeyView(detail);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        public void Refresh()
        {
            Navbar.Items.Refresh();
        }

        public void AddItem(UIElement menuItem)
        {
            Items.Add(menuItem);
        }

    }
    
    class NavbarSelectCommand : ICommand
    {
        private readonly ViewModel viewModel;
        private readonly int N_next;
        private readonly Detail detail;
        public NavbarSelectCommand(ViewModel viewModel, int index, Detail detail)
        {
            this.viewModel = viewModel;
            this.N_next = index + 1;
            this.detail = detail;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            while (N_next < viewModel.JsonView.Navbar.Items.Count)
            {
                viewModel.JsonView.RemoveAt(N_next);
            }
            viewModel.JsonView.Refresh();

            viewModel.KeyView = new KeyView(detail);
            viewModel.ValueView = new ValueView(detail, false);
        }
    }
}
