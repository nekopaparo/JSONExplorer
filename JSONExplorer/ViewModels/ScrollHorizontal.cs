using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSONExplorer.ViewModels
{
    // 水平滑動
    class ScrollHorizontal
    {
        private ScrollViewer ScrollViewer { get; }
        private bool isScrollMove = false;
        private double HorizontalPosition { get; set; }
        private double MouseDownPoint_X { get; set; }
        public ScrollHorizontal(ScrollViewer scrollViewer, bool Vertical = false)
        {
            this.ScrollViewer = scrollViewer;
            // 設定
            scrollViewer.Focusable = false; // 取消關注
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled; // 關閉垂直
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden; // 開啟水平(無拉條)
            // 水平移動
            HorizontalPosition = scrollViewer.HorizontalOffset;

            scrollViewer.MouseLeave += MoveEnd;
            scrollViewer.PreviewMouseUp += MoveEnd;

            scrollViewer.PreviewMouseDown += (sender, e) =>
            {
                isScrollMove = true;
                MouseDownPoint_X = GetPoint_X;
            };
            
            scrollViewer.MouseMove += (sender, e) =>
            {
                if (isScrollMove)
                {
                    ScrollViewer.ScrollToHorizontalOffset(HorizontalPosition + MouseDownPoint_X - GetPoint_X);
                }
            };
            if (Vertical)
            {
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden; // 開啟垂直
                // 滾輪滑動
                scrollViewer.PreviewMouseWheel += (sender, e) =>
                {
                    if (e.Delta > 0)
                    {
                        scrollViewer.LineDown();
                    }
                    else if (e.Delta < 0)
                    {
                        scrollViewer.LineUp();
                    }
                };
            }
            else
            {
                // 滾輪滑動
                scrollViewer.PreviewMouseWheel += (sender, e) =>
                {
                    if (e.Delta > 0)
                    {
                        scrollViewer.LineLeft();
                    }
                    else if (e.Delta < 0)
                    {
                        scrollViewer.LineRight();
                    }
                    // 紀錄目前位置
                    HorizontalPosition = scrollViewer.HorizontalOffset;
                };
            }
        }

        private void MoveEnd(object sender, RoutedEventArgs e)
        {
            isScrollMove = false;
            HorizontalPosition = ScrollViewer.HorizontalOffset; // 紀錄目前位置
        }

        private double GetPoint_X => Mouse.GetPosition(ScrollViewer).X;

    }
}
