using Dapper;
using System.Data;
using Microsoft.Data.SqlClient.DataClassification;
using ModelLayer;

public class LabelDL : ILabelDL
{
    private readonly IDbConnection _db;

    public LabelDL(IDbConnection db)
    {
        _db = db;
    }

    public async Task<int> CreateLabel(string labelName, int userId)
    {
        var query = @"
        INSERT INTO Labels (LabelName, CreatedAt, UpdatedAt, UserId)
        VALUES (@LabelName, GETUTCDATE(), GETUTCDATE(), @UserId);
        SELECT CAST(SCOPE_IDENTITY() as int);";

        return await _db.ExecuteScalarAsync<int>(query, new { LabelName = labelName, UserId = userId });
    }

    public async Task<IEnumerable<Label>> GetLabels(int userId)
    {
        var query = "SELECT * FROM Labels WHERE UserId=@UserId";
        return await _db.QueryAsync<Label>(query, new { UserId = userId });
    }

    public async Task<bool> UpdateLabel(int labelId, string labelName, int userId)
    {
        var query = @"
        UPDATE Labels 
        SET LabelName=@LabelName, UpdatedAt=GETUTCDATE()
        WHERE LabelId=@LabelId AND UserId=@UserId";

        return await _db.ExecuteAsync(query, new { LabelId = labelId, LabelName = labelName, UserId = userId }) > 0;
    }

    public async Task<bool> DeleteLabel(int labelId, int userId)
    {
        var query = "DELETE FROM Labels WHERE LabelId=@LabelId AND UserId=@UserId";
        return await _db.ExecuteAsync(query, new { LabelId = labelId, UserId = userId }) > 0;
    }

    public async Task<bool> AddLabelToNote(int labelId, int noteId)
    {
        var query = "INSERT INTO NoteLabels (NoteId, LabelId) VALUES (@NoteId, @LabelId)";
        return await _db.ExecuteAsync(query, new { NoteId = noteId, LabelId = labelId }) > 0;
    }

    public async Task<bool> RemoveLabelFromNote(int labelId, int noteId)
    {
        var query = "DELETE FROM NoteLabels WHERE NoteId=@NoteId AND LabelId=@LabelId";
        return await _db.ExecuteAsync(query, new { NoteId = noteId, LabelId = labelId }) > 0;
    }

    public async Task<IEnumerable<Note>> GetNotesByLabel(int labelId, int userId)
    {
        var query = @"
        SELECT N.* FROM Notes N
        INNER JOIN NoteLabels NL ON N.NotesId = NL.NoteId
        WHERE NL.LabelId=@LabelId AND N.UserId=@UserId";

        return await _db.QueryAsync<Note>(query, new { LabelId = labelId, UserId = userId });
    }
}
