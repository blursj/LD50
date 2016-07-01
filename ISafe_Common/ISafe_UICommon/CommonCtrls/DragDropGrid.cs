using ISafe_UICommon.Adorners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ISafe_UICommon.CommonCtrls
{
    public class DragDropGrid : Grid
    {

        //记录拖拽元素的高度
        private static double DrapUIElementHeight = 0;

        //记录拖拽元素的宽度
        private static double DrapUIElementWidth = 0;

        //拖拽副本显示的装饰器
        private static DragAdorner adorner;

        private event DropOverHandler _DragDropOver;
        //拖放结束时触发的事件
        public event DropOverHandler DragDropOver
        {
            add
            {
                _DragDropOver += value;
            }
            remove
            {
                _DragDropOver -= value;
            }
        }

        public bool DragEnable
        {
            get { return (bool)GetValue(DragEnableProperty); }
            set { SetValue(DragEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DragEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragEnableProperty =
            DependencyProperty.Register("DragEnable", typeof(bool), typeof(DragDropGrid), new PropertyMetadata(false, new PropertyChangedCallback((sender, arg) =>
            {
                DragDropGrid _grid = sender as DragDropGrid;
                if (_grid != null)
                {
                    if (_grid.DragEnable)
                    {
                        _grid.PreviewMouseLeftButtonDown += grid_PreviewMouseLeftButtonDown;
                    }
                    else
                    {
                        _grid.PreviewMouseLeftButtonDown -= grid_PreviewMouseLeftButtonDown;
                    }
                }


            })));



        public bool DropEnable
        {
            get { return (bool)GetValue(DropEnableProperty); }
            set { SetValue(DropEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropEnableProperty =
            DependencyProperty.Register("DropEnable", typeof(bool), typeof(DragDropGrid), new PropertyMetadata(false, new PropertyChangedCallback((sender, arg) =>
            {
                DragDropGrid _grid = sender as DragDropGrid;
                if (_grid != null)
                {
                    if (_grid.DropEnable)
                    {
                        _grid.Drop += _grid_Drop;
                    }
                    else
                    {
                        _grid.Drop -= _grid_Drop;
                    }
                }
            })));

        /// <summary>
        /// 拖拽存放时发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _grid_Drop(object sender, DragEventArgs e)
        {
            var drag = sender as DragDropGrid;

            if (drag._DragDropOver != null)
            {
                object target = (e.OriginalSource as FrameworkElement).DataContext;
                var data = e.Data.GetData(typeof(ContentControl)) as ContentControl;
                drag._DragDropOver(data.Content, target);
            }
        }


        //开始拖拽
        static void grid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource == null)
            {
                return;
            }

            if (e.OriginalSource == sender)
            {
                return;
            }

            if ((e.OriginalSource as FrameworkElement).DataContext == null)
            {
                return;
            }

            DrapUIElementHeight = (e.OriginalSource as FrameworkElement).ActualHeight;
            DrapUIElementWidth = (e.OriginalSource as FrameworkElement).ActualWidth;

            UIElement findUI = FindDropUIElement(sender as UIElement) as UIElement;

            if (findUI == null)
            {
                return;
            }
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(FindDropUIElement(findUI));

            findUI.PreviewDragOver += DragDropGrid_PreviewDragOver;
            ContentControl contrl = new ContentControl();
            contrl.Content = (e.OriginalSource as FrameworkElement).DataContext;



            var dragData = new DataObject();

            adorner = new DragAdorner(findUI, contrl);
            adorner.LeftOffset = e.GetPosition(findUI).X - DrapUIElementWidth / 2;
            adorner.TopOffset = e.GetPosition(findUI).Y - DrapUIElementHeight / 2;
            layer.Add(adorner);

            //System.Windows.DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);

            System.Windows.DragDrop.DoDragDrop(findUI, contrl, DragDropEffects.Copy);

            if (adorner != null)
            {
                AdornerLayer.GetAdornerLayer(findUI).Remove(adorner);
                findUI.PreviewDragOver -= DragDropGrid_PreviewDragOver;
            }

            e.Handled = true;

        }


        /// <summary>
        /// 拖拽完后发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void DragDropGrid_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (adorner != null)
            {
                adorner.LeftOffset = e.GetPosition((sender as Grid)).X - DrapUIElementWidth / 2;
                adorner.TopOffset = e.GetPosition((sender as Grid)).Y - DrapUIElementHeight / 2;
                e.Handled = true;
            }
        }

        #region 功能函数区域



        #region FindGridChild

        /// <summary>
        /// Walks up the visual tree starting with the specified DependencyObject, 
        /// looking for a UIElement which is a child of the Canvas.  If a suitable 
        /// element is not found, null is returned.  If the 'depObj' object is a 
        /// UIElement in the Canvas's Children collection, it will be returned.
        /// </summary>
        /// <param name="depObj">
        /// A DependencyObject from which the search begins.
        /// </param>
        public UIElement FindCanvasChild(DependencyObject depObj)
        {
            while (depObj != null)
            {
                // If the current object is a UIElement which is a child of the
                // Canvas, exit the loop and return it.
                UIElement elem = depObj as UIElement;
                if (elem != null && base.Children.Contains(elem))
                    break;

                // VisualTreeHelper works with objects of type Visual or Visual3D.
                // If the current object is not derived from Visual or Visual3D,
                // then use the LogicalTreeHelper to find the parent element.
                if (depObj is Visual || depObj is Visual3D)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else
                    depObj = LogicalTreeHelper.GetParent(depObj);
            }

            return depObj as UIElement;
        }

        public static UIElement FindDropUIElement(DependencyObject depObj)
        {
            while (depObj != null)
            {
                // If the current object is a UIElement which is a child of the
                // Canvas, exit the loop and return it.
                UIElement elem = depObj as UIElement;


                if (elem != null && elem.AllowDrop == true)
                    break;

                // VisualTreeHelper works with objects of type Visual or Visual3D.
                // If the current object is not derived from Visual or Visual3D,
                // then use the LogicalTreeHelper to find the parent element.
                //if (depObj is Visual || depObj is Visual3D)
                //    depObj = VisualTreeHelper.GetParent(depObj);
                //else
                depObj = VisualTreeHelper.GetParent(depObj);

                if (depObj == null)
                {
                    return null;
                }
            }

            return depObj as UIElement;
        }

        #endregion // FindCanvasChild


        #endregion

        #region 拖放结束事件委托类型
        public delegate void DropOverHandler(object from, object to);

        #endregion

    }
}
