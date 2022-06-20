using OrderManagement.Domain.Entitities;

namespace OrderManagement.Domain.Contracts
{
    public interface IOrdersService
    {
        Task<Order> RegisterOrder(string createdBy, string product, string orderNo, decimal price, decimal total, DateTime orderDate);
    }
}
