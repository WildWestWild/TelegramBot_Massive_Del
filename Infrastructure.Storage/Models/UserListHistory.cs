namespace Infrastructure.Storage.Models;

public class UserListHistory
{
    public long ChatId { get; set; }
    
    public string ListName { get; set; }

    public DateTime LastUseDate { get; set; }
}