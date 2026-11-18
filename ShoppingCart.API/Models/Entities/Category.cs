namespace ShoppingCart.API.Models.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Icon { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}