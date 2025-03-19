namespace BakingSisters.Api.Models;

public class BaseModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = "admin";
    public DateTime UpdatedAt { get; set; }
    public string UpdateBy { get; set; } = string.Empty;
}