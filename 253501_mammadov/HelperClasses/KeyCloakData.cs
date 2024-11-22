namespace _253501_mammadov.HelperClasses
{
    /// <summary>
    /// Класс для хранения данных подключения к Keycloak.
    /// </summary>
    public class KeycloakData
    {
        /// <summary>
        /// Хост (URL) Keycloak сервера.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Название Realm в Keycloak.
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// Идентификатор клиента в Keycloak.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Секретный ключ клиента для аутентификации.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
