namespace Soat.AntiGaspi.Api.Contracts.Paging;

public class PaginedCollection<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();

    public int Total { get; init; }
}
