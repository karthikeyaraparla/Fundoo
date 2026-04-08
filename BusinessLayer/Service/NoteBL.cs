using BusinessLayer.Interface;
using DataBaseLayer.Interfaces;
using ModelLayer;
using ModelLayer.DTOs;

namespace BusinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteDL _noteDL;

        public NoteBL(INoteDL noteDL)
        {
            _noteDL = noteDL;
        }

        public async Task<int> CreateNote(CreateNoteDto dto, int userId)
        {
            return await _noteDL.CreateNote(dto, userId);
        }

        public async Task<IEnumerable<Note>> GetAllNotes(int userId)
        {
            return await _noteDL.GetAllNotes(userId);
        }

        public async Task<Note> GetNoteById(int noteId, int userId)
        {
            return await _noteDL.GetNoteById(noteId, userId);
        }

        public async Task<bool> UpdateNote(int noteId, UpdateNoteDto dto, int userId)
        {
            return await _noteDL.UpdateNote(noteId, dto, userId);
        }

        public async Task<bool> MoveToTrash(int noteId, int userId)
        {
            return await _noteDL.MoveToTrash(noteId, userId);
        }
        
        public async Task<IEnumerable<Note>> GetTrashedNotes(int userId)
            => await _noteDL.GetTrashedNotes(userId);

        public async Task<bool> RestoreNote(int noteId, int userId)
            => await _noteDL.RestoreNote(noteId, userId);

        public async Task<bool> PermanentDelete(int noteId, int userId)
            => await _noteDL.PermanentDelete(noteId, userId);

        public async Task<IEnumerable<Note>> GetArchivedNotes(int userId)
            => await _noteDL.GetArchivedNotes(userId);

        public async Task<bool> ArchiveNote(int noteId, int userId)
            => await _noteDL.ArchiveNote(noteId, userId);

        public async Task<bool> UnarchiveNote(int noteId, int userId)
            => await _noteDL.UnarchiveNote(noteId, userId);

        public async Task<bool> PinNote(int noteId, int userId)
            => await _noteDL.PinNote(noteId, userId);

        public async Task<bool> UnpinNote(int noteId, int userId)
            => await _noteDL.UnpinNote(noteId, userId);

        public async Task<bool> ChangeColor(int noteId, string color, int userId)
            => await _noteDL.ChangeColor(noteId, color, userId);
    }
}
