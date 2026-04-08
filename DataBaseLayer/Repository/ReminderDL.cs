using Dapper;
using System.Data;
using ModelLayer.Entities;

public class ReminderDL : IReminderDL
{
    private readonly IDbConnection _db;

    public ReminderDL(IDbConnection db)
    {
        _db = db;
    }

    public async Task<int> CreateReminder(DateTime dateTime, int noteId)
    {
        var query = @"
        INSERT INTO Reminders (DateTime, Status, NotesId)
        VALUES (@DateTime, 'Pending', @NoteId);
        SELECT CAST(SCOPE_IDENTITY() as int);";

        return await _db.ExecuteScalarAsync<int>(query, new
        {
            DateTime = dateTime,
            NoteId = noteId
        });
    }

    public async Task<IEnumerable<Reminder>> GetReminders(int noteId)
    {
        var query = "SELECT * FROM Reminders WHERE NotesId=@NoteId";
        return await _db.QueryAsync<Reminder>(query, new { NoteId = noteId });
    }

    public async Task<bool> UpdateReminder(int reminderId, DateTime dateTime, string status)
    {
        var query = @"
        UPDATE Reminders
        SET DateTime=@DateTime, Status=@Status
        WHERE ReminderId=@ReminderId";

        return await _db.ExecuteAsync(query, new
        {
            ReminderId = reminderId,
            DateTime = dateTime,
            Status = status
        }) > 0;
    }

    public async Task<bool> DeleteReminder(int reminderId)
    {
        var query = "DELETE FROM Reminders WHERE ReminderId=@ReminderId";
        return await _db.ExecuteAsync(query, new { ReminderId = reminderId }) > 0;
    }
}
