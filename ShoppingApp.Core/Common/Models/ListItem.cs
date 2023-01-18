namespace ShoppingApp.Core.Common.Models;

public class ListItem
{
    public string Id { get; set; }
    public string Value { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}