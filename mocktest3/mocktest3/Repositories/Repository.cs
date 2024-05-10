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
        
        await using SqlConnection connection= new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<OrderDto> GetOrderInfo(int id)
    {
        var query = "SELECT o.OrderId AS Id, o.OrderDate AS Date, o.TotalAmount AS TotalPrice, " +
                    "p.Name AS Name, \noi.Quantity AS Amount, \np.Price AS Price " +
                    "FROM [Order] o\nJOIN OrderItem oi ON oi.OrderId = o.OrderId\nJOIN Product p ON p.ProductId = oi.ProductId\nWHERE o.OrderId = @Id";
        
        await using SqlConnection connection= new SqlConnection(_configuration.GetConnectionString("Default"));
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
        var amountOrdinal = reader.GetOrdinal("Amount");
        var priceOrdinal = reader.GetOrdinal("Price");
        
        OrderDto orderDto = null;

        while (await reader.ReadAsync())
        {
            if (orderDto is not null)
            {
                orderDto.Products.Add(new ProductDto()
                {
                    Name = reader.GetString(nameOrdinal),
                    Amount = reader.GetInt32(amountOrdinal),
                    Price = reader.GetDecimal(priceOrdinal)
                });
            }
            else
            {
                orderDto = new OrderDto()
                {
                    Id = reader.GetInt32(idOrdinal),
                    Date = reader.GetDateTime(dateOrdinal),
                    TotalPrice = reader.GetDecimal(totalPriceOrdinal),
                    Products = new List<ProductDto>()
                    {
                        new ProductDto()
                        {
                            Name = reader.GetString(nameOrdinal),
                            Amount = reader.GetInt32(amountOrdinal),
                            Price = reader.GetDecimal(priceOrdinal)
                        }
                    }
                };
            }
        }

        if (orderDto is null) throw new Exception("ERROR WHILE GETTING ORDER INFO");

        return orderDto;
    }
}