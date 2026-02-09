using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

[Table("Employees")]
public class Employee
{
    [Key]
    public int EmployeeId { get; set; }
    [StringLength(150)]
    public string EmployeeCode { get; set; }
    [StringLength(150)]
    public string? FirstName { get; set; }
    [StringLength(150)]
    public string? LastName { get; set; }
    [StringLength(150)]
    public string? Email { get; set; }

    public DateTime? HireDate { get; set; }

    public Guid? UserId { get; set; }

    // Navigation property
    public User? UserNavigation { get; set; }
}
