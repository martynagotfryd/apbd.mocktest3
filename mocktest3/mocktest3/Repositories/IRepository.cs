using mocktest3.Models.DTOs;

namespace mocktest3.Repositories;

public interface IRepository
{
    Task<bool> DoesOrderExist(int id);
    Task<OrderDto> GetOrderInfo(int id);
}