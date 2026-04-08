using DataBaseLayer.Interfaces;
using Microsoft.Data.SqlClient.DataClassification;
using ModelLayer;

public class LabelBL : ILabelBL
{
    private readonly ILabelDL _labelDL;

    public LabelBL(ILabelDL labelDL)
    {
        _labelDL = labelDL;
    }

    public Task<int> CreateLabel(string labelName, int userId)
        => _labelDL.CreateLabel(labelName, userId);

    public Task<IEnumerable<Label>> GetLabels(int userId)
        => _labelDL.GetLabels(userId);

    public Task<bool> UpdateLabel(int labelId, string labelName, int userId)
        => _labelDL.UpdateLabel(labelId, labelName, userId);

    public Task<bool> DeleteLabel(int labelId, int userId)
        => _labelDL.DeleteLabel(labelId, userId);

    public Task<bool> AddLabelToNote(int labelId, int noteId)
        => _labelDL.AddLabelToNote(labelId, noteId);

    public Task<bool> RemoveLabelFromNote(int labelId, int noteId)
        => _labelDL.RemoveLabelFromNote(labelId, noteId);

    public Task<IEnumerable<Note>> GetNotesByLabel(int labelId, int userId)
        => _labelDL.GetNotesByLabel(labelId, userId);
}
