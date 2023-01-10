using System.Globalization;
using AccounteeDomain.Entities;

namespace AccounteeService.Contracts.Models;

public record CurrentUser(UserEntity User, CultureInfo Culture);

