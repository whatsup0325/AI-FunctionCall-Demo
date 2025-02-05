using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Test
{
    public class OrderPizzaPlugin
    {
        public Cart _Cart = new Cart();
        public Menu _Menu = new Menu();

        [KernelFunction("get_pizza_menu")]
        public Menu GetPizzaMenuAsync()
        {
            return _Menu;
        }

        [KernelFunction("add_pizza_to_cart")]
        [Description("Add a pizza to the user's cart; returns the new item and updated cart")]
        public void AddPizzaToCart(
           int pizzaId
        )
        {
            var pizza = _Menu.pizzas.FirstOrDefault(x => x.Id == pizzaId);

            if (pizza == null)
                return;

            _Cart.pizzas.Add(pizza);
        }

        [KernelFunction("remove_pizza_from_cart")]
        public void RemovePizzaFromCart(int pizzaId)
        {
            var pizza = _Cart.pizzas.FirstOrDefault(x => x.Id == pizzaId);

            if (pizza == null)
                return;

            _Cart.pizzas.Remove(pizza);
        }

        [KernelFunction("get_pizza_from_cart")]
        [Description("Returns the specific details of a pizza in the user's cart; use this instead of relying on previous messages since the cart may have changed since then.")]
        public Pizza? GetPizzaFromCart(int pizzaId)
        {
            return _Cart.pizzas.FirstOrDefault(x => x.Id == pizzaId);
        }

        [KernelFunction("get_cart")]
        [Description("Returns the user's current cart, including the total price and items in the cart.")]
        public Cart GetCart()
        {
            return _Cart;
        }

        [KernelFunction("checkout")]
        [Description("Check out the user's cart and return the total price.")]
        public int Checkout()
        {
            return _Cart.pizzas.Sum(x => x.Prcie);
        }
    }

    public class Cart
    {
        public List<Pizza> pizzas { get; set; } = new List<Pizza>();

    }

    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Prcie { get; set; }

    }


    public class Menu
    {
        public List<Pizza> pizzas { get; set; } = new List<Pizza>();

        public Menu()
        {
            pizzas.Add(new Pizza { Id = 1, Name = "瑪格麗特", Description = "Pepperoni, cheese, and tomato sauce", Prcie = 200 });
            pizzas.Add(new Pizza { Id = 2, Name = "起司", Description = "Cheese and tomato sauce", Prcie = 220 });
            pizzas.Add(new Pizza { Id = 3, Name = "夏威夷", Description = "Mushrooms, peppers, onions, and tomato sauce", Prcie = 250 });
            pizzas.Add(new Pizza { Id = 4, Name = "墨西哥", Description = "Pepperoni, jalapenos, and tomato sauce", Prcie = 250 });
            pizzas.Add(new Pizza { Id = 5, Name = "綜合", Description = "Pepperoni, sausage, mushrooms, and tomato sauce", Prcie = 320 });
        }
    }
}
