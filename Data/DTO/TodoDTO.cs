namespace managementapp.Data.DTO;

public class TodoDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid userId { get; set; }
    public DateTime ExpiredAt { get; set; }
}

