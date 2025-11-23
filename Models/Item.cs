namespace TRACK_BACKEND.Models;

public class Item
{
    public int Id { get; set; }                       
    public string Name { get; set; } = string.Empty;  
    public string Location { get; set; } = string.Empty;
    public int Price { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    
    // New fields for coordinates
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // NEW: Category
    public string Category { get; set; } = "Groceries";
}
