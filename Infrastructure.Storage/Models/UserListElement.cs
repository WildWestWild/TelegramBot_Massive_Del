namespace Infrastructure.Storage.Models
{
    public class UserListElement
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Внешний ключ на таблицу UserListInfo
        /// </summary>
        public long UserListInfoId { get; set; }

        public UserListInfo UserListInfo { get; set; }

        /// <summary>
        /// Номер элемента
        /// </summary>
        public ushort Number { get; set; }

        /// <summary>
        /// Наполнение элемента
        /// </summary>
        public string Data { get; set; }
        
        /// <summary>
        /// Вычеркнут ли элемент
        /// </summary>
        public bool IsStrikingOut { get; set; }
    }
}
