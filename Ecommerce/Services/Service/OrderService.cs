using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Ecommerce.Services.Interface;

namespace Ecommerce.Services.Service
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(string userId, Orderdto dto)
        {
            var user = await _orderRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var cart = await _orderRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found");

            var order = new Order
            {
                UserId = userId,
                Address = dto.Address,
                Pincode = dto.Pincode,
                TotalPrice = dto.TotalPrice,
                PaymentMethod = dto.PaymentMethod,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = dto.Orderitems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            await _orderRepository.CreateOrderAsync(order);

            var productIds = dto.Orderitems.Select(o => o.ProductId).ToList();
            var products = await _orderRepository.GetProductsByIdsAsync(productIds);

            foreach (var item in order.OrderItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null) product.stock -= item.Quantity;
            }

            var cartItems = await _orderRepository.GetCartItemsAsync(cart.Id);
            var cartItemsToRemove = cartItems.Where(c => productIds.Contains(c.ProductId)).ToList();

            if (cartItemsToRemove.Any()) await _orderRepository.RemoveCartItemsAsync(cartItemsToRemove);

            await _orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task<object> GetUserOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            if (orders == null || orders.Count == 0) return null;

            return orders.Select(o => new
            {
                o.OrderDate,
                o.PaymentMethod,
                o.Status,
                o.Address,
                OrderItems = o.OrderItems.Select(oi => new
                {
                    ProductName = oi.Product?.Name ?? "Unknown",
                    oi.Quantity,
                    oi.Price
                }).ToList()
            }).ToList();
        }
    }
}
