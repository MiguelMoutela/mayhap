using System;
using Mayhap.Samples.Dependencies;
using FakeResponse = System.ValueTuple<System.Func<string, object[], bool>, System.Func<object[], Mayhap.Maybe<Mayhap.Samples.Dependencies.CustomerDto>>>;

namespace Mayhap.Samples
{
    internal class CustomerServiceContext
    {
        private readonly CustomerConverter _converter = new CustomerConverter();
        private CustomerRepository _repository;
        private readonly Lazy<CustomerService> _customerService; 

        public CustomerServiceContext()
        {
            _customerService = new Lazy<CustomerService>(
                () => new CustomerService(_repository ?? CustomerRepository.WithResponses(), _converter));
        }

        public CustomerServiceContext WithRepositoryResponding(params FakeResponse[] responses)
        {
            _repository = CustomerRepository.WithResponses(responses);
            return this;
        }

        public CustomerService Service => _customerService.Value;
    }
}