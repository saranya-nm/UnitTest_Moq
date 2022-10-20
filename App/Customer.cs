using System;

namespace App
{
    public class Customer
    {
        public Customer(Company company,DateTime dOb,string eMail,string firName,string surName)
        {
            Company = company;
            DateOfBirth = dOb;
            EmailAddress = eMail;
            Firstname = firName;
            Surname = surName;
        }

        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public bool HasCreditLimit { get; set; }

        public int CreditLimit { get; set; }

        public Company Company { get; set; }

    }
}