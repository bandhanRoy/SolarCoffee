using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(SolarDbContext dbContext,
            ILogger<OrderService> logger,
            IProductService productService,
            IInventoryService inventoryService)
        {
            _db = dbContext;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Create an Open Sales Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            var now = DateTime.UtcNow;

            _logger.LogInformation("Generating New Order");

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

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Time = now,
                    Message = "Open Order Created"
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Time = now,
                    Message = e.StackTrace
                };
            }
        }

        /// <summary>
        /// Get All Orders from the data base
        /// </summary>
        /// <returns></returns>
        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
                .Include(salesOrderCustomer => salesOrderCustomer.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                .Include(salesOrderItem => salesOrderItem.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();
        }

        /// <summary>
        /// Marks an Open Sales Order as Paid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var now = DateTime.UtcNow;
            var order = _db.SalesOrders.Find(id);
            order.UpdatedOn = now;
            order.IsPaid = true;

            try
            {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Time = now,
                    Message = $"Order {order.Id} closed: Invoice paid in Full!"
                };
            }catch(Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Time = now,
                    Message = e.StackTrace
                };
            }
        }
    }
}
