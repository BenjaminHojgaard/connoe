using ConnOe.Data;
using ConnOe.Data.Models;
using ConnOe.Services.Inventory;
using ConnOe.Services.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnOe.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ConnoeDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(ILogger<OrderService> logger, ConnoeDbContext dbContext, IProductService productService, IInventoryService inventoryService)
        {
            _db = dbContext;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {

            _logger.LogInformation("Generating new order.");

            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);
                var inventoryId = _inventoryService.GetByProductId(item.Product.Id).Id;

                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            try
            {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();
                _logger.LogInformation($"Open order generated. ID: {order.Id}");
                return new ServiceResponse<bool> { Data = true, IsSuccess = true, Message = "Open order created", Time = DateTime.UtcNow };
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to create open order.");
                return new ServiceResponse<bool> { Data = false, IsSuccess = false, Message = e.StackTrace, Time = DateTime.UtcNow };
            }
        }

        public List<SalesOrder> GetOrders()
        {
            try
            {
                return _db.SalesOrders
                    .Include(o => o.Customer)
                    .ThenInclude(c => c.PrimaryAddress)
                    .Include(o => o.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                    .ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            try
            {
                var order = _db.SalesOrders.Find(id);
                order.UpdatedOn = DateTime.UtcNow;
                order.IsPaid = true;
                _db.SalesOrders.Update(order);
                _db.SaveChanges();
                return new ServiceResponse<bool> { Data = true, IsSuccess = true, Message = $"Order {order.Id} closed: Invoice paid in full.", Time = DateTime.UtcNow };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool> { Data = false, IsSuccess = false, Message = e.StackTrace, Time = DateTime.UtcNow };
                
            }
        }
    }
}
