using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PricingRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? StoreId { get; set; }
    public string? SKU { get; set; }
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public DateTime? PricingDate { get; set; }
    public DateTime? CreatedDate { get; set; }
}