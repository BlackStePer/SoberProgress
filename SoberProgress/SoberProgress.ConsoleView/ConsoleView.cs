using System.Globalization;
using SoberProgress.Domain;

namespace SoberProgress.ConsoleView
{
    /// <summary>
    /// Консольное отображение приложения
    /// </summary>
    public class ConsoleView
    {
        private readonly SoberService _service;

        public ConsoleView(SoberService service)
        {
            _service = service;
            //SeedData();
        }

        /// <summary>
        /// Запуск основного меню
        /// </summary>
        public void Run()
        {
            bool running = true;
            while (running)
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
                    case "1": RegisterNewUser(); break;
                    case "2": ShowLeaderboardMenu(); break;
                    case "3": CodeUser(); break;
                    case "4": ProcessRelapse(); break;
                    case "5": DeleteUser(); break;
                    case "0": running = false; break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неверный ввод! Нажмите любую клавишу для повтора...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
        private void RegisterNewUser()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ ===");

            Console.Write("Введите Фамилию: ");
            string surname = Console.ReadLine();
            Console.Write("Введите Имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите Отчество: ");
            string patronymic = Console.ReadLine();

            Console.Write("Введите дату последнего употребления (ДД.ММ.ГГГГ) или оставьте пустой: ");
            string dateInput = Console.ReadLine();

            DateTime? lastDrinkDate = null;
            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    lastDrinkDate = parsedDate;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный формат даты! Зарегистрирован с пустой датой.");
                    Console.ResetColor();
                }
            }

            _service.AddUser(name, surname, patronymic, lastDrinkDate);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nПользователь успешно добавлен в систему трезвости!");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void ShowLeaderboard()
        {
            var leaderboard = _service.GetLeaderboard();
            foreach (var user in leaderboard)
            {
                int days = _service.GetDaysSoberCount(user);

                string statusText = user.IsCoded ? "Закодирован" : "Самостоятельно";
                string fullName = $"{user.Surname} {user.Name} {user.Patronymic}";

                if (user.IsCoded) Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"{user.Id,-4} | {fullName,-30} | {days,-10} | {statusText,-15}");
                Console.ResetColor();
            }
        }

        private void ShowLeaderboardMenu()
        {
            Console.Clear();
            Console.WriteLine("=========================================================================");
            Console.WriteLine("                       ГЛУБОКИЙ ТИР-ЛИСТ ТРЕЗВОСТИ                       ");
            Console.WriteLine("=========================================================================");
            Console.WriteLine($"{"ID",-4} | {"ФИО",-30} | {"Дней трезв",-10} | {"Статус",-15}");
            Console.WriteLine("-------------------------------------------------------------------------");

            ShowLeaderboard();

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        private void CodeUser()
        {
            Console.Clear();
            Console.WriteLine("=== ПРОЦЕДУРА КОДИРОВАНИЯ ===");

            ShowLeaderboard();

            Console.Write("Введите ID пользователя для кодирования: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                AlcoUser user = _service.ReadUser(id);
                if (user != null)
                {
                    bool success = _service.Code(user);
                    if (success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Успех! {user.Name} успешно закодирован программными методами!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ошибка! Пользователь уже закодирован или данные неверны.");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь с таким ID не найден.");
                }
            }
            Console.ResetColor();
            Console.ReadKey();
        }

        private void ProcessRelapse()
        {
            Console.Clear();
            Console.WriteLine("=== ФИКСАЦИЯ СРЫВА ===");

            ShowLeaderboard();

            Console.Write("Введите ID сорвавшегося пользователя: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                AlcoUser user = _service.ReadUser(id);
                if (user != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Внимание! Сбрасываем прогресс для {user.Surname} {user.Name}.");
                    Console.ResetColor();

                    user.LastDrinkDate = DateTime.Now;
                    user.IsCoded = false;

                    _service.ChangeUser(user);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Данные обновлены. Счетчик дней сброшен. Путь начинается заново!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь не найден.");
                }
            }
            Console.ResetColor();
            Console.ReadKey();
        }

        private void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление ИЗ СИСТЕМЫ ===");

            ShowLeaderboard();

            Console.Write("Введите ID пользователя для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool deleted = _service.RemoveUser(id);
                if (deleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Пользователь успешно удален из картотеки.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка! Пользователь с таким ID не найден.");
                }
            }
            Console.ResetColor();
            Console.ReadKey();
        }

        private void SeedData()
        {
            _service.AddUser("Даниил", "Иванов", "Павлович", DateTime.Now.AddDays(-67)); 
            _service.AddUser("Илья", "Кузнецов", "Сергеевич", DateTime.Now.AddDays(-52));
            _service.AddUser("Алексей", "Зайцев", "Степанович", DateTime.Now.AddDays(-1488));
            _service.AddUser("Виктор", "Птицеедов", "Петухов", DateTime.Now.AddDays(-1));
            _service.AddUser("Тимофей", "Милуш", "Дотерович", DateTime.Now.AddDays(0));


            var users = _service.GetAllUsers();
            foreach (var u in users)
            {
                if (u.Surname == "Зайцев")
                {
                    _service.Code(u);
                }
            }
        }
    }
}