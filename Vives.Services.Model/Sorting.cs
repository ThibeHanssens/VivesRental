namespace Vives.Services.Model;

public class Sorting
{
    public Sorting()
    {
    }

    public Sorting(string propertyName, bool isDescending = false)
    {
        this.PropertyName = propertyName;
        this.IsDescending = isDescending;
    }

    public string? PropertyName { get; set; }

    public bool IsDescending { get; set; }
}