using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime HireDate { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }
} 