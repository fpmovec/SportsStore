using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepository;
        private Cart _cart;
        public OrderController(IOrderRepository orderRepository, Cart cart)
        {
            _orderRepository = orderRepository;
            _cart = cart;
        }

        public ViewResult Checkout()
            => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray(); 
                _orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }

        public ViewResult List()
            => View(_orderRepository.Orders.Where(o => !o.IsShipped));

        [HttpPost]
        public IActionResult MarkShipped(int OrderID)
        {
            Order? order = _orderRepository.Orders.FirstOrDefault(o => o.OrderID == OrderID);
            if (order != null)
            {
                order.IsShipped= true;
                _orderRepository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }
    }
}
