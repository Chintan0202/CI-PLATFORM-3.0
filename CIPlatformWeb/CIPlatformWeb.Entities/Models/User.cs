using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIPlatformMain.Entities.Models;

public partial class User
{
    public long UserId { get; set; }

    [Required(ErrorMessage = "Must Enter First Name")]
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }
    [Required(ErrorMessage = "Must Enter Email-Id")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]

    public string Email { get; set; } = null!;

    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{8,}$",
                   ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one special character, and one numeric digit.")]
    [Required(ErrorMessage = "Must Enter Passowrd")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "Must Enter 4 char password")]
    public string Password { get; set; } = null!;

    [NotMapped]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "Must Enter 4 char password")]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    public string ConformPassword { get; set; }

    public int PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public string? WhyIVolunteer { get; set; }

    public string? EmployeeId { get; set; }

    public string? Department { get; set; }

    public long? CityId { get; set; }

    public long? CountryId { get; set; }

    public string? ProfileText { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? Title { get; set; }

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Avability { get; set; }

    public string? Manager { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<ContactU> ContactUs { get; } = new List<ContactU>();

    public virtual ICollection<FavoriteMission> FavoriteMissions { get; } = new List<FavoriteMission>();

    public virtual ICollection<MissionApplication> MissionApplications { get; } = new List<MissionApplication>();

    public virtual ICollection<MissionInvite> MissionInviteFromUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionInvite> MissionInviteToUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();

    public virtual ICollection<Story> Stories { get; } = new List<Story>();

    public virtual ICollection<StoryInvite> StoryInviteFromUsers { get; } = new List<StoryInvite>();

    public virtual ICollection<StoryInvite> StoryInviteToUsers { get; } = new List<StoryInvite>();

    public virtual ICollection<Timesheet> Timesheets { get; } = new List<Timesheet>();

    public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();
}
