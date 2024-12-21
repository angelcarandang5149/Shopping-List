using System;
using System.Collections.Generic;

namespace ShoppingList
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsPurchased { get; set; }

        public Item(int id, string name, int quantity)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            IsPurchased = false;
        }
    }

    public class ShoppingListManager
    {
        private static int _nextItemId = 1;
        private readonly List<Item> _items = new List<Item>();

        public void AddItem(string name, int quantity)
        {
            var newItem = new Item(_nextItemId++, name, quantity);
            _items.Add(newItem);
            Console.WriteLine($"Item '{newItem.Name}' (x{newItem.Quantity}) has been added to the shopping list.");
        }

        public IEnumerable<Item> GetItems()
        {
            return _items;
        }

        public Item GetItemById(int id)
        {
            return _items.Find(item => item.Id == id);
        }

        public void UpdateItem(int id, string name, int quantity)
        {
            var item = GetItemById(id);
            if (item != null)
            {
                item.Name = name;
                item.Quantity = quantity;
                Console.WriteLine($"Item ID {id} has been updated.");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        public void MarkItemPurchased(int id)
        {
            var item = GetItemById(id);
            if (item != null)
            {
                item.IsPurchased = true;
                Console.WriteLine($"Item ID {id} has been marked as purchased.");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        public void RemoveItem(int id)
        {
            int removedCount = _items.RemoveAll(item => item.Id == id);
            if (removedCount > 0)
            {
                Console.WriteLine($"Item ID {id} has been removed from the shopping list.");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }
    }

    class Program
    {
        static void main(string[] args)
        {
            var shoppingListManager = new ShoppingListManager();

            shoppingListManager.AddItem("Milk", 2);
            shoppingListManager.AddItem("Pan de Sal", 1);
            shoppingListManager.AddItem("Eggs", 12);

            Console.WriteLine("\nShopping List:");
            foreach (var item in shoppingListManager.GetItems())
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Purchased: {item.IsPurchased}");
            }

            shoppingListManager.MarkItemPurchased(1);

            shoppingListManager.UpdateItem(2, "Whole Wheat Bread", 2);

            shoppingListManager.RemoveItem(3);

            Console.WriteLine("\nUpdated Shopping List:");
            foreach (var item in shoppingListManager.GetItems())
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Purchased: {item.IsPurchased}");
            }

            Console.ReadKey();
        }
    }
}