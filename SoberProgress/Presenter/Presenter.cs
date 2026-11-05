using PresenterSpace.DTO;
using SoberProgress.Domain;

namespace PresenterSpace
{
    /// <summary>
    /// Класс презентера
    /// </summary>
    public class Presenter
    {
        private readonly SoberService _service;
        private readonly IView _view;

        public Presenter(SoberService service, IView view)
        {
            _service = service;
            _view = view;

            _view.AddUserRequested += OnAddUser;
            _view.CodeUserRequested += OnCodeUser;
            _view.RelapseUserRequested += OnRelapseUser;
            _view.DeleteUserRequested += OnDeleteUser;
        }

        /// <summary>
        /// Запуск
        /// </summary>
        public void Launch()
        {
            UpdateDataOnUi();
            _view.Run();
        }

        private void UpdateDataOnUi()
        {
            var domainLeaderboard = _service.GetLeaderboard();

            var leaderboardDto = domainLeaderboard.Select(user => new AlcoUserDTO
            {
                Id = user.Id,
                FullName = $"{user.Surname} {user.Name} {user.Patronymic}",
                DaysSober = _service.GetDaysSoberCount(user),
                Method = user.IsCoded ? "Закодирован" : "Самостоятельно",
                SoberRank = _service.GetSoberStatus(user),
                IsCoded = user.IsCoded
            }).ToList();

            var rawStats = _service.GetSystemStatistics();
            var statsDto = new SystemStatisticsDTO
            {
                AvgDaysSober = rawStats.avgDays,
                TotalUsers = rawStats.totalUsers,
                CodedCount = rawStats.codedCount,
                CodedPercentage = rawStats.codedPercentage
            };

            _view.DisplayLeaderboard(leaderboardDto);
            _view.DisplayStatistics(statsDto);
        }

        private void OnAddUser()
        {
            if (string.IsNullOrWhiteSpace(_view.InputName) || string.IsNullOrWhiteSpace(_view.InputSurname))
            {
                _view.ShowMessage("Имя и Фамилия обязательны для заполнения!", isError: true);
                return;
            }

            _service.AddUser(_view.InputName.Trim(), _view.InputSurname.Trim(), _view.InputPatronymic.Trim(), _view.InputLastDrinkDate);
            _view.ClearInputFields();
            UpdateDataOnUi();
            _view.ShowMessage("Пользователь успешно добавлен в систему!");
        }

        private void OnCodeUser()
        {
            int? id = _view.SelectedUserId;
            if (id == null) return;

            var user = _service.ReadUser(id.Value);
            if (user != null)
            {
                if (_service.Code(user))
                {
                    UpdateDataOnUi();
                    _view.ShowMessage($"{user.Name} успешно закодирован!");
                }
                else
                {
                    _view.ShowMessage("Пользователь уже находится под кодировкой!", isError: true);
                }
            }
        }

        private void OnRelapseUser()
        {
            int? id = _view.SelectedUserId;
            if (id == null) return;

            var user = _service.ReadUser(id.Value);
            if (user != null)
            {
                if (_view.ConfirmAction($"Вы уверены, что хотите зафиксировать срыв для {user.Name}?"))
                {
                    user.LastDrinkDate = DateTime.Now;
                    user.IsCoded = false;
                    _service.ChangeUser(user);

                    UpdateDataOnUi();
                    _view.ShowMessage("Счетчик трезвости обнулен. Путь начинается сначала.");
                }
            }
        }

        private void OnDeleteUser()
        {
            int? id = _view.SelectedUserId;
            if (id == null) return;

            if (_view.ConfirmAction("Удалить пользователя из системы трезвости?"))
            {
                if (_service.RemoveUser(id.Value))
                {
                    UpdateDataOnUi();
                    _view.ShowMessage("Пользователь успешно удален.");
                }
                else
                {
                    _view.ShowMessage("Не удалось найти или удалить пользователя.", isError: true);
                }
            }
        }
    }
}

