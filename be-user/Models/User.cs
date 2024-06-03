namespace project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string? Lastname { get; set; }   
        public string? Firstname { get; set; }
        public string Password { get; set; } = String.Empty;


    }
}
