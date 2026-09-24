using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.Domain
{
    /// <summary>
    /// Сущность алкоголик, хранит даннные алкоголика
    /// </summary>
    public class AlcoUser : IDomainObject
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public DateTime? RegistryDate { get; set; }
        public DateTime? LastDrinkDate { get; set; }

        public bool IsCoded { get; set; }

        public AlcoUser() { }

        public AlcoUser(string name, string surname, string patronymic)
        {
            (Name, Surname, Patronymic) = (name, surname, patronymic);
        }


    }
}
