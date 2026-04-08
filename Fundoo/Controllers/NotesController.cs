using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Interface;
using ModelLayer.DTOs;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL _noteBL;

        public NotesController(INoteBL noteBL)
        {
            _noteBL = noteBL;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("UserId").Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
        {
            var userId = GetUserId();
            var id = await _noteBL.CreateNote(dto, userId);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var notes = await _noteBL.GetAllNotes(userId);
            return Ok(notes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, UpdateNoteDto dto)
        {
            var userId = GetUserId();
            var result = await _noteBL.UpdateNote(id, dto, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Trash(int id)
        {
            var userId = GetUserId();
            var result = await _noteBL.MoveToTrash(id, userId);
            return Ok(result);
        }

        [HttpGet("trash")]
        public async Task<IActionResult> GetTrash()
        {
            var userId = GetUserId();
            return Ok(await _noteBL.GetTrashedNotes(userId));
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.RestoreNote(id, userId));
        }

        [HttpDelete("{id}/permanent")]
        public async Task<IActionResult> DeletePermanent(int id)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.PermanentDelete(id, userId));
        }

        [HttpGet("archive")]
        public async Task<IActionResult> GetArchive()
        {
            var userId = GetUserId();
            return Ok(await _noteBL.GetArchivedNotes(userId));
        }

        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> Archive(int id)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.ArchiveNote(id, userId));
        }

        [HttpPatch("{id}/unarchive")]
        public async Task<IActionResult> Unarchive(int id)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.UnarchiveNote(id, userId));
        }
        
        [HttpPatch("{id}/pin")]
        public async Task<IActionResult> Pin(int id)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.PinNote(id, userId));
        }

        [HttpPatch("{id}/unpin")]
        public async Task<IActionResult> Unpin(int id)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.UnpinNote(id, userId));
        }
        
        [HttpPatch("{id}/color")]
        public async Task<IActionResult> ChangeColor(int id, [FromQuery] string color)
        {
            var userId = GetUserId();
            return Ok(await _noteBL.ChangeColor(id, color, userId));
        }
    }
}
