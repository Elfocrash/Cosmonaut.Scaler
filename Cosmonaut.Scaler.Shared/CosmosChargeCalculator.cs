using System;

namespace Cosmonaut.Scaler.Shared
{
    public class CosmosChargeCalculator
    {
        private const double DollarsPerRu = 0.00008;
        private const int HoursInADay = 24;
        private const int DaysInAYear = 365;

        public static double GetEstimatedHourlyCharge(int throughput)
        {
            return DollarsPerRu * throughput;
        }

        public static double GetEstimatedDailyCharge(int throughput)
        {
            return GetEstimatedHourlyCharge(throughput) * HoursInADay;
        }

        public static double GetAverageMonthlyCharge(int throughput)
        {
            var daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            return GetEstimatedDailyCharge(throughput) * daysInCurrentMonth;
        }
    }
}