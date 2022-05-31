namespace Soat.AntiGaspi.Api.Contracts;

public class GetAnnonceResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string CompanyName { get; set; } = default!;

    public DateTime? Availability { get; set; }

    public DateTime? Expiration { get; set; }
}
