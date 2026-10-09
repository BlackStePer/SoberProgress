namespace SoberProgress.Domain.ModelInterfaces;

/// <summary>
/// Репозиторий модели, для хранилищь данных не требующие подключения
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDomainRepository<T> where T : IDomainObject, new()
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
