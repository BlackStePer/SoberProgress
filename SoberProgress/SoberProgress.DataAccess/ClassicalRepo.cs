using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.Domain
{
    /// <summary>
    /// Реализация IRtpository через список
    /// </summary>
    public class ClassicalRepo : IRepository<AlcoUser>
    {
        private readonly List<AlcoUser> _users = new List<AlcoUser>();

        private int _nextId = 1;

        public IEnumerable<AlcoUser> ReadAll()
        {
            return _users.ToList();
        }

        public AlcoUser ReadById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void Create(AlcoUser item)
        {
            if (item == null) return;

            item.Id = _nextId++;
            _users.Add(item);
        }

        public void Update(AlcoUser item)
        {
            if (item == null) return;

            AlcoUser? existingUser = _users.FirstOrDefault(u => u.Id == item.Id);
            if (existingUser != null)
            {
                existingUser.Name = item.Name;
                existingUser.Surname = item.Surname;
                existingUser.Patronymic = item.Patronymic;
                existingUser.LastDrinkDate = item.LastDrinkDate;
                existingUser.IsCoded = item.IsCoded;
            }
        }

        public void Delete(int id)
        {
            AlcoUser? user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }
    }
}
