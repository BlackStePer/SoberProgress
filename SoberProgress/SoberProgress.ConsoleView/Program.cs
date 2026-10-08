using SoberProgress.DataAccess;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            IRepository<AlcoUser> repository = new DapperRepository<AlcoUser>();
            SoberService soberService = new SoberService(repository);

            ConsoleView app = new ConsoleView(soberService);
            app.Run();
        }
    }
}