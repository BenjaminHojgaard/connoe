﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConnOe.Services.Customer
{
    public interface ICustomerService
    {
        public List<ConnOe.Data.Models.Customer> GetAllCustomers();
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer);
        public ServiceResponse<bool> DeleteCustomer(int id);
        public Data.Models.Customer GetById(int id);
    }
}
