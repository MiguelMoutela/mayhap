using System;
using System.Threading.Tasks;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.Traditional
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

        public CustomerDto CreateCustomer(string name)
        {
            var customer = Customer.Create(name);
            if (customer == null)
            {
                return null;
            }

            var customerDto = _converter.ToDto(customer);
            return _repository.Add(customerDto);
        }

        public async Task<CustomerDto> CreateCustomerAsync(string name)
        {
            var customer = Customer.Create(name);
            if (customer == null)
            {
                return null;
            }

            var customerDto = _converter.ToDto(customer);
            return await _repository.AddAsync(customerDto);
        }

        public CustomerDto Deposit(Guid id, decimal amount)
        {
            var customerDto = _repository.Find(id.ToString("N"));

            if (customerDto == null)
            {
                return null;
            }

            var customer = _converter.ToEntity(customerDto);
            if (customer == null)
            {
                return null;
            }

            var deposit = customer.Deposit(amount);
            if (deposit == 0)
            {
                return null;
            }

            customerDto = _converter.ToDto(customer);
            customerDto = _repository.Update(customerDto);
            return customerDto;
        }
    }
}