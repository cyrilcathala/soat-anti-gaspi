namespace Soat.AntiGaspi.Api.Contracts;

public class CreateAnnonceRequest
{
    public string? Title { get; init; }

    public string? Description { get; init; }

    public string? Email { get; init; }

    public string? CompanyName { get; init; }

    public string? Address { get; init; }

    public DateTime? Availability { get; init; }

    public DateTime? Expiration { get; init; }
}
