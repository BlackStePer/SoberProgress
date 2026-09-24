using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IRepository<AlcoUser> repository = new ClassicalRepo())
            {
                SoberService soberService = new SoberService(repository);

                ConsoleView app = new ConsoleView(soberService);
                app.Run();
            }
        }
    }
}