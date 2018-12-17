using System;
using Mayhap.Samples.RailwayOriented;
using FakeResponse = System.ValueTuple<System.Func<string, object[], bool>, System.Func<object[], Mayhap.Maybe<Mayhap.Samples.Shared.CustomerDto>>>;

namespace Mayhap.Samples
{
    internal class RoCustomerServiceContext
    {
        private readonly CustomerConverter _converter = new CustomerConverter();
        private CustomerRepository _repository;
        private readonly Lazy<CustomerService> _customerService; 

        public RoCustomerServiceContext()
        {
            _customerService = new Lazy<CustomerService>(
                () => new CustomerService(_repository ?? CustomerRepository.WithResponses(), _converter));
        }

        public RoCustomerServiceContext WithRepositoryResponding(params FakeResponse[] responses)
        {
            _repository = CustomerRepository.WithResponses(responses);
            return this;
        }

        public CustomerService Service => _customerService.Value;
    }
}