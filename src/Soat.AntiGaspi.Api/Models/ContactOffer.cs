namespace Soat.AntiGaspi.Api.Models;

public class ContactOffer
{
    public int Id { get; set; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public string? Email { get; init; }

    public string? Message { get; init; }

    public Guid OfferId { get; set; }

    public DateTimeOffset CreationDate { get; init; } = DateTimeOffset.Now;
}
