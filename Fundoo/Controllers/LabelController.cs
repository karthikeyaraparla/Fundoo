using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ModelLayer.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly ILabelBL _labelBL;

    public LabelsController(ILabelBL labelBL)
    {
        _labelBL = labelBL;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst("UserId").Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LabelDto dto)
    {
        var userId = GetUserId();
        return Ok(await _labelBL.CreateLabel(dto.LabelName, userId));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        return Ok(await _labelBL.GetLabels(userId));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LabelDto dto)
    {
        var userId = GetUserId();
        return Ok(await _labelBL.UpdateLabel(id, dto.LabelName, userId));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        return Ok(await _labelBL.DeleteLabel(id, userId));
    }

    [HttpPost("{labelId}/notes/{noteId}")]
    public async Task<IActionResult> AddLabelToNote(int labelId, int noteId)
    {
        return Ok(await _labelBL.AddLabelToNote(labelId, noteId));
    }

    [HttpDelete("{labelId}/notes/{noteId}")]
    public async Task<IActionResult> RemoveLabelFromNote(int labelId, int noteId)
    {
        return Ok(await _labelBL.RemoveLabelFromNote(labelId, noteId));
    }

    [HttpGet("{labelId}/notes")]
    public async Task<IActionResult> GetNotes(int labelId)
    {
        var userId = GetUserId();
        return Ok(await _labelBL.GetNotesByLabel(labelId, userId));
    }
}
