using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using ип_калькулятор;

namespace UnitTestProject1
{
    [TestClass]
    public class Калькулятор
    {
        [TestMethod]
        public void LoanCalculator_CalculateMonthlyPayment_ReturnsCorrectPayment()
        {
            // Arrange
            double loanAmount = 100000;
            double interestRate = 5;
            int loanTerm = 30;
            double expectedMonthlyPayment = 536.82;

            // Act
            var calculator = new Form1();
            double monthlyPayment = calculator.CalculateMonthlyPayment(loanAmount, interestRate, loanTerm);

            // Assert
            Assert.AreEqual(expectedMonthlyPayment, monthlyPayment, 0.01);
        }

        [TestMethod]
        public void Constructor_OpensConnection()
        {
            // Arrange and Act
            db db = new db();
            db.con.Open();
            // Assert
            Assert.IsTrue(db.con.State == ConnectionState.Open);
            db.con.Close();
        }
    }
}
