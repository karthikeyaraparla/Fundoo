using System.Reflection.Emit;
using ModelLayer;
using Label = Microsoft.Data.SqlClient.DataClassification.Label;

public interface ILabelDL
{
    Task<int> CreateLabel(string labelName, int userId);
    Task<IEnumerable<Label>> GetLabels(int userId);
    Task<bool> UpdateLabel(int labelId, string labelName, int userId);
    Task<bool> DeleteLabel(int labelId, int userId);
    Task<bool> AddLabelToNote(int labelId, int noteId);
    Task<bool> RemoveLabelFromNote(int labelId, int noteId);
    Task<IEnumerable<Note>> GetNotesByLabel(int labelId, int userId);
}
