namespace AccounteeCommon.Options;

public class JwtOptions
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int TokenValidMinutes { get; set; }
    public int ForgottenPasswordValidMinutes { get; set; }
    public int MaxAttemptCount { get; set; }
}