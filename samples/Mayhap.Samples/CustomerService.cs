using System;
using System.Threading.Tasks;
using Mayhap.Samples.Dependencies;

namespace Mayhap.Samples
{
    public class CustomerService
    {
        private readonly CustomerRepository _repository;
        private readonly CustomerConverter _converter;

        public CustomerService(CustomerRepository repository, CustomerConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public Maybe<CustomerDto> CreateCustomer(string name)
        {
            var customer = Customer.Create(name);
            var customerDto = customer.Continue(c => _converter.ToDto(c));
            return customerDto.Continue(c => _repository.Add(c));
        }

        public Task<Maybe<CustomerDto>> CreateCustomerAsync(string name)
        {
            var customer = Customer.Create(name);
            var customerDto = customer.Continue(c => _converter.ToDto(c));
            return customerDto.Continue(async c => await _repository.AddAsync(c));
        }

        public Maybe<CustomerDto> Deposit(Guid id, decimal amount)
        {
            return _repository.Find(id.ToString("n"))
                .Continue(customerDto => _converter.ToEntity(customerDto), out var customer)
                .Continue(c => c.Deposit(amount))
                .Continue(_ => _converter.ToDto(customer))
                .Continue(customerDto => _repository.Update(customerDto));
        }
    }
}