namespace managementapp.Data.Models;

public class Message
{
    public Guid Id { get; set; }//hidden
    public string? Chatroom { get; set; }
    public string MessageText { get; set; }
    public DateTime TimeStamp { get; set; }
    public Guid SenderId { get; set; } 
    public Guid? RecieverId { get; set; }
}
