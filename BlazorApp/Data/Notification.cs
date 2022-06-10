using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Data
{
    public class Notification : DbEntity
    {
        [Required]
        public string Text { get; set; }
    }
}
