using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderMgmtApp.UIComponents
{
    public class RoundedPanel : UserControl
    {
        private int _borderRadius = 15;
        private Color _borderColor = Color.Silver;

        public RoundedPanel()
        {
            this.DoubleBuffered = true;
        }

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var path = GetRoundedRectanglePath(ClientRectangle))
            using (var pen = new Pen(_borderColor, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Region = new Region(path);
                e.Graphics.DrawPath(pen, path);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, _borderRadius, _borderRadius, 180, 90);
            path.AddArc(rect.Right - _borderRadius, rect.Y, _borderRadius, _borderRadius, 270, 90);
            path.AddArc(rect.Right - _borderRadius, rect.Bottom - _borderRadius, _borderRadius, _borderRadius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - _borderRadius, _borderRadius, _borderRadius, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            using (var path = GetRoundedRectanglePath(ClientRectangle))
            {
                Region = new Region(path);
            }
        }
    }
}
