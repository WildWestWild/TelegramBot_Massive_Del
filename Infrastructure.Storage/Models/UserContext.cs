using System.Data;

namespace Infrastructure.Storage.Models;

public class UserContext
{
    /// <summary>
    /// Идентификатор чатов (ChatId)
    /// </summary>
    public long ChatId { get; set; }
    
    /// <summary>
    /// Идентификатор для джойнов
    /// </summary>
    public long UserListInfoId { get; set; }
    
    /// <summary>
    /// Является ли активным данный список?
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Последняя активная команда
    /// </summary>
    public int? Command { get; set; }
}