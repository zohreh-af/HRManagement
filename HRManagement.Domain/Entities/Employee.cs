namespace HRManagement.Domain.Entities;


public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    public string EmployeeCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public DateTime? HireDate { get; set; }

    public Guid? UserId { get; set; }

    // Navigation property
    public User? UserNavigation { get; set; }
}
