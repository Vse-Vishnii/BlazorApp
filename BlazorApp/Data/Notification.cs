using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Data
{
    public class Notification : DbEntity
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Notification(string message, DateTime date)
        {
            Text = message;
            Date = date;
            Id = Guid.NewGuid().ToString();
        }
    }
}
