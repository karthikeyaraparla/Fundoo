using ModelLayer;
using ModelLayer;

namespace DataBaseLayer.Interfaces
{
    public interface ICollaboratorDL
    {
        Task<int> AddCollaborator(string email, int userId, int noteId);

        Task<IEnumerable<Collaborator>> GetCollaborators(int noteId);

        Task<bool> RemoveCollaborator(int collaboratorId, int noteId, int userId);
    }
}
