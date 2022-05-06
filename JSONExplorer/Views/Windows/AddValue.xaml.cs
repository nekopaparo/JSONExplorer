using JSONExplorer.Models;
using JSONExplorer.ViewModels;
using JSONExplorer.Views.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using static JSONExplorer.ViewModels.WindowControl;

namespace JSONExplorer.Views.Windows
{
    /// <summary>
    /// AddValue.xaml 的互動邏輯
    /// </summary>
    internal enum ValueType
    {
        StringOrDigit,
        Dictionary,
        List
    }
    public partial class AddValue : Window
    {
        private ValueType ValueType;

        public AddValue(KeyView keyView)
        {
            InitializeComponent();

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

            // Value Type Select
            StrDig.Checked += (sender, e) =>
            {
                ValueType = ValueType.StringOrDigit;
                ValueV.Visibility = Visibility.Visible;
            };
            AddDictionary.Checked += (sender, e) =>
            {
                ValueType = ValueType.Dictionary;
                ValueV.Visibility = Visibility.Hidden;
                System.Console.WriteLine("g");
            };
            AddList.Checked += (sender, e) =>
            {
                ValueType = ValueType.List;
                ValueV.Visibility = Visibility.Hidden;
            };

            // Key
            Type type = keyView.Value.GetType();
            if (type == typeof(Dictionary<string, object>))
            {
                Dictionary<string, object> P = keyView.Value as Dictionary<string, object>;
                string key;
                for (int i = 0; i < 1000; i++)
                {
                    key = String.Format("Key{0}", i);
                    if (!P.ContainsKey(key))
                    {
                        ValueK.Text = key;
                        break;
                    }
                };
                Submit.Click += (sender, e) =>
                {
                    if ((keyView.Value as Dictionary<string, object>).ContainsKey(ValueK.Text))
                    {
                        //
                    }
                    else
                    {
                        (keyView.Value as Dictionary<string, object>).Add(ValueK.Text, GetAddValue());
                        keyView.AddKeyButton(ValueK.Text, new ValueViewShowCommand(keyView, ValueK.Text), new KeyViewSelectCommand(keyView, ValueK.Text), ValueK.Text);
                    }
                };
            }
            else if (type == typeof(List<object>))
            {
                List<object> P = keyView.Value as List<object>;
                int index = P.Count;
                string title = String.Format("[{0}]", index);
                ValueK.Text = title;
                ValueK.IsEnabled = false;
                Submit.Click += (sender, e) =>
                {
                    P.Add(GetAddValue());
                    keyView.AddKeyButton(title, new ValueViewShowCommand(keyView, index), new KeyViewSelectCommand(keyView, index), index);
                };
            }
            else
            {
                AddDictionary.IsChecked = true;
                StrDig.Visibility = Visibility.Hidden;
                ValueK.Visibility = Visibility.Hidden;
                Submit.Click += (sender, e) =>
                {
                    if (keyView.detail.SetValue(GetAddValue()))
                    {
                        keyView.Value = keyView.detail.GetValue();
                    }
                };
            }

            Submit.Click += (sender, e) =>
            {
                keyView.Refresh();
                keyView.Summary.Refresh();
                keyView.ViewModel.ValueView = keyView.Summary;
                windowControl.ResizeWindow(SC.Close);
            };

        }
        private object GetAddValue()
        {
            switch (ValueType)
            {
                case ValueType.StringOrDigit:
                    return ValueV.Text;
                case ValueType.Dictionary:
                    return JsonExplorer.GetDictionary;
                case ValueType.List:
                    return  JsonExplorer.GetList;
                default:
                    return null;
            }
        }
    }
}
