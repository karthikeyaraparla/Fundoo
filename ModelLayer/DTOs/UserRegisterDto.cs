using System.ComponentModel.DataAnnotations;
namespace ModelLayer.DTOs

{
        public class UserRegisterDto
        {
            
            [MinLength(2, ErrorMessage = "Name should be atleast 2 chars")]
            [MaxLength(100, ErrorMessage = "Name should not exceed 100 chars")]
            [Required(ErrorMessage = "Name is Required")]
            public string FirstName { get; set; } = string.Empty;
            
            [MaxLength(100, ErrorMessage = "Name should not exceed 100 chars")]
            [Required(ErrorMessage = "Name is Required")]
            public string LastName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email should not be empty")]
            [EmailAddress(ErrorMessage = "Invalid Email Format")]
            public string Email { get; set; } = string.Empty;

            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$")]
            public string Password { get; set; } = string.Empty;
        }
    }
