using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Data
{
    [NotMapped]
    public class Session : DbEntity
    {
        public string UserId { get; set; }
        public bool Blocked { get; set; } = false;
    }
}
