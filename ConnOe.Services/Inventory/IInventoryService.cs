using ConnOe.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnOe.Services.Inventory
{
    public interface IInventoryService
    {
        public List<ProductInventory> GetCurrentInventory();
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        public ProductInventory GetByProductId(int id);
        public List<ProductInventorySnapshot> GetSnapshotHistory();
    }
}
