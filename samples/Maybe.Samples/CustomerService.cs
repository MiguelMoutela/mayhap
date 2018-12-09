using System;
using System.Threading.Tasks;
using Maybe.Samples.Dependencies;

namespace Maybe.Samples
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
            var customerDto = Track.Continue(customer, () => _converter.ToDto(customer));
            return Track.Continue(customerDto, () => _repository.Add(customerDto));
        }

        public Task<Maybe<CustomerDto>> CreateCustomerAsync(string name)
        {
            var customer = Customer.Create(name);
            var customerDto = Track.Continue(customer, () => _converter.ToDto(customer));
            return Track.Continue(customerDto, async () => await _repository.AddAsync(customerDto));
        }

        public Maybe<CustomerDto> Deposit(Guid id, decimal amount)
        {
            var customerDto = _repository.Find(id.ToString("n"));
            var customer = Track.Continue(customerDto, () => _converter.ToEntity(customerDto));
            var accountBalance = Track.Continue(customer, () => customer.Data.Deposit(amount));
            var updatedCustomerDto = Track.Continue(accountBalance, () => _converter.ToDto(customer));
            return Track.Continue(updatedCustomerDto, () => _repository.Update(updatedCustomerDto));
        }
    }
}