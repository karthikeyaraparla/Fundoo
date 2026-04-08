using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs;

public class CollaboratorDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
