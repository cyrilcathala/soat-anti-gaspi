namespace Soat.AntiGaspi.Api.Contracts;

public class CreateOfferRequest
{
    public string? Title { get; init; }

    public string? Description { get; init; }

    public string? Email { get; init; }

    public string? CompanyName { get; init; }

    public string? Address { get; init; }

    public DateOnly? Availability { get; init; }

    public DateOnly? Expiration { get; init; }
}
