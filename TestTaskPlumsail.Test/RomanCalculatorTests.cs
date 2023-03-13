using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace TestTaskPlumsail
{
    [TestClass]
    public class RomanCalculatorTests
    {
        [TestMethod]
        [DataRow("", "input is null or empty")]
        [DataRow("1", "invalid input")]
        [DataRow("IVI", "invalid romanian number")]
        public void Evaluate_InvalidArguments_ThrowsArgumentExceptions(string input, string message)
        {
            var calculator = new RomanCalculator();

            try
            {
                calculator.Evaluate(input);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentException);
                Assert.AreEqual(message, e.Message);
            }
        }

        [TestMethod]
        [DataRow("I-V")]
        [DataRow("MMM+M")]
        public void Evaluate_OutOfRangeResult_ThrowsException(string input)
        {
            var calculator = new RomanCalculator();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calculator.Evaluate(input));
        }

        [TestMethod]
        [DataRow("I+")]
        [DataRow("I()")]
        public void Evaluate_InvalidExpression_ThrowsException(string input)
        {
            var calculator = new RomanCalculator();

            Assert.ThrowsException<SyntaxErrorException>(() => calculator.Evaluate(input));
        }


        [TestMethod]
        [DataRow("I", "I")]
        [DataRow("II+II", "IV")]
        [DataRow("II+V*(L+I)", "CCLVII")]
        [DataRow("(MMMDCCXXIV - MMCCXXIX) * II", "MMCMXC")]
        public void Evaluate_CorrectArguments_ReturnsCorrectResults(string input, string excpectedResult)
        {
            var calculator = new RomanCalculator();

            var actucalResult = calculator.Evaluate(input);

            Assert.AreEqual(excpectedResult, actucalResult);
        }
    }
}
