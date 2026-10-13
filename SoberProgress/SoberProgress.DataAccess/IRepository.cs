namespace SoberProgress.Domain.ModelInterfaces;

/// <summary>
/// Репозиторий требующий отключений через Dispose
/// </summary>
/// <typeparam name="T">Тип объектов репозитория</typeparam>
public interface IRepository<T> : ICRUDRepository<T>, IDisposable where T : IDomainObject, new()
{
}
