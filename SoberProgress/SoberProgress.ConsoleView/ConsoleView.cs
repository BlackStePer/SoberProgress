using System.Globalization;
using PresenterSpace;
using PresenterSpace.DTO;

namespace SoberProgress.ConsoleView
{
    /// <summary>
    /// Консольное отображение приложения
    /// </summary>
    public class ConsoleView : IView
    {
        public event Action AddUserRequested;
        public event Action CodeUserRequested;
        public event Action RelapseUserRequested;
        public event Action DeleteUserRequested;


        public string InputName { get; private set; }
        public string InputSurname { get; private set; }
        public string InputPatronymic { get; private set; }
        public DateTime? InputLastDrinkDate { get; private set; }
        public int? SelectedUserId { get; private set; }


        private List<AlcoUserDTO> _currentLeaderboard = new List<AlcoUserDTO>();
        private SystemStatisticsDTO _currentStats = new SystemStatisticsDTO();

        private bool _running = false;

        public void DisplayLeaderboard(IEnumerable<AlcoUserDTO> leaderboard)
        {
            _currentLeaderboard = leaderboard.ToList();
        }

        public void DisplayStatistics(SystemStatisticsDTO stats)
        {
            _currentStats = stats;
        }

        public void Run()
        {
            _running = true;
            while (_running)
            {
                RenderMainMenu();
            }
        }

        private void RenderMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("    ПРОГРАММА МОНИТОРИНГА ТРЕЗВОСТИ     ");
            Console.WriteLine("             «ТРЕЗВЫЙ ПУТЬ»             ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("1. Зарегистрировать нового пользователя");
            Console.WriteLine("2. Посмотреть глобальный Лидерборд");
            Console.WriteLine("3. Закодировать пользователя");
            Console.WriteLine("4. Изменить данные пользователя (Срыв)");
            Console.WriteLine("5. Удалить пользователя из системы");
            Console.WriteLine("0. Выход");
            Console.WriteLine("----------------------------------------");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": ProcessRegistrationInput(); break;
                case "2": ShowLeaderboardMenu(); break;
                case "3": ProcessIdSelection(CodeUserRequested, "=== ПРОЦЕДУРА КОДИРОВАНИЯ ==="); break;
                case "4": ProcessIdSelection(RelapseUserRequested, "=== ФИКСАЦИЯ СРЫВА ==="); break;
                case "5": ProcessIdSelection(DeleteUserRequested, "=== УДАЛЕНИЕ ИЗ СИСТЕМЫ ==="); break;
                case "0": _running = false; break;
                default:
                    ShowMessage("Неверный ввод! Нажмите любую клавишу для повтора...", isError: true);
                    Console.ReadKey();
                    break;
            }
        }

        private void ProcessRegistrationInput()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ ===");

            Console.Write("Введите Фамилию: ");
            InputSurname = Console.ReadLine();
            Console.Write("Введите Имя: ");
            InputName = Console.ReadLine();
            Console.Write("Введите Отчество: ");
            InputPatronymic = Console.ReadLine();

            Console.Write("Введите дату последнего употребления (ДД.ММ.ГГГГ) или оставьте пустой: ");
            string dateInput = Console.ReadLine();

            InputLastDrinkDate = null;
            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    InputLastDrinkDate = parsedDate;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный формат даты! Попытка регистрации с пустой датой.");
                    Console.ResetColor();
                }
            }

            AddUserRequested?.Invoke();
            Console.ReadKey();
        }

        private void PrintLeaderboardRows()
        {
            foreach (var dto in _currentLeaderboard)
            {
                if (dto.IsCoded) Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"{dto.Id,-4} | {dto.FullName,-30} | {dto.DaysSober,-10} | {dto.Method + " [" + dto.SoberRank + "]",-25}");
                Console.ResetColor();
            }
        }

        private void ShowLeaderboardMenu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[СТАТИСТИКА СИСТЕМЫ] Всего на учете: {_currentStats.TotalUsers} | Средняя трезвость: {_currentStats.AvgDaysSober} дн. | Закодировано: {_currentStats.CodedCount} ({_currentStats.CodedPercentage}%)");
            Console.ResetColor();

            Console.WriteLine("=========================================================================");
            Console.WriteLine("                       ГЛУБОКИЙ ТИР-ЛИСТ ТРЕЗВОСТИ                       ");
            Console.WriteLine("=========================================================================");
            Console.WriteLine($"{"ID",-4} | {"ФИО",-30} | {"Дней трезв",-10} | {"Статус",-15}");
            Console.WriteLine("-------------------------------------------------------------------------");

            PrintLeaderboardRows();

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        private void ProcessIdSelection(Action targetEvent, string headerText)
        {
            Console.Clear();
            Console.WriteLine(headerText);
            Console.WriteLine("-------------------------------------------------------------------------");
            PrintLeaderboardRows();
            Console.WriteLine("-------------------------------------------------------------------------");

            Console.Write("\nВведите ID пользователя для проведения операции: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                SelectedUserId = id;
                targetEvent?.Invoke();
            }
            else
            {
                ShowMessage("Неверный формат ID!", isError: true);
            }
            Console.ReadKey();
        }

        public void ShowMessage(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        public bool ConfirmAction(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"\n{message} (y/n): ");
            Console.ResetColor();
            string response = Console.ReadLine()?.Trim().ToLower();
            return response == "y" || response == "yes";
        }

        public void ClearInputFields()
        {
            InputName = null;
            InputSurname = null;
            InputPatronymic = null;
            InputLastDrinkDate = null;
            SelectedUserId = null;
        }
    }
}