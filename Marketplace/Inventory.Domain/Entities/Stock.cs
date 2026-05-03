namespace Inventory.Domain.Entities;

public class Stock
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private Stock() { }

    public static Stock Create(Guid productId)
    {
        return new Stock
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = 1
        };
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}