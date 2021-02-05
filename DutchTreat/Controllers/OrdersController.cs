using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<ProductsController> logger;
        private readonly IMapper mapper;

        public OrdersController(IDutchRepository repository, ILogger<ProductsController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> Get(bool includeItems = true)
        {
            try
            {
                var results = repository.GetAllOrders(includeItems);

                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(repository.GetAllOrders()) );
            }
            catch (System.Exception ex)
            {
                logger.LogError($"Failed to get products: {ex}");
                return BadRequest($"Failed to get products");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Get(int id)
        {
            try
            {
                var order = repository.GetOrderById(id);

                return order != null ? Ok(mapper.Map<Order,OrderViewModel>(order)) : (IActionResult)NotFound();
            }
            catch (System.Exception ex)
            {
                logger.LogError($"Failed to get product: {ex}");
                return BadRequest($"Failed to get product");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = mapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    repository.AddEntity(model);

                    if (repository.SaveChanges())
                    {
                        var vm = mapper.Map<Order, OrderViewModel>(newOrder);

                        return Created($"/api/orders{ vm.OrderId}", model);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError($"Failed to save a new order: {ex}");
                throw;
            }

            return BadRequest("Failed to save new order"); ;
        }
    }
}