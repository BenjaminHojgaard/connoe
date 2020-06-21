using ConnOe.Data;
using ConnOe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnOe.Services.Inventory
{
    public class InventoryService : IInventoryService
    {

        private readonly ConnoeDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(ConnoeDbContext dbContext, ILogger<InventoryService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        

        public ProductInventory GetByProductId(int id)
        {
            return _db.ProductInventories.Include(pInventory => pInventory.Product).FirstOrDefault(pInventory => pInventory.Product.Id == id);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                .Include(pInventory => pInventory.Product)
                .Where(pInventory => !pInventory.Product.IsArchived)
                .ToList();
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchived)
                .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _db.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception e)
                {

                    _logger.LogError("Error creating snapshot. Stacktrace: " + e.StackTrace);
                }


                _db.SaveChanges();

                return new ServiceResponse<ProductInventory> { Data = inventory, IsSuccess = true, Message = $"Product {id} inventory adjusted", Time = DateTime.UtcNow };
            }
            catch (Exception e)
            {

                return new ServiceResponse<ProductInventory> { Data = null, IsSuccess = false, Message = "Error updating Product Inventory. Error: " + e.StackTrace, Time = DateTime.UtcNow };
            }
        }
        private void CreateSnapshot(ProductInventory inventory)
        {
            var snapshot = new ProductInventorySnapshot { SnapshotTime = DateTime.UtcNow, Product = inventory.Product, QuantityOnHand = inventory.QuantityOnHand };
            _db.Add(snapshot);
        }
    }
}
