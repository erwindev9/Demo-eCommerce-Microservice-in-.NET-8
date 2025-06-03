using System.ComponentModel.DataAnnotations;
namespace AuthenticationApi.Application.DTOs
{
    public record GetUserDTO
    (
        int Id,
        [Required] string? Name,
        [Required] string? TelephoneNumber,
        [Required, EmailAddress] string? Email,
        [Required] string? Address,
        [Required] string? Role

    );
}
