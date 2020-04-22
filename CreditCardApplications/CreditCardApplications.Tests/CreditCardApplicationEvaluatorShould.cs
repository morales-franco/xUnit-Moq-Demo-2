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
            /*
             * TODO: Specifyng DefaultValue strategy
             * With this approach, we write a explicit value for LicenseKey property
             * mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns(GetLicenseKeyExpiryString);
             * 
             * With the following approach, we delegate to Moq the default value.
             * In this case: x.ServiceInformation.License.LicenseKey = null porque LicenseKey is STRING.
             * Si fuea una clase/interface seria mockeada.
             * 
             * Si o si tenemos que usar uno de los dos approachs porque sino vamos a tener un NULL-REFERENCE-EXCEPTION
             */

            mockValidator.DefaultValue = DefaultValue.Mock;

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
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey); //RETURN NULL

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
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

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
            
            //EXPLICIT Mocking
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns(GetLicenseKeyExpiryString);

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

        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            //arrange
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //TODO: Hierarchies properties - manual way
            //var mockLicenseData = new Mock<ILicenseData>();
            //mockLicenseData.Setup(x => x.LicenseKey).Returns("EXPIRED");

            //var mockServiceInfo = new Mock<IServiceInformation>();
            //mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);

            //mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);

            //TODO: Property return a specified value
            //mockValidator.Setup(x => x.LicenseKey)
            //             .Returns("EXPIRED");

            //TODO: Hierarchies properties - automatically approach Level1.Level2.Property
            //TODO: Property return a specified value from function
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey)
                         .Returns(GetLicenseKeyExpiryString);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 42 };

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLicenseKeyExpiryString()
        {
            // E.g. read from vendor-supplied constants file
            return "EXPIRED";
        }

        [Fact]
        public void UseDetailedLookupForOlderApplications()
        {
            /*
             * By default MOCK properties DO NOT remember changes doing in the test process.
             * Ex: IFrequentFlyerNumberValidator has a enum properties, it is modified in the Evaluate Method
             * however when return from the method, the enum property do not keep the changed value.
             * This is de default behaviour but if we want to keep the changes to TRACKING DATA
             * we can two options:
             */

            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            //TODO: Tracking changes to mock properties values - track all properties
            //SetupAllProperties - order is important, must be before Setup
            mockValidator.SetupAllProperties();
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            //TODO: Tracking changes to mock properties values - track a specified property
            ///mockValidator.SetupProperty(x => x.ValidationMode);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 30 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //check value property - tracking changes of ValidationMode property 
            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }


    }
}
