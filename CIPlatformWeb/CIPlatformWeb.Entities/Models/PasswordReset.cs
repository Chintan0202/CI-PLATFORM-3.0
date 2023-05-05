using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIPlatformMain.Entities.Models;

public partial class PasswordReset
{
    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int Id { get; set; }


    [NotMapped]
    [Required(ErrorMessage = "Must Enter Passowrd")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "Must Enter 4 char password")]
    public string Password { get; set; }
    [NotMapped]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "Must Enter 4 char password")]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    public string ConfirmPassword { get; set; }
}
