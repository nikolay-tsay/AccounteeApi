namespace AccounteeCQRS.Responses.Income;

public class IncomeProductResponse
{
    public required int Id { get; init; }
    public required int IdProduct { get; init; }
    public required string ProductName { get; init; }
    public required decimal Amount { get; init; }
}