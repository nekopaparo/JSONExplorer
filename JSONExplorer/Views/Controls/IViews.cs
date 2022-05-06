using JSONExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSONExplorer.Views.Controls
{
    internal interface IViews
    {
        ViewModel ViewModel { get; }
        List<UIElement> Items { get; }
        void AddItem(UIElement element);
        void RemoveAt(int index);
        void Refresh();
    }
}
