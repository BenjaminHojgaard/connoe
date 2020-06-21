using ConnOe.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnOe.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ConnoeDbContext _db;
        public CustomerService(ConnoeDbContext dbContext)
        {
            _db = dbContext;
        }


        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer> { Data = customer, IsSuccess = true, Message = "New customer added.", Time = DateTime.UtcNow };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer> { Data = null, IsSuccess = false, Message = e.StackTrace, Time = DateTime.UtcNow };
            }
        }

        public ServiceResponse<bool> DeleteCustomer(int id)
        {

            var cust = _db.Customers.Find(id);

            if (cust == null)
            {
                return new ServiceResponse<bool> { Data = false, IsSuccess = false, Message = "Could not find customer to delete.", Time = DateTime.UtcNow };
                
            }

            try
            {
                _db.Customers.Remove(cust);
                _db.SaveChanges();
                return new ServiceResponse<bool> { Data = true, IsSuccess = true, Message = "Customer successfully deleted.", Time = DateTime.UtcNow };
            }
            catch (Exception e)
            {

                return new ServiceResponse<bool> { Data = false, IsSuccess = false, Message = e.StackTrace, Time = DateTime.UtcNow };
            }
           


        }

        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers.Include(customer => customer.PrimaryAddress).OrderBy(customer => customer.LastName).ToList();
        }

        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}
