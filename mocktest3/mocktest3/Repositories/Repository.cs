using Microsoft.Data.SqlClient;
using mocktest3.Models.DTOs;

namespace mocktest3.Repositories;

public class Repository : IRepository
{
    private readonly IConfiguration _configuration;

    public Repository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<bool> DoesOrderExist(int id)
    {
        var query = "SELECT 1 FROM [Order] o WHERE o.OrderId = @Id";
        
        await using SqlConnection connection= new SqlConnection(_configuration.GetConnectionString("Defaul"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        var res = command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<OrderDto> GetOrderInfo(int id)
    {
        var query = "SELECT o.OrderId AS Id, o.OrderDate AS Date, o.TotalAmount AS TotalPrice, " +
                    "p.Name AS Name, \noi.Quantity AS Amount, \np.Price AS Price " +
                    "FROM [Order] o\nJOIN OrderItem oi ON oi.OrderId = o.OrderId\nJOIN Product p ON p.ProductId = oi.ProductId\nWHERE o.OrderId = @Id";
        
        await using SqlConnection connection= new SqlConnection(_configuration.GetConnectionString("Defaul"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@Id", id);
        
        await connection.OpenAsync();
        
        var reader = await command.ExecuteReaderAsync();

        var idOrdinal = reader.GetOrdinal("Id");
        var dateOrdinal = reader.GetOrdinal("Date");
        var totalPriceOrdinal = reader.GetOrdinal("TotalPrice");
        var nameOrdinal = reader.GetOrdinal("Name");
        
        
        OrderDto orderDto = null;

        while (await reader.ReadAsync())
        {
            if (orderDto is not null)
            {
                orderDto.Products.Add(new ProductDto()
                {
                    Name = 
                });
            }
            else
            {
                orderDto = new OrderDto()
                {
                    
                };
            }
        }

        if (orderDto is null) throw new Exception("ERROR WHILE GETTING ORDER INFO");

        return orderDto;
    }
}