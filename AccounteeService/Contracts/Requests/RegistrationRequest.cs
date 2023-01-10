﻿using System.Text.Json.Serialization;
using AccounteeDomain.Entities.Enums;

namespace AccounteeService.Contracts.Requests;

public class RegistrationRequest
{
    public int IdRole { get; set; }
    public string Login { get; set; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserLanguages Language { get; set; }
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public decimal? IncomePercent { get; set; }
}