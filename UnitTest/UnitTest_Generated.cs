using Xunit;

namespace DemoUnitTest_ConsoleApp.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_WhenAddingTwoNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 3;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void Subtract_WhenSubtractingTwoNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 3;

            // Act
            int result = calculator.Subtract(a, b);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Multiply_WhenMultiplyingTwoNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 3;

            // Act
            int result = calculator.Multiply(a, b);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void Divide_WhenDividingTwoNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 6;
            int b = 3;

            // Act
            int result = calculator.Divide(a, b);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Divide_WhenDividingByZero_ThrowsDivideByZeroException()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 6;
            int b = 0;

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(a, b));
        }
    }
}