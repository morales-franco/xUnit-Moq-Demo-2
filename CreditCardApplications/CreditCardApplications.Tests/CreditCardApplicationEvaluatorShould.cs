using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            //Arrange
            /*
             * TODO: Mocking IFrequentFlyerNumberValidator injecting it by constructor.
             * IFrequentFlyerNumberValidator Mock will return default values in methods, for example
             * if you have a method that return a boolean, this method always will return FALSE (with the default setup)
             */
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication() { GrossAnnualIncome = 100_000 };

            //Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = 
                new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 19 };

            //Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            /*
             * TODO: Mock result Method - setup the mockValidator for the IsValid Method return true!
             * IsValid method will return TRUE when the parameter is "XXX1", if the FrequentFlyerNumber is
             * different to "XXX1", the IsValid Method will return FALSE
             */
            mockValidator.Setup(x => x.IsValid("XXX1")).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "XXX1" //This value map with setup mock configuration
            };

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        //TODO: Refactoring 1 - Using It class to Avoid Hardcoding parameters
        [Fact]
        public void DeclineLowIncomeApplications_Refactor1()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            //TODO: Use It class to avoid hardcoding parameters. In this case we says: if you receive any string, return TRUE.
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //TODO: Setup mock object - configuration of method result.

            //mockValidator.Setup(x => x.IsValid(It.Is<string>(frequentFlyerNumber => frequentFlyerNumber.StartsWith("X"))))
            //             .Returns(true);

            //mockValidator.Setup(x => x.IsValid(It.IsIn("X","Y","Z")))
            //             .Returns(true);

            //mockValidator.Setup(x => x.IsValid(It.IsInRange("A", "Z", Moq.Range.Inclusive)))
            //             .Returns(true);

            //mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]", 
            //                    System.Text.RegularExpressions.RegexOptions.None)))
            //             .Returns(true);


            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "x"
            };

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            /*
             * Mock Behaviours
             * MockBehavior.Loose: (default) if we don't setup any methods, never thow exceptions, return default values of returning type.
             * MockBehavior.Strict: if we don't setup a method & then, this method is called --> throw an exception.
             */
            /*
             * TODO: Mocking with MockBehavior.Strict
             */
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            //Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplicationsOutDemo()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            /*
             * TODO: Mocking method with out parameters
             */
            bool isValid = true;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };

            //act
            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }


    }
}
