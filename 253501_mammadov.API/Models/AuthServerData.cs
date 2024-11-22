namespace _253501_mammadov.API.Models
{
    /// <summary>
    /// Класс, описывающий данные сервера аутентификации.
    /// </summary>
    public class AuthServerData
    {
        /// <summary>
        /// Адрес сервера (хост).
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Название Realm (пространства имен) в Keycloak.
        /// </summary>
        public string Realm { get; set; }
    }
}
