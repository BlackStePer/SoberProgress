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
}
