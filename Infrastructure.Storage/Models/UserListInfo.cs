namespace Infrastructure.Storage.Models
{
    public class UserListInfo
    {
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
        /// Активный контекст (на какой список сейчас смотрит пользователь)
        /// </summary>
        public bool IsActiveContext { get; set; }

        /// <summary>
        /// Коллекция из элементов списка
        /// </summary>
        public ICollection<UserListElement> UserListElements { get; set; }
    }
}
