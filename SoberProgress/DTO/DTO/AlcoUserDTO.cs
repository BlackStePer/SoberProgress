namespace PresenterSpace.DTO
{
    public class AlcoUserDTO
    {
        /// <summary>
        /// Данные пользователя
        /// </summary>
        public int Id { get; set; }
        public string FullName { get; set; }
        public int DaysSober { get; set; }
        public string Method { get; set; } 
        public string SoberRank { get; set; } 
        public bool IsCoded { get; set; }
    }
}
