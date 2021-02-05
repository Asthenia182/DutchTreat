using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext ctx;
        private readonly IWebHostEnvironment hosting;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment hosting, UserManager<StoreUser>userManager)
        {
            this.ctx = ctx;
            this.hosting = hosting;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            ctx.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("agembarzewska@gmail.com") ;
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Aga",
                    LastName = "Gembarzewska",
                    Email = "agembarzewska@gmail.com",
                    UserName = "agembarzewska@gmail.com"
                };

                var result = await userManager.CreateAsync(user, "Qwerty123@");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user!");
                }
            }
            if (!ctx.Products.Any())
            {
                ///creating sample data
                var filePath = Path.Combine(hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                ctx.Products.AddRange(products);

                var order = ctx.Orders.Where(x => x.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };              
                }

                ctx.SaveChanges();
            }
        }
    }
}