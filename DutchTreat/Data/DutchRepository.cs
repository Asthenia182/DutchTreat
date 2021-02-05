using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            context.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems = true)
        {
            return includeItems
                ? context.Orders
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .ToList()
                : context.Orders.ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                logger.LogInformation("GetAllProducts was called");

                return context.Products
                     .OrderBy(x => x.Title)
                     .ToList();
            }
            catch (System.Exception ex)
            {
                logger.LogError("Failed to get all products: {ex}");
                return null;
            }
        }

        public Order GetOrderById(int id)
        {
            return context.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return context.Products
                .OrderBy(x => x.Category == category)
                .ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }
    }
}