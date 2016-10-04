using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;


namespace NetDXFViewer
{
    public class ZoomBorder : Border
    {
        private UIElement _child = null;
        private Point _origin;
        private Point _start;

            private TranslateTransform GetTranslateTransform(UIElement element)
    {
      return (TranslateTransform)((TransformGroup)element.RenderTransform)
        .Children.First(tr => tr is TranslateTransform);
    }

    private ScaleTransform GetScaleTransform(UIElement element)
    {
      return (ScaleTransform)((TransformGroup)element.RenderTransform)
        .Children.First(tr => tr is ScaleTransform);
    }
        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                {
                    this.Initialize(value);
                }
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            _child = element;
            if (_child != null)
            {
                var group = new TransformGroup();
                var st = new ScaleTransform();
                group.Children.Add(st);
                var tt = new TranslateTransform();
                group.Children.Add(tt);
                _child.RenderTransform = group;
                _child.RenderTransformOrigin = new Point(0.0, 0.0);
                this.MouseWheel += Child_MouseWheel;
                this.MouseLeftButtonDown += Child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += Child_MouseLeftButtonUp;
                this.MouseMove += Child_MouseMove;
                this.PreviewMouseRightButtonDown += Child_PreviewMouseRightButtonDown;
            }
        }

        public void Reset(double gridHeight,double gridWidth)
        {
            if (_child != null)
            {
            	double hauteurVu = 100;
            	
            	//double scale = Application.Current.MainWindow.Height/(hauteurVu);
            	double scale = ((Grid)Application.Current.MainWindow.Content).ActualHeight/(hauteurVu);
            	if(scale == 0)
            	{
            		scale=1.0;
            		hauteurVu=462;
            	}
            	var st = GetScaleTransform(_child);
                /*st.ScaleX = 1.0;
                st.ScaleY = 1.0;*/
                st.ScaleX = scale;
                st.ScaleY = scale;
                var tt = GetTranslateTransform(_child);
                tt.X = -gridWidth*scale/2;
                tt.Y = -gridHeight*scale/2-(Application.Current.MainWindow.Height*scale)/2+hauteurVu*scale;
                //tt.X = -gridWidth/2;
                //tt.Y = -gridHeight/2;
            }
        }
        
        public void Reset()
        {
        	Reset(this.Height,this.Width);
        }
        
        private void Child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_child != null)
            {
                var st = GetScaleTransform(_child);
                var tt = GetTranslateTransform(_child);

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                {
                    return;
                }

                Point relative = e.GetPosition(_child);
                double abosuluteX;
                double abosuluteY;
                abosuluteX = relative.X * st.ScaleX + tt.X;
                abosuluteY = relative.Y * st.ScaleY + tt.Y;
                st.ScaleX += zoom;
                st.ScaleY += zoom;
                tt.X = abosuluteX - relative.X * st.ScaleX;
                tt.Y = abosuluteY - relative.Y * st.ScaleY;
               
            }
        }

        private void Child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_child != null)
            {
                var tt = GetTranslateTransform(_child);
                _start = e.GetPosition(this);
                _origin = new Point(tt.X, tt.Y);
                this.Cursor = Cursors.Hand;
                _child.CaptureMouse();
            }
        }

        private void Child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_child != null)
            {
                _child.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
            }
        }

        void Child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void Child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_child != null)
            {
                if (_child.IsMouseCaptured)
                {
                    var tt = GetTranslateTransform(_child);
                    Vector v = _start - e.GetPosition(this);
                    tt.X = _origin.X - v.X;
                    tt.Y = _origin.Y - v.Y;
                }
            }
        }
    }
}
