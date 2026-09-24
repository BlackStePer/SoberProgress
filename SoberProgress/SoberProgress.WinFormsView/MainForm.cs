using SoberProgress.Domain;

namespace SoberProgress.WinFormsView
{
    /// <summary>
    /// Основная форма программы
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly SoberService _service;

        public MainForm(SoberService service)
        {
            InitializeComponent();
            _service = service;

            FormBorderStyle = FormBorderStyle.FixedSingle; 
            MaximizeBox = false;                           
            StartPosition = FormStartPosition.CenterScreen;

            ConfigureDataGridView();
            UpdateLeaderboard();
        }

        private void ConfigureDataGridView()
        {
            dgvLeaderboard.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLeaderboard.MultiSelect = false;
            dgvLeaderboard.ReadOnly = true;
            dgvLeaderboard.AllowUserToAddRows = false;
            dgvLeaderboard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvLeaderboard.Columns.Clear();
            dgvLeaderboard.Columns.Add("Id", "ID");
            dgvLeaderboard.Columns.Add("FullName", "ФИО Пользователя");
            dgvLeaderboard.Columns.Add("DaysSober", "Дней трезвости");
            dgvLeaderboard.Columns.Add("Method", "Метод борьбы");

            dgvLeaderboard.Columns["Id"].FillWeight = 15;
        }

        private void UpdateLeaderboard()
        {
            dgvLeaderboard.Rows.Clear();

            var leaderboard = _service.GetLeaderboard();

            foreach (var user in leaderboard)
            {
                int days = _service.GetDaysSoberCount(user);
                string statusText = user.IsCoded ? "Закодирован" : "Самостоятельно";
                string fullName = $"{user.Surname} {user.Name} {user.Patronymic}";

                int rowIndex = dgvLeaderboard.Rows.Add(user.Id, fullName, days, statusText);

                if (user.IsCoded)
                {
                    dgvLeaderboard.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                    dgvLeaderboard.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkGoldenrod;
                }
            }
        }

        private int? GetSelectedUserId()
        {
            if (dgvLeaderboard.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(dgvLeaderboard.SelectedRows[0].Cells["Id"].Value);
            }
            MessageBox.Show("Пожалуйста, выберите пользователя из списка!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtSurname.Text))
            {
                MessageBox.Show("Имя и Фамилия обязательны для заполнения!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime selectedDate = dtpLastDrink.Value.Date;

            _service.AddUser(txtName.Text.Trim(), txtSurname.Text.Trim(), txtPatronymic.Text.Trim(), selectedDate);

            txtName.Clear();
            txtSurname.Clear();
            txtPatronymic.Clear();
            dtpLastDrink.Value = DateTime.Now;

            UpdateLeaderboard();
            MessageBox.Show("Пользователь успешно встал на путь трезвости!", "Победа!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            int? userId = GetSelectedUserId();
            if (userId == null) return;

            AlcoUser user = _service.ReadUser(userId.Value);
            if (user != null)
            {
                bool success = _service.Code(user);
                if (success)
                {
                    UpdateLeaderboard();
                    MessageBox.Show($"{user.Name} успешно закодирован, теперь он не такой алкаш!", "Кодирование успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Пользователь уже находится под кодировкой!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRelapse_Click(object sender, EventArgs e)
        {
            int? userId = GetSelectedUserId();
            if (userId == null) return;

            AlcoUser user = _service.ReadUser(userId.Value);
            if (user != null)
            {
                var dialogResult = MessageBox.Show($"Вы уверены, что хотите зафиксировать срыв для {user.Name}?", "Внимание! Срыв прогресса", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    user.LastDrinkDate = DateTime.Now;
                    user.IsCoded = false;

                    _service.ChangeUser(user);

                    UpdateLeaderboard();
                    MessageBox.Show("Счетчик трезвости обнулен. Путь начинается сначала.", "Данные обновлены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int? userId = GetSelectedUserId();
            if (userId == null) return;

            var dialogResult = MessageBox.Show("Удалить пользователя из системы трезвости?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                bool deleted = _service.RemoveUser(userId.Value);
                if (deleted)
                {
                    UpdateLeaderboard();
                    MessageBox.Show("Пользователь удален.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось найти или удалить пользователя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}