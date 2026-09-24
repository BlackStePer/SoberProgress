namespace SoberProgress.Domain.ModelInterfaces
{
    /// <summary>
    /// Интерфейс репозитория
    /// </summary>
    /// <typeparam name="T">Тип объектов репозитория</typeparam>
    public interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> ReadAll();
        T ReadById(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);
    }
}
