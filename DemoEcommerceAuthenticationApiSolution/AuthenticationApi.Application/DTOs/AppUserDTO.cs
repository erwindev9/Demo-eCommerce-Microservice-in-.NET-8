using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record AppUserDTO(
        int Id,
        [Required] string? Name,
        [Required] string? TelephoneNumber,
        [Required] string? Email,
        [Required] string? Address,
        [Required] string? Password, 
        [Required] string? Role
        
    );
        
    
}
