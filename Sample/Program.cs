using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ShoppingListApp
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPurchased { get; set; }
    }

    public interface IShoppingListService
    {
        void AddItem(Item item);
        IEnumerable<Item> GetAllItems();
        void MarkAsPurchased(int itemId);
    }

    public class ShoppingListManager : IShoppingListService
    {
        private readonly List<Item> _items = new List<Item>();

        public void AddItem(Item item)
        {
            item.Id = _items.Count + 1;
            item.IsPurchased = false;
            _items.Add(item);
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _items;
        }

        public void MarkAsPurchased(int itemId)
        {
            var item = _items.Find(i => i.Id == itemId);
            if (item != null)
                item.IsPurchased = true;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get()
        {
            return Ok(_shoppingListService.GetAllItems());
        }

        [HttpPost]
        public ActionResult AddItem([FromBody] Item item)
        {
            _shoppingListService.AddItem(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{itemId}")]
        public ActionResult MarkAsPurchased(int itemId)
        {
            _shoppingListService.MarkAsPurchased(itemId);
            return NoContent();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddTransient<IShoppingListService, ShoppingListManager>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}