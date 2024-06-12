using System.ComponentModel.DataAnnotations;

namespace managementapp.Data.DTO;

public class MessageDTO
{
    public string? RecieverId { get; set; }
    public Guid? ChatroomId { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string MessageText { get; set; } 

}
