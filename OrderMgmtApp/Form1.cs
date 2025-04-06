using OrderMgmtApp.Components;
using OrderMgmtApp.Controllers;
using OrderMgmtApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OrderMgmtApp
{
    public partial class Form1 : Form
    {
        private OrderController _orderController = new OrderController();
        private List<TableOrderContainer> _containers = new List<TableOrderContainer>();
        private const int ContainerWidth = 300;
        private FlowLayoutPanel _flowLayout;
        private Panel _scrollPanel;

        public Form1()
        {
            // Auto-generated code
            InitializeComponent();
            // User-written code
            InitializeUIComponents();

            LoadSampleData();

            ConfigureEventHandlers();
        }

        private void InitializeUIComponents()
        {
            // Main layout
            this.Size = new System.Drawing.Size(1200, 800);
            this.Text = "Table Order Management";
            this.DoubleBuffered = true;

            // Scrollable container area
            _scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Container holder
            _flowLayout = new FlowLayoutPanel
            {
                //Dock = DockStyle.Left,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };

            // Add button
            var addButton = new Button
            {
                Text = "+ Add Table",
                Size = new System.Drawing.Size(60, 60),
                Font = new System.Drawing.Font("Arial", 14),
                Dock = DockStyle.Right,
                Margin = new Padding(20)
            };
            addButton.Click += AddTableButton_Click;

            // Layout composition
            _scrollPanel.Controls.Add(_flowLayout);
            this.Controls.Add(addButton);
            this.Controls.Add(_scrollPanel);
        }

        private void ConfigureEventHandlers()
        {
            this.Resize += (s, e) => RefreshContainers();
            _scrollPanel.Resize += (s, e) => RefreshContainers();
        }

        private void LoadSampleData()
        {
            // Sample table orders
            var table1 = new TableOrder {
                ID = 1,
                GuestName = "Guest 1",
                Items = new List<OrderItem>(),
            };

            var productNames = new string[]
            {
                "Bat Energia", "Cheese Cake Frutos Rojos Ind", "Chocolate Porc", "Zanahoria Entera"
            };

            var prices = new double[]
            {
                22222.221, 29629.63, 11111.111, 70703.704
            };

            for (int i = 0; i < productNames.Length; i++)
            {
                table1.Items.Add(new OrderItem
                {
                    BaseProduct = new Product
                    {
                        ID = i + 1,
                        Name = productNames[i],
                        Price = prices[i],
                    },
                    Quantity = 1,
                    Modifiers = new List<ProductModifier>(),
                });
            }

            _orderController.GetAllTableOrders().AddRange(new[] { table1 });
            RefreshContainers();
        }

        private void RefreshContainers()
        {
            try
            {
                _flowLayout.SuspendLayout();

                // Only recreate when necessary
                var existing = _containers.ToArray();
                var needed = _orderController.GetAllTableOrders().ToArray();

                // Modified using standard LINQ
                foreach (var container in existing
                    .Where(e => !needed.Any(n => n.ID == e.TableOrder.ID)))
                {
                    _flowLayout.Controls.Remove(container);
                    _containers.Remove(container);
                }

                foreach (var order in needed)
                {
                    var container = _containers.FirstOrDefault(c => c.TableOrder.ID == order.ID)
                                    ?? new TableOrderContainer(order);
                    // Update existing container
                    container.Height = CalculateContainerHeight();
                    container.Width = ContainerWidth;

                    if (!_containers.Contains(container))
                    {
                        _containers.Add(container);
                        _flowLayout.Controls.Add(container);
                    }
                }

                //UpdateScrollState();
            }
            finally
            {
                _flowLayout.ResumeLayout(true);
            }
        }


        private int CalculateContainerHeight()
        {
            int visibleHeight = _scrollPanel.ClientSize.Height - _scrollPanel.Padding.Vertical;
            return visibleHeight;
        }

        private void AddTableButton_Click(object sender, System.EventArgs e)
        {
            var newTable = new TableOrder
            {
                ID = _orderController.GetAllTableOrders().Count + 1,
                GuestName = $"Guest {_orderController.GetAllTableOrders().Count + 1}",
                Items = new List<OrderItem>(),
            };
            _orderController.GetAllTableOrders().Add(newTable);
            RefreshContainers();
        }
    }
}
