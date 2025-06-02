using System.ComponentModel.DataAnnotations;

public class AuthUser
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
}