using System;
using System.Collections.Generic;

namespace SchoolManagementDatabaseFirst.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserType { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string ProfileImageUrl { get; set; } = null!;

    public string Password { get; set; } = null!;
}
