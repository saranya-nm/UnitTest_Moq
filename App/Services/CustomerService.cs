using System;
using System.Text.RegularExpressions;

namespace App
{
    public class CustomerService
    {
        private const string VERY_IMP_CLI = "VeryImportantClient";
        private const string IMP_CLI = "ImportantClient";
        //private Customer _customer;

        public ICompanyRepository _companyRepository = new CompanyRepository();
        public ICustomerCreditService _customerCreditService = new CustomerCreditServiceClient();
        public ICustomerDataAccess _customerDataAccess = new CustomerDataAccess();
        
        public CustomerService(ICompanyRepository compRep, ICustomerCreditService customerCreditService,
            ICustomerDataAccess customerDataAccess)
        {
            _companyRepository = compRep;
            _customerCreditService = customerCreditService;
            _customerDataAccess = customerDataAccess;
        }
        public CustomerService()
        { }
        public bool AddCustomer(string firName, string surName, string eMail, DateTime dateOfBirth, int companyId)
        {
            if (IsValidCustomer(firName, surName, eMail, dateOfBirth))
            {

                var company = _companyRepository.GetById(companyId);
                var customer = new Customer(company, dateOfBirth, eMail, firName, surName);

                if (ComCreditLimitValid(customer))
                {
                    _customerDataAccess.AddCustomer(customer);
                    return true;
                }
            }
            return false;
        }

        private bool IsValidCustomer(string firName, string surName, string eMail, DateTime dateBirth)
        {
            if (string.IsNullOrEmpty(firName) || string.IsNullOrEmpty(surName))
                return false;

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(eMail).Success)
                return false;

            int age = DateTime.Now.Year - dateBirth.Year;
            if (DateTime.Now.DayOfYear < dateBirth.DayOfYear)
                age--;

            if (age < 21)
                return false;


            return true;
        }

        private bool ComCreditLimitValid(Customer customer)
        {
            if (customer != null && customer.Company != null)
            {
                if (customer.Company.Name != VERY_IMP_CLI)
                {
                    customer.HasCreditLimit = true;

                    var creditLimit = _customerCreditService.GetCreditLimit(customer.Firstname,
                                        customer.Surname, customer.DateOfBirth).Result;

                    customer.CreditLimit = (customer.Company.Name == IMP_CLI ? creditLimit * 2 : creditLimit);

                }
                else
                    customer.HasCreditLimit = false;

                if (customer.HasCreditLimit && customer.CreditLimit < 500)

                    return false;

                return true;
            }
            else
                return false;
        }
    }
}
