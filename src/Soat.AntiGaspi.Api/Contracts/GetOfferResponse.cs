namespace Soat.AntiGaspi.Api.Contracts;

public class GetOfferResponse
{
    public Guid Id { get; init; }

    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;

    public string CompanyName { get; init; } = default!;

    public DateOnly? Availability { get; init; }

    public DateOnly? Expiration { get; init; }
}
