using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmtApp.Controllers
{
    public class OrderController
    {
        private List<Models.TableOrder> _tableOrders;

        public OrderController() {
            _tableOrders = new List<Models.TableOrder>();
        }

        public List<Models.TableOrder> GetAllTableOrders()
        {
            return _tableOrders;
        }

        public bool MoveOrderItem(int srcTableOrderId, int srcOrderItemPos, int trgTableOrderId, int trgOrderItemPos)
        {
            // Check if source table order exist
            var srcTableOrder = _tableOrders.Where(to => to.ID == srcTableOrderId).FirstOrDefault();
            if (srcTableOrder == null)
            {
                return false;
            }
            // Check if source order item pos exist
            var srcOrderItem = srcTableOrder.Items.ElementAtOrDefault(srcOrderItemPos);
            if (srcOrderItem == null)
            {
                return false;
            }
            // Check if target table order exist
            var trgTableOrder = _tableOrders.Where(to => to.ID == trgTableOrderId).FirstOrDefault();
            if (trgTableOrder == null)
            {
                return false;
            }
            // Check if target order item pos exist or last pos
            if (trgOrderItemPos > trgTableOrder.Items.Count() || trgOrderItemPos < 0)
            {
                return false;
            }
            // Move order item from source to target table
            try
            {
                // Remove order item from source table
                srcTableOrder.Items.RemoveAt(srcOrderItemPos);
                // Insert (or append) order item to target table
                trgTableOrder.Items.Insert(trgOrderItemPos, srcOrderItem);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }
    }
}
