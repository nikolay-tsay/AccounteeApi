using AccounteeCommon.HttpContexts;
using AccounteeDomain.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccounteeDomain.Extensions;

public static class ContextExtension
{
    public static PropertyBuilder<DateTime> UtcDate(this PropertyBuilder<DateTime> property) =>
        property.HasConversion(x => x, x => DateTime.SpecifyKind(x, DateTimeKind.Utc));

    public static PropertyBuilder<DateTime?> UtcDate(this PropertyBuilder<DateTime?> property) =>
        property.HasConversion(x => x, x => x == null ? null : DateTime.SpecifyKind(x.Value, DateTimeKind.Utc));

    public static EntityTypeBuilder<T> CompanyFilter<T>(this EntityTypeBuilder<T> property) where T : class, IBaseWithCompany =>
        property.HasQueryFilter(x => GlobalHttpContext.GetIgnoreCompanyFilter() || x.IdCompany == GlobalHttpContext.GetCompanyId());
}