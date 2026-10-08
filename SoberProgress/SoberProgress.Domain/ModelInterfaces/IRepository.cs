namespace SoberProgress.Domain.ModelInterfaces;

/// <summary>
/// Интерфейс репозитория
/// </summary>
/// <typeparam name="T">Тип объектов репозитория</typeparam>
public interface IRepository<T> : IDisposable where T : class
{
    /// <summary>
    /// Получить список всех объектов репозитория
    /// </summary>
<<<<<<< HEAD
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
=======
    /// <returns></returns>
    IEnumerable<T> ReadAll();
    /// <summary>
    /// Получить объект по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    T ReadById(int id);
    /// <summary>
    /// Создать новый объект в репозитории
    /// </summary>
    /// <param name="item"></param>
    void Create(T item);
    /// <summary>
    /// Обновить данные о пользователе
    /// </summary>
    /// <param name="item">Данные пользователя</param>
    void Update(T item);
    /// <summary>
    /// Удалить объект из репозитория
    /// </summary>
    /// <param name="id">ID объекта</param>
    void Delete(int id);
>>>>>>> 20ae0d4ea8bca5299f83543b978f195dd2c91906
}
