using ModelLayer.Entities;

public interface IReminderBL
{
    Task<int> CreateReminder(DateTime dateTime, int noteId);
    Task<IEnumerable<Reminder>> GetReminders(int noteId);
    Task<bool> UpdateReminder(int reminderId, DateTime dateTime, string status);
    Task<bool> DeleteReminder(int reminderId);
}
