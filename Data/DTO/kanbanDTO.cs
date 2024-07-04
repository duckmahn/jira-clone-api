using managementapp.Data.Models;

namespace managementapp.Data.DTO;

public class kanbanDTO
{
    public string Name { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}
