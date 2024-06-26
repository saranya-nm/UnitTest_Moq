﻿using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App
{
    public interface ICustomerCreditService
    {
         Task<int>  GetCreditLimit(string firstname, string surname, DateTime dateOfBirth);
    }

    public class CustomerCreditServiceClient : ICustomerCreditService, IDisposable
    {
        private readonly HttpClient _apiClient;

        public CustomerCreditServiceClient()
        {
            _apiClient = new HttpClient();
        }

        public async Task<int> GetCreditLimit(string firstname, string surname, DateTime dateOfBirth)
        {
            return JObject.Parse(await (await _apiClient.PostAsync(ConfigurationManager.AppSettings["getCreditLimitEndpoint"],
                new StringContent("{\"fn\":\"" + firstname + "\",\"sn\":\"" + surname + "\",\"dob\":\"" + dateOfBirth + "\"}")))
                .Content.ReadAsStringAsync()).GetValue("creditLimit").Value<int>();
        }

        public void Dispose()
        {
            _apiClient.Dispose();
        }
    }
}