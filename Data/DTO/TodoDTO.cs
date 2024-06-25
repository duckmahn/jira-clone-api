namespace managementapp.Data.DTO;

public class TodoDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public Guid ProjectId { get; set; }

    public DateTime ExpiredAt { get; set; }
}

