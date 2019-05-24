namespace API.Models
{
    public class EmployeeWithStatus
    {
        public string Status { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public EmployeeWithStatus(EmployeeToTrip e)
        {
            EmployeeID = e.Employee.EmployeeID;
            Name = e.Employee.Name;
            Email = e.Employee.Email;
            Status = e.Status;
        }
    }
}
