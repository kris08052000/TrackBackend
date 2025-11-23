using System.ComponentModel.DataAnnotations.Schema;

namespace TRACK_BACKEND.Models;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}