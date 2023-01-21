namespace AccounteeService.Contracts.Models;

public sealed class UserToIncomeModel
{
    public int Id { get; init; }
    public decimal? IncomePercent { get; init; }
}