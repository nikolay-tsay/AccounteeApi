namespace AccounteeService.Contracts.Requests;

public class CreateIncomeRequest
{
    public int IdCategory { get; set; }
    public int? IdService { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime LastEdited { get; set; }
    public decimal TotalAmount { get; set; }

    public IEnumerable<ProductToIncomeRequest>? ProductToIncomeRequests { get; set; }
    public IEnumerable<UserToIncomeRequest>? UserToIncomeRequests { get; set; }
}