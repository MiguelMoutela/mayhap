using System;
using Maybe.Samples.Dependencies;
using FakeResponse = System.ValueTuple<System.Func<string, Maybe.Samples.Dependencies.CustomerDto, bool>, System.Func<Maybe.Samples.Dependencies.CustomerDto, Maybe.Maybe<Maybe.Samples.Dependencies.CustomerDto>>>;

namespace Maybe.Samples
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