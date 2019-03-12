using System;
using System.Threading.Tasks;
using Mayhap.Maybe;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.RailwayOriented
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly CustomerConverter _converter;

        public CustomerService(ICustomerRepository repository, CustomerConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public Maybe<CustomerDto> CreateCustomer(string name)
        {
            var customer = Customer.Create(name);
            var customerDto = customer.Map(c => _converter.ToDto(c));
            return customerDto.Map(c => _repository.Add(c));
        }

        public Task<Maybe<CustomerDto>> CreateCustomerAsync(string name)
        {
            var customer = Customer.Create(name);
            var customerDto = customer.Map(c => _converter.ToDto(c));
            return customerDto.Map(async c => await _repository.AddAsync(c));
        }

        public Maybe<CustomerDto> Deposit(Guid id, decimal amount)
        {
            return _repository.Find(id.ToString("n"))
                .Map(customerDto => _converter.ToEntity(customerDto), out var customer)
                .Map(c => c.Deposit(amount))
                .Map(_ => _converter.ToDto(customer))
                .Map(customerDto => _repository.Update(customerDto));
        }
    }
}