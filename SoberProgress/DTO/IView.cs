using PresenterSpace.DTO;

namespace PresenterSpace
{
    /// <summary>
    /// Интерфейс вьюх.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Событие запроса на регистрацию нового пользователя.
        /// </summary>
        event Action AddUserRequested;

        /// <summary>
        /// Событие отправки запроса на кодирование.
        /// </summary>
        event Action CodeUserRequested;

        /// <summary>
        /// Событие подтверждения факта срыва.
        /// </summary>
        event Action RelapseUserRequested;

        /// <summary>
        /// Событие отправки запроса на удаление.
        /// </summary>
        event Action DeleteUserRequested;

        public string InputName { get; }
        public string InputSurname { get; }
        public string InputPatronymic { get; }
        public DateTime? InputLastDrinkDate { get; }
        public int? SelectedUserId { get; }

        /// <summary>
        /// Выводит таблицу тир-листа лидеров трезвости на экран.
        /// </summary>
        /// <param name="leaderboard">Коллекция DTO пользователей.</param>
        void DisplayLeaderboard(IEnumerable<AlcoUserDTO> leaderboard);

        /// <summary>
        /// Выводит данные системы.
        /// </summary>
        /// <param name="stats">Коллекция DTO статистики.</param>
        void DisplayStatistics(SystemStatisticsDTO stats);

        /// <summary>
        /// Отображает уведомление об ошибке.
        /// </summary>
        /// <param name="message">Текст уведомления.</param>
        /// <param name="isError">Флаг, указывающий на то, является ли сообщение критической ошибкой.</param>
        void ShowMessage(string message, bool isError = false);

        /// <summary>
        /// Запрашивает подтверждение выполнения опасного действия.
        /// </summary>
        /// <param name="message">Текст вопроса.</param>
        /// <returns>Соглсилие.</returns>
        bool ConfirmAction(string message);

        /// <summary>
        /// Производит очистку всех доступных полей пользовательского ввода.
        /// </summary>
        void ClearInputFields();

        /// <summary>
        /// Активирует и запускает бесконечный цикл обработки или отрисовки текущего интерфейса.
        /// </summary>
        void Run();
    }
}
