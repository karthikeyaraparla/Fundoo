using System;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs
{
    public class CreateNoteDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 255 characters")]
        public string Title { get; set; }

        [MaxLength(5000, ErrorMessage = "Description too long")]
        public string Description { get; set; }

        public DateTime? Reminder { get; set; }

        [RegularExpression("^#([A-Fa-f0-9]{6})$", ErrorMessage = "Invalid color format. Use hex like #FFFFFF")]
        public string Colour { get; set; }

        [Url(ErrorMessage = "Invalid image URL")]
        public string Image { get; set; }
    }

    public class UpdateNoteDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 2)]
        public string Title { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        public DateTime? Reminder { get; set; }

        [RegularExpression("^#([A-Fa-f0-9]{6})$")]
        public string Colour { get; set; }
    }
}
