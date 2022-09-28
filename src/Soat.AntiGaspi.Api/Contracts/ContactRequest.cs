namespace Soat.AntiGaspi.Api.Contracts;

public class ContactRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Message { get; init; }
}
