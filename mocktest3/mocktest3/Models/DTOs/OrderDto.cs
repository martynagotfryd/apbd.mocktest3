namespace mocktest3.Models.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double TotalPrice { get; set; }
    public List<ProductDto> Products { get; set; }
}

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public int Amount { get; set; }
    public double Price { get; set; }
}