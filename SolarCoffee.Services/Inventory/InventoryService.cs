using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext dbContext, ILogger<InventoryService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Gets a procut by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventorys
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }

        /// <summary>
        /// Return all current inventory from the database
        /// </summary>
        /// <returns></returns>

        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventorys
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Return Snapshot History for Previous 6 hours
        /// </summary>
        /// <returns></returns>
        public List<ProductInventorySnapshot> GetSnapShotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(
                    snap => snap.SnapShotTime > earliest
                        && !snap.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Updates no of units available of the provided productId
        /// Adjust QuantityOnHand by adjustment value
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="adjustment">number of products added / removed from inventory</param>
        /// <returns></returns>
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            var now = DateTime.UtcNow;
            try
            {
                var inventory = _db.ProductInventorys
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapShot();
                }catch(Exception e)
                {
                    _logger.LogError("Error creating inventory snapshot");
                    _logger.LogError(e.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = inventory,
                    Message = $"Product {id} Inventory adjusted",
                    Time = now
                };

            }catch(Exception)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = null,
                    Message = $"Error Updating Product Quantity on Hand",
                    Time = now
                };
            }
        }

        /// <summary>
        /// Create a snapshot record using the provided Inventory Instance
        /// </summary>
        /// <param name="productInventory"></param>
        private void CreateSnapShot()
        {
            var now = DateTime.UtcNow;

            var inventories = _db.ProductInventorys
                .Include(inv => inv.Product)
                .ToList();

            foreach (var inventory in inventories)
            {
                var snapshot = new ProductInventorySnapshot
                {
                    SnapShotTime = now,
                    Product = inventory.Product,
                    QuantityOnHand = inventory.QuantityOnHand
                };
                _db.Add(snapshot);

            }
            _db.SaveChanges();
        }
    }
}
