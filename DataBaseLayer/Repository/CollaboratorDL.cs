using Dapper;
using System.Data;
using DataBaseLayer.Interfaces;
using ModelLayer;

public class CollaboratorDL : ICollaboratorDL
{
    private readonly IDbConnection _db;

    public CollaboratorDL(IDbConnection db)
    {
        _db = db;
    }

    public async Task<int> AddCollaborator(string email, int userId, int noteId)
    {
        var query = @"
        INSERT INTO Collaborators (Email, OwnerUserId, NoteId)
        VALUES (@Email, @UserId, @NoteId);
        SELECT CAST(SCOPE_IDENTITY() as int);";

        return await _db.ExecuteScalarAsync<int>(query, new
        {
            Email = email,
            UserId = userId,
            NoteId = noteId
        });
    }

    public async Task<IEnumerable<Collaborator>> GetCollaborators(int noteId)
    {
        var query = "SELECT * FROM Collaborators WHERE NoteId=@NoteId";
        return await _db.QueryAsync<Collaborator>(query, new { NoteId = noteId });
    }

    public async Task<bool> RemoveCollaborator(int collaboratorId, int noteId, int userId)
    {
        var query = @"
        DELETE FROM Collaborators 
        WHERE CollaboratorId=@Id AND NoteId=@NoteId AND UserId=@UserId";

        return await _db.ExecuteAsync(query, new
        {
            Id = collaboratorId,
            NoteId = noteId,
            UserId = userId
        }) > 0;
    }
}
