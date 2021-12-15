using Microsoft.Extensions.Logging;
using MOQExample.Domain;
using MOQExample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOQExample.Controllers
{
    public class ProductController
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;
        public ProductController(IProductRepository productService, ILogger logger=null)
        {
            _productRepository = productService;
            _logger = logger;
        }

        public List<Product> GetProducts()
        {
            var products = _productRepository.GetAll();
           
            return products;
        }

        public Product GetById(int id)
        {
            try
            {
                var result = _productRepository.Get(id);
                return result;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message);
                throw;
            }          
        }

        public bool AddProduct(Product product)
        {
            //if (string.IsNullOrEmpty(product.Name)) throw new ArgumentNullException("Product name can not be null or empty");

            //if (product.Id <= 0) throw new ArgumentOutOfRangeException("Product id should be greater then 0");

            var result= _productRepository.AddProduct(product);
            return result;
        }
    }
}
