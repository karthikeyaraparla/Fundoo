using ModelLayer;
using ModelLayer.DTOs;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        Task<int> CreateNote(CreateNoteDto dto, int userId);
        Task<IEnumerable<Note>> GetAllNotes(int userId);
        Task<Note> GetNoteById(int noteId, int userId);
        Task<bool> UpdateNote(int noteId, UpdateNoteDto dto, int userId);
        Task<bool> MoveToTrash(int noteId, int userId);
        Task<IEnumerable<Note>> GetTrashedNotes(int userId);
        Task<bool> RestoreNote(int noteId, int userId);
        Task<bool> PermanentDelete(int noteId, int userId);
        Task<IEnumerable<Note>> GetArchivedNotes(int userId);
        Task<bool> ArchiveNote(int noteId, int userId);
        Task<bool> UnarchiveNote(int noteId, int userId);
        Task<bool> PinNote(int noteId, int userId);
        Task<bool> UnpinNote(int noteId, int userId);
        Task<bool> ChangeColor(int noteId, string color, int userId);
    }
}
