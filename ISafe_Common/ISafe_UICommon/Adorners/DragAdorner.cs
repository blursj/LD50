using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ISafe_UICommon.Adorners
{
    public class DragAdorner : Adorner
    {
        protected UIElement _child;

        public object Data
        {
            get;
            private set;
        }


        public DragAdorner(UIElement owner, UIElement element)
            : base(owner)
        {
            element.Opacity = 0.7;
            _child = element;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            this._child.Measure(finalSize);

            return this._child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._child.Arrange(new Rect(_child.DesiredSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _child;
        }

        private double _leftOffset;
        public double LeftOffset
        {
            get
            {
                return this._leftOffset;
            }
            set
            {
                this._leftOffset = value;
                this.UpdatePosition();
            }

        }

        private double _topOffset;
        public double TopOffset
        {
            get { return this._topOffset; }
            set
            {
                this._topOffset = value;
                this.UpdatePosition();
            }
        }


        private void UpdatePosition()
        {
            AdornerLayer adorner = (AdornerLayer)this.Parent;
            if (adorner != null)
            {
                adorner.Update(this.AdornedElement);
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(this._leftOffset, this._topOffset));
            return result;
        }
    }
}
