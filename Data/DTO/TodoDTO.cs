﻿namespace managementapp.Data.DTO;

public class TodoDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } //hidden
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
}

