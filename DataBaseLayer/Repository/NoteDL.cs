using Dapper;
using System.Data;
using ModelLayer.DTOs;
using DataBaseLayer.Interfaces;
using ModelLayer;

namespace DataBaseLayer.Repository
{
    public class NoteDL : INoteDL
    {
        private readonly IDbConnection _db;

        public NoteDL(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateNote(CreateNoteDto dto, int userId)
        {
            var query = @"
            INSERT INTO Notes (Title, Description, Reminder, Colour, Image, UserId)
            VALUES (@Title, @Description, @Reminder, @Colour, @Image, @UserId);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            // ExecuteScalarAsync<int> runs the insert and reads back the generated identity value.
            return await _db.ExecuteScalarAsync<int>(query, new
            {
                dto.Title,
                dto.Description,
                dto.Reminder,
                dto.Colour,
                dto.Image,
                UserId = userId
            });
        }

        public async Task<IEnumerable<Note>> GetAllNotes(int userId)
        {
            var query = "SELECT * FROM Notes WHERE UserId = @UserId AND IsTrash = 0";
            // QueryAsync<Note> maps each returned row directly into the Note model.
            return await _db.QueryAsync<Note>(query, new { UserId = userId });
        }

        public async Task<Note> GetNoteById(int noteId, int userId)
        {
            var query = "SELECT * FROM Notes WHERE NotesId = @NoteId AND UserId = @UserId";
            return await _db.QueryFirstOrDefaultAsync<Note>(query, new { NoteId = noteId, UserId = userId });
        }

        public async Task<bool> UpdateNote(int noteId, UpdateNoteDto dto, int userId)
        {
            var query = @"
            UPDATE Notes
            SET Title=@Title,
                Description=@Description,
                Reminder=@Reminder,
                Colour=@Colour,
                UpdatedAt=GETUTCDATE()
            WHERE NotesId=@NoteId AND UserId=@UserId";

            var result = await _db.ExecuteAsync(query, new
            {
                dto.Title,
                dto.Description,
                dto.Reminder,
                dto.Colour,
                NoteId = noteId,
                UserId = userId
            });

            return result > 0;
        }

        public async Task<bool> MoveToTrash(int noteId, int userId)
        {
            var query = "UPDATE Notes SET IsTrash=1 WHERE NotesId=@NoteId AND UserId=@UserId";

            var result = await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId });

            return result > 0;
        }

        public async Task<IEnumerable<Note>> GetTrashedNotes(int userId)
        {
            var query = "SELECT * FROM Notes WHERE UserId=@UserId AND IsTrash=1";
            return await _db.QueryAsync<Note>(query, new { UserId = userId });
        }

        public async Task<bool> RestoreNote(int noteId, int userId)
        {
            var query = "UPDATE Notes SET IsTrash=0 WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId }) > 0;
        }

        public async Task<bool> PermanentDelete(int noteId, int userId)
        {
            var query = "DELETE FROM Notes WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId }) > 0;
        }

        public async Task<IEnumerable<Note>> GetArchivedNotes(int userId)
        {
            var query = "SELECT * FROM Notes WHERE UserId=@UserId AND IsArchive=1 AND IsTrash=0";
            return await _db.QueryAsync<Note>(query, new { UserId = userId });
        }

        public async Task<bool> ArchiveNote(int noteId, int userId)
        {
            var query = "UPDATE Notes SET IsArchive=1 WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId }) > 0;
        }

        public async Task<bool> UnarchiveNote(int noteId, int userId)
        {
            var query = "UPDATE Notes SET IsArchive=0 WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId }) > 0;
        }

        public async Task<bool> PinNote(int noteId, int userId)
        {
            var query = "UPDATE Notes SET IsPin=1 WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId }) > 0;
        }

        public async Task<bool> UnpinNote(int noteId, int userId)
        {
            var query = "UPDATE Notes SET IsPin=0 WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { NoteId = noteId, UserId = userId }) > 0;
        }

        public async Task<bool> ChangeColor(int noteId, string color, int userId)
        {
            var query = "UPDATE Notes SET Colour=@Colour WHERE NotesId=@NoteId AND UserId=@UserId";
            return await _db.ExecuteAsync(query, new { Colour = color, NoteId = noteId, UserId = userId }) > 0;
        }
    }
}
