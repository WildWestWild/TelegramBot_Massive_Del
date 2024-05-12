namespace Infrastructure.Storage.Models
{
    public class UserListInfo
    {
        /// <summary>
        /// Идентификатор для джойнов
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор чатов (ChatId)
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }
        
        /// <summary>
        /// Кол-во символов во всех элементах списка
        /// </summary>
        public long CountSymbolsInList { get; set; }

        /// <summary>
        /// Коллекция из элементов списка
        /// </summary>
        public ICollection<UserListElement> UserListElements { get; set; }
    }
}
