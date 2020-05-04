 using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productsService;

        public ProductController(ILogger<ProductController> logger, IProductService productsService) {
            _logger = logger;
            _productsService = productsService;
        }

        /// <summary>
        /// Get All Product
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");
            var products = _productsService.GetAllProducts();
            var productViewModule = products.Select(ProductMapper.SerializeProductModel);
            return Ok(productViewModule);
        }

        /// <summary>
        /// Archive a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation($"Archiving product {id}");
            var archiveResult = _productsService.ArchiveById(id);
            return Ok(archiveResult); 
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("/api/product")]
        public ActionResult AddProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Adding new product");
            var newProduct = ProductMapper.SerializeProductModel(product);
            var newproductResponse = _productsService.CreateProduct(newProduct);
            return Ok(newproductResponse);
        }
    }
}
