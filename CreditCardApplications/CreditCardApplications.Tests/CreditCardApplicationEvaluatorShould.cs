using System;
using Xunit;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            //Arrange
            var sut = new CreditCardApplicationEvaluator(null);
            var application = new CreditCardApplication() { GrossAnnualIncome = 100_000 };

            //Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            var sut = new CreditCardApplicationEvaluator(null);

            var application = new CreditCardApplication { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        // other evaluator test conditions
    }
}
