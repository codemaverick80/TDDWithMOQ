using MOQExample.Domain;
using System.Collections.Generic;

namespace MOQExample.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product Get(int id);

        bool AddProduct(Product product);
    }
}