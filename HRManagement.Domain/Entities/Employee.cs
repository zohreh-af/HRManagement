using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Domain.Entities;


[Table("Employees")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int EmployeeId { get; set; }

    [Column("FirstName")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Column("LastName")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Column("Email")]
    [StringLength(150)]
    public string? Email { get; set; }

    [Column("HireDate")]
    public DateTime? HireDate { get; set; }

    [Column("UserId")]
    public Guid? UserId { get; set; }

    // Navigation property
    public User? User { get; set; }
}
