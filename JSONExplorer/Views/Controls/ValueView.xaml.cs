using JSONExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace JSONExplorer.Views.Controls
{
    /// <summary>
    /// ValueView.xaml 的互動邏輯
    /// </summary>
    public partial class ValueView : UserControl
    {
        private Detail detail;
        private readonly bool isSelect;
        public ValueView(Detail detail, bool isSelect = true)
        {
            InitializeComponent();

            this.detail = detail;
            this.isSelect = isSelect;

            Refresh();

            // 修改
            btnSet.Click += (sender, e) =>
            {
                if (detail.SetValue(textBoxValue.Text))
                {
                    MessageBox.Show("修改成功", "確認視窗");
                    Refresh();
                }
                else
                {
                    MessageBox.Show("修改失敗", "確認視窗");
                }
            };
        }
        public void Refresh()
        {
            if (!isSelect)
            {
                Title.Visibility = Visibility.Collapsed;

                ScrollValues.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                Grid.SetRow(ScrollValues, 0);
                Grid.SetRowSpan(ScrollValues, 2);

                textBoxValue.Text = detail.GetString();
                textBoxValue.IsReadOnly = true;

                return;
            }

            object value = null;

            if (detail.DataType == typeof(Dictionary<string, object>))
            {
                ViewKey.Content = String.Format("View : {0}", detail.Key.ToString());
                value = detail.GetValue();
            }
            else if (detail.DataType == typeof(List<object>))
            {
                ViewKey.Content = String.Format("View : [{0}]", detail.ToString());
                value = detail.GetValue();
            }
            // 如果是string取消水平拖曳
            ScrollValues.HorizontalScrollBarVisibility =
                (value.GetType() == typeof(string)) ?
                    ScrollBarVisibility.Disabled : ScrollBarVisibility.Visible;

            textBoxValue.Text = detail.GetString();
        }
    }
}
