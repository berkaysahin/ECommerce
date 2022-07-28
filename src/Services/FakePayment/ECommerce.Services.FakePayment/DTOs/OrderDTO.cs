namespace ECommerce.Services.FakePayment.DTOs;

public class OrderDTO
{
    public OrderDTO()
    {
        OrderItems = new List<OrderItemDTO>();
    }

    public string BuyerId { get; set; }

    public List<OrderItemDTO> OrderItems { get; set; }

    public AddressDTO Address { get; set; }
}

public class OrderItemDTO
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string PictureUrl { get; set; }
    public Decimal Price { get; set; }
}

public class AddressDTO
{
    public string Province { get; set; }

    public string District { get; set; }

    public string Street { get; set; }

    public string ZipCode { get; set; }

    public string Line { get; set; }
}
