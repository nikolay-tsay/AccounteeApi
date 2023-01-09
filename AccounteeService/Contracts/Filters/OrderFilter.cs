namespace AccounteeService.Contracts.Filters;

public class OrderFilter
{
    public string? PropertyName { get; set; } 
    public bool? IsDescending { get; set; }
}