using SoberProgress.DataAccess;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.WinFormsView
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IRepository<AlcoUser> repository = new EntityRepository<AlcoUser>();

            SoberService soberService = new SoberService(repository);

            //SeedData(soberService);

            Application.Run(new MainForm(soberService));
        }

        private static void SeedData(SoberService service)
        {
            service.AddUser("Даниил", "Иванов", "Павлович", DateTime.Now.AddDays(-67));
            service.AddUser("Илья", "Кузнецов", "Сергеевич", DateTime.Now.AddDays(-52));
            service.AddUser("Алексей", "Зайцев", "Степанович", DateTime.Now.AddDays(-1488));
            service.AddUser("Виктор", "Птицеедов", "Петухов", DateTime.Now.AddDays(-1));
            service.AddUser("Тимофей", "Милуш", "Дотерович", DateTime.Now.AddDays(0));


            var users = service.GetAllUsers();
            foreach (var u in users)
            {
                if (u.Surname == "Зайцев")
                {
                    service.Code(u);
                }
            }
        }
    }
}