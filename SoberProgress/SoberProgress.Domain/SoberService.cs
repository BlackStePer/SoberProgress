using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.Domain
{
    /// <summary>
    /// Класс бизнес логики приложения
    /// </summary>
    public class SoberService
    {
        private IRepository<AlcoUser> _repository;

        public SoberService(IRepository<AlcoUser> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Добавления пользователя в репозиторий
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="lastDrinkDate">Дата последнего срыва</param>
        public void AddUser(string name, string surname, string patronymic, DateTime? lastDrinkDate)
        {
            AlcoUser newUser = new AlcoUser(name, surname, patronymic)
            {
                RegistryDate = DateTime.Now,
                LastDrinkDate = lastDrinkDate,
                IsCoded = false
            };

            _repository.Create(newUser);
        }

        /// <summary>
        /// Удаление пользователя из репозитория
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Выполнилось ли удаление</returns>
        public bool RemoveUser(int id)
        {
            AlcoUser user = _repository.ReadById(id);

            if(user == null)
            {
                return false;
            }

            _repository.Delete(id);

            return true;
        }

        /// <summary>
        /// Найти пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Данные пользователя</returns>
        public AlcoUser ReadUser(int id)
        {
            return _repository.ReadById(id);
        }

        /// <summary>
        /// Изменить данные пользователя в репозитории
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        /// <returns>Удлось ли произвести изменения</returns>
        public bool ChangeUser(AlcoUser user)
        {
            int id;
            try
            {
                id = user.Id;
            }
            catch (Exception ex)
            {
                return false;
            }

            _repository.Update(user);
            return true;
        }

        /// <summary>
        /// Кодирование пользователя(Изменение IsCode на true)
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        /// <returns></returns>
        public bool Code(AlcoUser user)
        {
            if(user == null || user.IsCoded)
            {
                return false;
            }

            user.IsCoded = true;

            _repository.Update(user);

            return true;
        }

        /// <summary>
        /// Узнать скролько дней пользователь трезв
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        /// <returns>Количество трезвых дней</returns>
        public int GetDaysSoberCount(AlcoUser user)
        {
            if (user.LastDrinkDate == null)
                return 0;

            int days = (DateTime.Now.Date - user.LastDrinkDate.Value.Date).Days;

            return days < 0 ? 0 : days;
        }

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        /// <returns>Список пользователей</returns>
        public IEnumerable<AlcoUser> GetAllUsers()
        {
            return _repository.ReadAll();
        }

        /// <summary>
        /// Получить актуальный топ алкашей
        /// </summary>
        /// <returns>Отсортированная коллекция алкашей</returns>
        public IEnumerable<AlcoUser> GetLeaderboard()
        {
            return _repository.ReadAll()
                .OrderByDescending(user => GetDaysSoberCount(user))
                .ThenByDescending(user => user.IsCoded); 
        }
    }
}
