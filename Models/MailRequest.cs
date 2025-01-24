using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models;

public class MailRequest
{
    [Required]
    [EmailAddress]
    public string RecipientAddress { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;
} 