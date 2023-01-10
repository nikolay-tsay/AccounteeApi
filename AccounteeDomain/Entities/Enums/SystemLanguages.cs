using System.Runtime.Serialization;

namespace AccounteeDomain.Entities.Enums;

public enum SystemLanguages
{
    [EnumMember(Value = "en")]
    En = 1,
    [EnumMember(Value = "ru-ru")]
    Ru = 2,
}