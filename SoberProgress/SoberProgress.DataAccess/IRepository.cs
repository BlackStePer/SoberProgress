namespace SoberProgress.Domain.ModelInterfaces;

/// <summary>
/// Репозиторий для объектов требующих подключения
/// </summary>
/// <typeparam name="T">Тип объектов репозитория</typeparam>
public interface IRepository<T> : IDomainRepository<T> where T : IDomainObject, new()
{
}
