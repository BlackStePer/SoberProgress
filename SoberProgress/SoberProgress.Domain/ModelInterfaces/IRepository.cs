namespace SoberProgress.Domain.ModelInterfaces
{
    /// <summary>
    /// Интерфейс репозитория
    /// </summary>
    /// <typeparam name="T">Тип объектов репозитория</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Получить колекцию всех объектов репозитория
        /// </summary>
        /// <returns>Коллекция объектов</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Получить данные об объекте по id
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Данные об объекте</returns>
        T ReadById(int id);

        /// <summary>
        /// Добавить новый объект в репощиторий
        /// </summary>
        /// <param name="item">Данные об объекте</param>
        void Create(T item);

        /// <summary>
        /// Обновить данные объекта в репозитории
        /// </summary>
        /// <param name="item">Данные об объекте</param>
        void Update(T item);

        /// <summary>
        /// Удалить объект из репозитория
        /// </summary>
        /// <param name="id">id объекта</param>
        void Delete(int id);
    }
}
