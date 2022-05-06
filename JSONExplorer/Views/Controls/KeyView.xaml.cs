using JSONExplorer.ViewModels;
using JSONExplorer.Views.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSONExplorer.Views.Controls
{
    /// <summary>
    /// DictionaryKey.xaml 的互動邏輯
    /// </summary>
    public partial class KeyView : UserControl, IViews
    {
        public List<UIElement> Items { get; } = new List<UIElement>();
        public ViewModel ViewModel { get; }
        public ValueView Summary { get; }
        public Detail detail;
        public object Value;

        public KeyView(Detail detail)
        {
            InitializeComponent();
            Keys.DataContext = this;
            ViewModel = DataContext as ViewModel;
            this.detail = detail;
            this.Value = detail.GetValue();
            Summary = new ValueView(detail, false);
            ViewModel.ValueView = Summary;


            Type type = Value.GetType();
            if (type == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> kvp in (Dictionary<string, object>)Value)
                {
                    AddKeyButton(kvp.Key, new ValueViewShowCommand(this, kvp.Key), new KeyViewSelectCommand(this, kvp.Key), kvp.Key);
                }
            }
            else if (type == typeof(List<object>))
            {
                int index = 0;
                foreach (object obj in (List<object>)Value)
                {
                    string title = String.Format("[{0}]", index);
                    AddKeyButton(title, new ValueViewShowCommand(this, index), new KeyViewSelectCommand(this, index), index);
                    index++;
                }
            }
            else
            {
                //
            }
            // 新增按鈕
            CreateKeyBtn.Click += (sender, e) =>
            {
                AddValue addValue = new AddValue(this);
                addValue.Show();
            };
        }

        public void AddKeyButton(string content, ICommand command, ICommand doubleClickCommand, object key)
        {
            Button button = new Button
            {
                Content = content,
                Command = command
            };
            AddLeftDoubleClickMouseCommand(button, doubleClickCommand);
            button.MouseRightButtonDown += (sender, e) =>
            {
                new RemoveValue(ViewModel.KeyView, key, ViewModel.GetPosition()).Show();
            };
            AddItem(button);
        }
        private void AddLeftDoubleClickMouseCommand(UIElement element, ICommand command)
        {
            // 滑鼠左鍵連按2下觸發
            MouseBinding mouseBinding = new MouseBinding
            {
                Gesture = new MouseGesture(MouseAction.LeftDoubleClick),
                Command = command
            };
            // 套用連點設定
            element.InputBindings.Add(mouseBinding); 
        }

        public void RemoveKey(object key)
        {
            Type type = Value.GetType();
            if (type == typeof(Dictionary<string, object>))
            {
                int index = 0;
                foreach (KeyValuePair<string, object> kvp in Value as Dictionary<string, object>)
                {
                    if (kvp.Key == (key as string))
                    {
                        break;
                    }
                    ++index;
                }
                if ((Value as Dictionary<string, object>).Remove(key as string))
                {
                    Items.RemoveAt(index);
                }
                
            }
            else if (type == typeof(List<object>))
            {
                (Value as List<object>).RemoveAt((int)key);
                Items.RemoveAt((int)key);
            }
            else
            {
                //
            }
            Refresh();
        }

        public void AddItem(UIElement button)
        {
            Items.Add(button);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        public void Refresh()
        {
            Keys.Items.Refresh();
        }
    }

    
    class KeyViewSelectCommand : ICommand
    {
        private readonly KeyView keyView;
        private readonly string title;
        private readonly object key;

        // Dictionary
        public KeyViewSelectCommand(KeyView keyView, string key)
        {
            this.keyView = keyView;
            this.key = key;
            this.title = String.Format("[\"{0}\"]", key);
        }
        // List
        public KeyViewSelectCommand(KeyView keyView, int index)
        {
            this.keyView = keyView;
            this.key = index;
            this.title = String.Format("[{0}]", index);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            keyView.ViewModel.JsonView.AddNavbar(title, new Detail(keyView.Value, key));
        }
    }
    // </Keys>


    class ValueViewShowCommand : ICommand
    {
        private readonly KeyView keyView;
        private readonly object key;

        public ValueViewShowCommand(KeyView keyView, object key)
        {
            this.keyView = keyView;
            this.key = key;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            keyView.ViewModel.ValueView = new ValueView(new Detail(keyView.Value, key));
        }
    }
}
