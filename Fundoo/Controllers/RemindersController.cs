using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    private readonly IReminderBL _reminderBL;

    public RemindersController(IReminderBL reminderBL)
    {
        _reminderBL = reminderBL;
    }

    [HttpPost("{noteId}")]
    public async Task<IActionResult> Create(int noteId, [FromBody]CreateReminderDto dto)
    {
        return Ok(await _reminderBL.CreateReminder(dto.DateTime, noteId));
    }

    [HttpGet("{noteId}")]
    public async Task<IActionResult> Get(int noteId)
    {
        return Ok(await _reminderBL.GetReminders(noteId));
    }

    [HttpPut("{reminderId}")]
    public async Task<IActionResult> Update(int reminderId, [FromBody]UpdateReminderDto dto)
    {
        return Ok(await _reminderBL.UpdateReminder(reminderId, dto.DateTime, dto.Status));
    }

    [HttpDelete("{reminderId}")]
    public async Task<IActionResult> Delete(int reminderId)
    {
        return Ok(await _reminderBL.DeleteReminder(reminderId));
    }
}
