using System;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.Traditional
{
    public class CustomerConverter
    {
        public Customer ToEntity(CustomerDto customerDto)
            => Guid.TryParse(customerDto.Id, out var id)
                ? Customer.Load(id, customerDto.Name, customerDto.AccountBalance)
                : null;

        public CustomerDto ToDto(Customer customer)
            => new CustomerDto
            {
                Id = customer.Id.ToString("N"),
                AccountBalance = customer.AccountBalance,
                Name = customer.Name
            };
    }
}