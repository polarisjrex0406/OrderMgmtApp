using OrderMgmtApp.Models;
using OrderMgmtApp.UIComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderMgmtApp.Components
{
    public class TableOrderContainer : RoundedPanel
    {
        public TableOrder TableOrder { get; }

        public TableOrderContainer(TableOrder tableOrder)
        {
            this.AutoSize = false; // Disable auto-sizing
            // Set border properties
            BorderRadius = 10;
            BorderColor = Color.Silver;
            BackColor = Color.White;
            Padding = new Padding(8);
            TableOrder = tableOrder;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Controls.Clear();

            // Main vertical split container
            var mainSplit = new SplitContainer
            {
                Orientation = Orientation.Horizontal,
                Dock = DockStyle.Fill,
                SplitterWidth = 1,
                BorderStyle = BorderStyle.None,
                FixedPanel = FixedPanel.Panel1,
                SplitterDistance = 40,
                IsSplitterFixed = true,
            };

            // Header panel
            var header = new Panel { Dock = DockStyle.Fill };
            mainSplit.Panel1.Controls.Add(header);

            // Nested split container for items and footer
            var contentSplit = new SplitContainer
            {
                Orientation = Orientation.Horizontal,
                Dock = DockStyle.Fill,
                SplitterWidth = 1,
                BorderStyle = BorderStyle.None,
                FixedPanel = FixedPanel.Panel2,
                IsSplitterFixed = true,
            };

            // Items panel
            var itemsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true
            };

            // Footer panel
            var footer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Gainsboro,
                Height = 50
            };

            // Composition
            mainSplit.Panel2.Controls.Add(contentSplit);
            contentSplit.Panel1.Controls.Add(itemsPanel);
            contentSplit.Panel2.Controls.Add(footer);
            Controls.Add(mainSplit);

            // Header content
            var lblTitle = new Label
            {
                Text = $"{TableOrder.GuestName}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            header.Controls.Add(lblTitle);

            // Footer content
            var btnDelete = new Button
            {
                Image = Image.FromFile("Res/icons8-remove-32.png"),
                ImageAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                BackColor = Color.IndianRed,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            footer.Controls.Add(btnDelete);

            // Add items
            foreach (var item in TableOrder.Items)
            {
                itemsPanel.Controls.Add(new OrderItemControl(item));
            }
        }
    }
}
