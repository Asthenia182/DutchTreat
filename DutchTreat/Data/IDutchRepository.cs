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

        Order GetOrderById(string userName, int id);

        IEnumerable<Order> GetAllOrdersByUser(string userName, bool includeItems = true);
    }
}