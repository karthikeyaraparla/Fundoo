namespace ModelLayer.Entities
{
    public class Reminder
    {
        public int ReminderId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public int NotesId { get; set; }
    }
}
