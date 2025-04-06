using OrderMgmtApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderMgmtApp.Components
{
    public class OrderItemControl : UserControl
    {
        public OrderItem Item { get; }

        public OrderItemControl(OrderItem item)
        {
            Item = item;
            InitializeComponents();
            // Enable full control mouse interaction
            SetStyle(ControlStyles.EnableNotifyMessage, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void InitializeComponents()
        {
            // Main layout
            this.Size = new Size(280, 60);
            this.BackColor = Color.LightGray;
            this.Margin = new Padding(5);

            // Quantity label
            var lblQuantity = new Label
            {
                Text = Item.Quantity.ToString(),
                Font = new Font("Arial", 14),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Left,
                BackColor = Color.Transparent,
            };

            // Product info
            var productPanel = new Panel {
                Dock = DockStyle.Left,
                BackColor = Color.Transparent,
            };
            var lblProduct = new Label
            {
                Text = Item.BaseProduct.Name,
                Font = new Font("Arial", 12),
                Top = 5
            };

            // Modifiers
            var modifierPanel = new FlowLayoutPanel
            {
                Top = 25,
                Height = 20,
                AutoSize = true
            };

            foreach (var modifier in Item.Modifiers)
            {
                modifierPanel.Controls.Add(new Label
                {
                    Text = $"→ {modifier.Name}",
                    Font = new Font("Arial", 9, FontStyle.Italic),
                    ForeColor = Color.DarkGray
                });
            }

            // Composition
            productPanel.Controls.Add(lblProduct);
            productPanel.Controls.Add(modifierPanel);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(productPanel);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var container = FindParentContainer();
            if (container != null)
            {
                var data = new DataObject();
                data.SetData("OrderItem", Item);
                data.SetData("SourceContainer", container);
                data.SetData("DragImage", CaptureControlImage());
                DoDragDrop(data, DragDropEffects.Move);
            }
        }

        private Bitmap CaptureControlImage()
        {
            var bmp = new Bitmap(Width, Height);
            DrawToBitmap(bmp, new Rectangle(0, 0, Width, Height));
            bmp.MakeTransparent(BackColor);
            return bmp;
        }

        private TableOrderContainer FindParentContainer()
        {
            Control parent = Parent;
            while (parent != null && !(parent is TableOrderContainer))
                parent = parent.Parent;
            return parent as TableOrderContainer;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }
    }
}
