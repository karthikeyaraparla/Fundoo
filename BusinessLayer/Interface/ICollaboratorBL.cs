using ModelLayer;

public interface ICollaboratorBL
{
    Task<int> AddCollaborator(string email, int userId, int noteId);

    Task<IEnumerable<Collaborator>> GetCollaborators(int noteId);

    Task<bool> RemoveCollaborator(int collaboratorId, int noteId, int userId);
}
