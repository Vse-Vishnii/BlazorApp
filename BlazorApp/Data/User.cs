using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Data
{
    [NotMapped]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

        public string[] Roles { get; } = {"visitor", "admin"};

        public User()
        {
            UserName = "";
            Password = "";
            Role = "visitor";
        }
    }
}
