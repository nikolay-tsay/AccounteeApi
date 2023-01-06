namespace AccounteeService.Contracts.Requests;

public class UserToIncomeRequest
{
    public int Id { get; set; }
    public decimal? IncomePercent { get; set; }
}