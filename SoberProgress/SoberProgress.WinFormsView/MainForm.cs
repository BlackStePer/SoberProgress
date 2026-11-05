using PresenterSpace;
using PresenterSpace.DTO;

namespace SoberProgress.WinFormsView
{
    /// <summary>
    /// Основная форма программы
    /// </summary>
    public partial class MainForm : Form, IView
    {
        public event Action AddUserRequested;
        public event Action CodeUserRequested;
        public event Action RelapseUserRequested;
        public event Action DeleteUserRequested;


        public string InputName => txtName.Text;
        public string InputSurname => txtSurname.Text;
        public string InputPatronymic => txtPatronymic.Text;
        public DateTime? InputLastDrinkDate => dtpLastDrink.Value.Date;

        public int? SelectedUserId
        {
            get
            {
                if (dgvLeaderboard.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvLeaderboard.SelectedRows[0].Cells["Id"].Value);
                }
                ShowMessage("Пожалуйста, выберите пользователя из списка!", isError: true);
                return null;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            ConfigureDataGridView();
            WireUiEvents();
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
            dgvLeaderboard.Columns.Add("SoberRank", "Ранг трезвости");

            dgvLeaderboard.Columns["Id"].FillWeight = 15;
        }

        private void WireUiEvents()
        {
            btnAdd.Click += (s, e) => AddUserRequested?.Invoke();
            btnCode.Click += (s, e) => CodeUserRequested?.Invoke();
            btnRelapse.Click += (s, e) => RelapseUserRequested?.Invoke();
            btnDelete.Click += (s, e) => DeleteUserRequested?.Invoke();
        }

        public void DisplayLeaderboard(IEnumerable<AlcoUserDTO> leaderboard)
        {
            dgvLeaderboard.Rows.Clear();

            foreach (var dto in leaderboard)
            {
                int rowIndex = dgvLeaderboard.Rows.Add(dto.Id, dto.FullName, dto.DaysSober, dto.Method, dto.SoberRank);

                if (dto.IsCoded)
                {
                    dgvLeaderboard.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                    dgvLeaderboard.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkGoldenrod;
                }
            }
        }

        public void DisplayStatistics(SystemStatisticsDTO stats)
        {
            Stats.Text = $"  [АНАЛИТИКА СИСТЕМЫ]  " +
                         $"Всего на учете: {stats.TotalUsers} чел.  |  " +
                         $"Средняя трезвость: {stats.AvgDaysSober} дн.  |  " +
                         $"Закодировано: {stats.CodedCount} ({stats.CodedPercentage}%)";
        }

        public void ShowMessage(string message, bool isError = false)
        {
            MessageBoxIcon icon = isError ? MessageBoxIcon.Error : MessageBoxIcon.Information;
            string caption = isError ? "Внимание / Ошибка" : "Успех";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
        }

        public bool ConfirmAction(string message)
        {
            var dialogResult = MessageBox.Show(message, "Подтверждение действия", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return dialogResult == DialogResult.Yes;
        }

        public void ClearInputFields()
        {
            txtName.Clear();
            txtSurname.Clear();
            txtPatronymic.Clear();
            dtpLastDrink.Value = DateTime.Now;
        }

        public void Run()
        {
            Application.Run(this);
        }
    }
}