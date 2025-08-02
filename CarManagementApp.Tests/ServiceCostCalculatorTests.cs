using CarManagementApp.Domain.Services;
using Xunit;

namespace CarManagementApp.Tests
{
    public class ServiceCostCalculatorTests
    {
        [Fact]
        public void CalculateServiceCost_WithWholeHours_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal hoursWorked = 3.0m;
            decimal expectedCost = 225.00m; // 3 hours * €75

            // Act
            decimal actualCost = ServiceCostCalculator.CalculateServiceCost(hoursWorked);

            // Assert
            Assert.Equal(expectedCost, actualCost);
        }

        [Fact]
        public void CalculateServiceCost_WithPartialHours_ShouldRoundUp()
        {
            // Arrange
            decimal hoursWorked = 2.5m;
            decimal expectedCost = 225.00m; // 3 hours * €75 (rounded up from 2.5)

            // Act
            decimal actualCost = ServiceCostCalculator.CalculateServiceCost(hoursWorked);

            // Assert
            Assert.Equal(expectedCost, actualCost);
        }

        [Fact]
        public void CalculateServiceCost_WithZeroHours_ShouldReturnZero()
        {
            // Arrange
            decimal hoursWorked = 0m;
            decimal expectedCost = 0m;

            // Act
            decimal actualCost = ServiceCostCalculator.CalculateServiceCost(hoursWorked);

            // Assert
            Assert.Equal(expectedCost, actualCost);
        }

        [Fact]
        public void CalculateServiceCost_WithNegativeHours_ShouldReturnZero()
        {
            // Arrange
            decimal hoursWorked = -1.5m;
            decimal expectedCost = 0m;

            // Act
            decimal actualCost = ServiceCostCalculator.CalculateServiceCost(hoursWorked);

            // Assert
            Assert.Equal(expectedCost, actualCost);
        }

        [Fact]
        public void CalculateServiceCost_WithSmallPartialHours_ShouldRoundUp()
        {
            // Arrange
            decimal hoursWorked = 0.1m;
            decimal expectedCost = 75.00m; // 1 hour * €75 (rounded up from 0.1)

            // Act
            decimal actualCost = ServiceCostCalculator.CalculateServiceCost(hoursWorked);

            // Assert
            Assert.Equal(expectedCost, actualCost);
        }

        [Fact]
        public void GetHourlyRate_ShouldReturnCorrectRate()
        {
            // Arrange
            decimal expectedRate = 75.00m;

            // Act
            decimal actualRate = ServiceCostCalculator.GetHourlyRate();

            // Assert
            Assert.Equal(expectedRate, actualRate);
        }

        [Theory]
        [InlineData(1.0, 75.00)]
        [InlineData(1.5, 150.00)]
        [InlineData(2.0, 150.00)]
        [InlineData(2.1, 225.00)]
        [InlineData(3.0, 225.00)]
        [InlineData(4.7, 375.00)]
        public void CalculateServiceCost_WithVariousHours_ShouldCalculateCorrectly(decimal hours, decimal expectedCost)
        {
            // Act
            decimal actualCost = ServiceCostCalculator.CalculateServiceCost(hours);

            // Assert
            Assert.Equal(expectedCost, actualCost);
        }
    }
} 