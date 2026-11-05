using DIContainer;
using SoberProgress.DataAccess;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;
using PresenterSpace;

namespace SoberProgress.Presentation
{
    /// <summary>
    /// Основная точка входа
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var container = new Danject();

            container.Register<ICRUDRepository<AlcoUser>, EntityRepository<AlcoUser>>(Lifetime.Singleton);
            container.Register<SoberService, SoberService>(Lifetime.Transient);
            container.Register<Presenter, Presenter>(Lifetime.Transient);

            Console.Title = "SoberProgress - Точка сборки IoC/DI";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("    АРХИТЕКТУРНЫЙ КОРЕНЬ КОМПОЗИЦИИ (IoC)         ");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine("Выберите режим отображения приложения:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1 [WinForms]  — Графический оконный интерфейс");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("2 [Console]   — Текстовый терминальный интерфейс");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Ваш выбор (1 или 2): ");

            string choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                container.Register<IView, WinFormsView.MainForm>(Lifetime.Transient);
                Console.WriteLine("\nЗапуск графического интерфейса WinForms...");

                var presenter = container.Resolve<Presenter>();
                presenter.Launch();
            }
            else if (choice == "2")
            {
                container.Register<ICRUDRepository<AlcoUser>, DapperRepository<AlcoUser>>(Lifetime.Singleton);
                container.Register<IView, ConsoleView.ConsoleView>(Lifetime.Transient);

                Console.WriteLine("\nПереключение на DapperRepository...");
                Console.WriteLine("Запуск текстового интерфейса...");
                System.Threading.Thread.Sleep(1000);

                var presenter = container.Resolve<Presenter>();
                presenter.Launch();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nНеверный выбор. Работа программы завершена.");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }
}