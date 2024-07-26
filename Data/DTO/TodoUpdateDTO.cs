namespace managementapp.Data.DTO;

public class TodoUpdateDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid userId { get; set; }
    public DateTime ExpiredAt { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; }
}

