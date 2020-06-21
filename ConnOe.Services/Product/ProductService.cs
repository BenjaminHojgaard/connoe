using ConnOe.Data;
using ConnOe.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnOe.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ConnoeDbContext _db;

        public ProductService(ConnoeDbContext dbContext)
        {
            _db = dbContext;
        }

        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            var product = _db.Products.Find(id);
            if (product != null)
            {
                try
                {
                    product.IsArchived = true;
                    _db.SaveChanges();
                    return new ServiceResponse<Data.Models.Product> { Data = product, IsSuccess = true, Message = "Successfully archived product.", Time = DateTime.UtcNow };
                }
                catch (Exception e)
                {

                    return new ServiceResponse<Data.Models.Product> { Data = null, IsSuccess = false, Message = "Failed to archive product.", Time = DateTime.UtcNow };
                }
            }

            return new ServiceResponse<Data.Models.Product> { Data = null, IsSuccess = false, Message = "Failed to archive product.", Time = DateTime.UtcNow };

        }

        public ServiceResponse<Data.Models.Product> CreatedProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);

                var newInventory = new ProductInventory { Product = product, QuantityOnHand = 0, IdealQuantity = 10, };

                _db.ProductInventories.Add(newInventory);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product> { Data = product, Time = DateTime.UtcNow, Message = "Saved new product", IsSuccess = true };
            }
            catch (Exception e)
            {

                return new ServiceResponse<Data.Models.Product> { Data = product, Time = DateTime.UtcNow, Message = "Error saving new product", IsSuccess = false };
            }
            


        }

        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }
    }
}
