namespace eCommerce.Domain.DomainObjects
{
    public class CreateOrderDomain
    {
        public int UserId { get; set; }
        public List<CreateOrderItemDomain> Items { get; set; } = new();
    }

    public class CreateOrderItemDomain
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }


    public sealed record OrderCreationResult(int OrderId, string OrderNumber);
}
