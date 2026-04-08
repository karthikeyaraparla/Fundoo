using System.ComponentModel.DataAnnotations;

public class CreateReminderDto
{
    [Required]
    public DateTime DateTime { get; set; }
}

public class UpdateReminderDto
{
    public DateTime DateTime { get; set; }
    public string Status { get; set; }
}
