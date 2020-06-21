using ConnOe.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnOe.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
        ServiceResponse<bool> MarkFulfilled(int id);
    }
}
