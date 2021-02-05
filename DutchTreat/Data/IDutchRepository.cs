using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        bool SaveChanges();

        void AddEntity(object model);

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetProductsByCategory(string category);

        Order GetOrderById(int id);

        IEnumerable<Order> GetAllOrders(bool includeItems);
    }
}