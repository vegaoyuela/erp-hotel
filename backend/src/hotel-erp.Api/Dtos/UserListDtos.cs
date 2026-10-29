namespace hotel_erp.Api.Dtos.Common;

public sealed class UserPageQuery : SearchPageQuery
{
    public Guid? RoleId { get; set; }
}

public sealed class RoleOptionPageQuery : SearchPageQuery
{
    public bool AssignableOnly { get; set; }
}

public sealed record RoleOptionDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
