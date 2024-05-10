using mocktest3.Models.DTOs;

namespace mocktest3.Repositories;

public interface IRepository
{
    Task<bool> DoesOrderExist(int id);
    Task<OrderDto> GetOrderInfo(int id);
    Task<bool> DoesProductExist(int orderId);
    Task UpdateAmount(int orderId, int productId, int amount);
}