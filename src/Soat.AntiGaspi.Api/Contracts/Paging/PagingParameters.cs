namespace Soat.AntiGaspi.Api.Contracts.Paging;

public class PagingParameters
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public SortOrder? SortOrder { get; init; }
}