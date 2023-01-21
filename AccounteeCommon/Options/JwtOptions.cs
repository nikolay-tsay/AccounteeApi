﻿namespace AccounteeCommon.Options;

public sealed class JwtOptions
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int TokenValidMinutes { get; set; }
}