namespace ShoppingCart.API.Models.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}