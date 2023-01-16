namespace AccounteeCQRS.Responses.Income;

public record IncomeUserResponse
{
    public required int Id { get; init; }
    public required int IdUser { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required decimal Amount { get; init; }
};