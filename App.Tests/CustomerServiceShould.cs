using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace App.Tests
{
    [TestFixture]
    public sealed class CustomerServiceShould
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new Mock<ICompanyRepository>();
        private readonly Mock<ICustomerCreditService> _custCredMock = new Mock<ICustomerCreditService>();
        private readonly Mock<ICustomerDataAccess> _custMockData = new Mock<ICustomerDataAccess>();
        private CustomerService _cusService;

        [SetUp]
        public void SetUp()
        {
            _cusService = new CustomerService(_companyRepositoryMock.Object,
                _custCredMock.Object,
                _custMockData.Object);
        }
        /// <summary>
        /// Positive Test For Important Client
        /// </summary>
        [Test]
        public void AddCustomerTest_ImpPositive()
        {
            //Arrange
            Company company = new Company { Id = 4, 
                                            Name = "ImportantClient", 
                                            Classification = Classification.Gold };

            Customer customer = new Customer(company, 
                                            new DateTime(1980, 3, 27), 
                                            "Joe@adomain.co.in", 
                                            "Joe", 
                                            "Bloggs");

            var credLimit = 260;

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
           
            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));

            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();

            //Act
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, 
                customer.DateOfBirth, company.Id);
            //Assert
            Assert.True(result);
        }


        /// <summary>
        /// Negative Test For Important Client
        /// </summary>
        [Test]
        public void AddCustomerTest_ImpNegative()
        {
            Company company = new Company
            {
                Id = 4,
                Name = "ImportantClient",
                Classification = Classification.Gold
            };

            Customer customer = new Customer(company,
                                            new DateTime(1980, 3, 27),
                                            "Joe@adomain.co.in",
                                            "Joe",
                                            "Bloggs");

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
            var credLimit = 5; //Low Credit Limit

            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));
            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, 
                customer.DateOfBirth, company.Id);

            Assert.False(result);
        }

        /// <summary>
        /// Negative Test For Empty Name and invalid Email Id
        /// </summary>
        [Test]
        public void AddCustomerTest_NameNegative()
        {
            Company company = new Company
            {
                Id = 4,
                Name = "ImportantClient",
                Classification = Classification.Gold
            };

            Customer customer = new Customer(company,
                                            new DateTime(1980, 3, 27),
                                            "adomain.co.in",
                                            "",
                                            "");

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
            var credLimit = 5;
            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));
            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, 
                customer.DateOfBirth, company.Id);

            Assert.False(result);
        }
        /// <summary>
        /// Negative Test For invalid age
        /// </summary>
        [Test]
        public void AddCustomerTest_AgeNegative()
        {
            Company company = new Company
            {
                Id = 4,
                Name = "ImportantClient",
                Classification = Classification.Gold
            };

            Customer customer = new Customer(company,
                                            new DateTime(2005, 3, 27),
                                            "adomain.co.in",
                                            "",
                                            "");

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
            var credLimit = 5;
            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));
            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, 
                customer.DateOfBirth, company.Id);

            Assert.False(result);
        }

        /// <summary>
        /// Negative Test For invalid age
        /// </summary>
        [Test]
        public void AddCustomerTest_VeryImpPos()
        {
            //Test for VeryImportant Client. No credit limit set.
            Company company = new Company
            {
                Id = 4,
                Name = "VeryImportantClient",
                Classification = Classification.Gold
            };

            Customer customer = new Customer(company,
                                            new DateTime(1991, 3, 27),
                                            "Joe@adomain.co.in",
                                            "Joe",
                                            "Pitt");

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
            var credLimit = 5;
            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));
            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();

            //Act
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, 
                customer.DateOfBirth, company.Id);
            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Positive Test For Normal Client
        /// </summary>
        [Test]
        public void AddCustomerTest_ClientPos()
        {
            Company company = new Company
            {
                Id = 4,
                Name = "Client",
                Classification = Classification.Gold
            };

            Customer customer = new Customer(company,
                                            new DateTime(1991, 3, 27),
                                            "Joe@adomain.co.in",
                                            "Joe",
                                            "Pitt");

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
            var credLimit = 560;
            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));
            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, 
                customer.EmailAddress, customer.DateOfBirth, company.Id);

            Assert.True(result);
        }

        /// <summary>
        /// Negative Test For Normal Client
        /// </summary>
        [Test]
        public void AddCustomerTest_ClientNeg()
        {
            Company company = new Company
            {
                Id = 4,
                Name = "Client",
                Classification = Classification.Gold
            };

            Customer customer = new Customer(company,
                                            new DateTime(1991, 3, 27),
                                            "Joe@adomain.co.in",
                                            "Joe",
                                            "Pitt");

            _companyRepositoryMock.Setup(x => x.GetById(company.Id)).Returns(company);
            var credLimit = 260;
            _custCredMock.Setup(a => a.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
                .Returns(Task.FromResult(credLimit));
            _custMockData.Setup(at => at.AddCustomer(customer)).Verifiable();
            var result = _cusService.AddCustomer(customer.Firstname, customer.Surname, 
                customer.EmailAddress, customer.DateOfBirth, company.Id);

            Assert.False(result);
        }
    }
}
