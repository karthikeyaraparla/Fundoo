using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ModelLayer.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly ICollaboratorBL _collabBL;

    public CollaboratorsController(ICollaboratorBL collabBL)
    {
        _collabBL = collabBL;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst("UserId").Value);
    }

    [HttpPost("{noteId}")]
    public async Task<IActionResult> Add(int noteId, [FromBody] CollaboratorDto dto)
    {
        var userId = GetUserId();
        var id = await _collabBL.AddCollaborator(dto.Email, userId, noteId);
        return Ok(id);
    }

    [HttpGet("{noteId}")]
    public async Task<IActionResult> Get(int noteId)
    {
        return Ok(await _collabBL.GetCollaborators(noteId));
    }

    [HttpDelete("{noteId}/{collaboratorId}")]
    public async Task<IActionResult> Remove(int noteId, int collaboratorId)
    {
        var userId = GetUserId();
        return Ok(await _collabBL.RemoveCollaborator(collaboratorId, noteId, userId));
    }
}
