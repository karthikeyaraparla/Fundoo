using DataBaseLayer.Interfaces;
using ModelLayer;

public class CollaboratorBL : ICollaboratorBL
{
    private readonly ICollaboratorDL _collabDL;

    public CollaboratorBL(ICollaboratorDL collabDL)
    {
        _collabDL = collabDL;
    }

    public Task<int> AddCollaborator(string email, int userId, int noteId)
        => _collabDL.AddCollaborator(email, userId, noteId);

    public Task<IEnumerable<Collaborator>> GetCollaborators(int noteId)
        => _collabDL.GetCollaborators(noteId);

    public Task<bool> RemoveCollaborator(int collaboratorId, int noteId, int userId)
        => _collabDL.RemoveCollaborator(collaboratorId, noteId, userId);
}
