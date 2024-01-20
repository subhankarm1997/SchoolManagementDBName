using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    [MaxLength(256)]
    public string FullName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    public string UserType { get; set; }

    public string ProfileImageUrl { get; set; }

    // Add other properties based on the user type

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
