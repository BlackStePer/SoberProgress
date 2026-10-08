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

            if(user is null)
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
            if(user is null)
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
            if(user is null || user.IsCoded)
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

        /// <summary>
        /// Получить статистические данныые
        /// </summary>
        /// <returns>Кортеж содержащий: (Среднюю трезвость, количество пользователей, количество закодированных, процент закодированных среди всех)</returns>
        public (double avgDays, int totalUsers, int codedCount, double codedPercentage) GetSystemStatistics()
        {
            var users = _repository.ReadAll().ToList();
            if (!users.Any()) return (0, 0, 0, 0);

            int total = users.Count;
            int coded = users.Count(u => u.IsCoded);
            double avgDays = users.Average(u => GetDaysSoberCount(u));
            double percentage = ((double)coded / total) * 100;

            return (Math.Round(avgDays, 1), total, coded, Math.Round(percentage, 1));
        }

        /// <summary>
        /// Получить психологический ранг пользователя
        /// </summary>
        public string GetSoberStatus(AlcoUser user)
        {
            int days = GetDaysSoberCount(user);

            if (days == 0) return "Критическая фаза";
            if (days < 7) return "Детоксикация";
            if (days < 30) return "Первые шаги";
            if (days < 90) return "Уверенный подъем";
            if (days < 365) return "Стабильная трезвость";
            return "Трезвый мудрец";
        }
    }
}
