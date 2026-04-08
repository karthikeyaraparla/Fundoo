using DataBaseLayer.Interfaces;
using ModelLayer.Entities;

public class ReminderBL : IReminderBL
{
    private readonly IReminderDL _reminderDL;

    public ReminderBL(IReminderDL reminderDL)
    {
        _reminderDL = reminderDL;
    }

    public Task<int> CreateReminder(DateTime dateTime, int noteId)
        => _reminderDL.CreateReminder(dateTime, noteId);

    public Task<IEnumerable<Reminder>> GetReminders(int noteId)
        => _reminderDL.GetReminders(noteId);

    public Task<bool> UpdateReminder(int reminderId, DateTime dateTime, string status)
        => _reminderDL.UpdateReminder(reminderId, dateTime, status);

    public Task<bool> DeleteReminder(int reminderId)
        => _reminderDL.DeleteReminder(reminderId);
}
