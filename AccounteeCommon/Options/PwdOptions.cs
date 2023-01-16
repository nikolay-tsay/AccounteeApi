namespace AccounteeCommon.Options;

public sealed class PwdOptions
{
    public int HashSize { get; set; }
    public int SaltSize { get; set; }
    public int Iterations { get; set; }
}