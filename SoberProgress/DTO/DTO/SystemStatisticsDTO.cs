namespace PresenterSpace.DTO
{
    /// <summary>
    /// Cтатистика системы
    /// </summary>
    public class SystemStatisticsDTO
    {
        public int TotalUsers { get; set; }
        public double AvgDaysSober { get; set; }
        public int CodedCount { get; set; }
        public double CodedPercentage { get; set; }
    }
}
