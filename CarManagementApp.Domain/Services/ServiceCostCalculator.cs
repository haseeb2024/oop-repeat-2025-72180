namespace CarManagementApp.Domain.Services
{
    public static class ServiceCostCalculator
    {
        private const decimal HourlyRate = 75.00m;
        
        /// <summary>
        /// Calculates the total cost of service based on hours worked.
        /// Cost is €75 per hour (or partial hour) - rounded up.
        /// </summary>
        /// <param name="hoursWorked">The number of hours worked</param>
        /// <returns>The total cost rounded up to the nearest hour</returns>
        public static decimal CalculateServiceCost(decimal hoursWorked)
        {
            if (hoursWorked <= 0)
                return 0;
            
            // Round up to the nearest hour
            decimal roundedHours = Math.Ceiling(hoursWorked);
            
            return roundedHours * HourlyRate;
        }
        
        /// <summary>
        /// Gets the hourly rate for services
        /// </summary>
        /// <returns>The hourly rate</returns>
        public static decimal GetHourlyRate()
        {
            return HourlyRate;
        }
    }
} 