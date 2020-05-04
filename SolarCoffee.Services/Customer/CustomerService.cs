using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Add a new record
        /// </summary>
        /// <returns>Service response <Customer></Customer></returns>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer  customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "New customer added",
                    Time = DateTime.UtcNow,
                    Data = customer
                };

            }catch(Exception)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = "Failed to add customer",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
        }

        /// <summary>
        /// Delete the customer record
        /// </summary>
        /// <param name="id">Int customer id</param>
        /// <returns></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _db.Customers.Find(id);
            var now = DateTime.UtcNow;

            if(customer == null)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Customer Not Found",
                    Time = now,
                    Data = false
                };
            }

            try
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Customer Deleted",
                    Time = now,
                    Data = true
                };

            }
            catch(Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = now,
                    Data = false
                };
            }
        }

        /// <summary>
        /// Returns alist of customers from database
        /// </summary>
        /// <returns>List<Customer></Customer></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
           return _db.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        /// <summary>
        /// Get Customer record By Id
        /// </summary>
        /// <param name="id">int customer priomary key</param>
        /// <returns></returns>
        public Data.Models.Customer GetCustomerById(int id)
        {
            return _db.Customers
                .Find(id);
        }
    }
}
