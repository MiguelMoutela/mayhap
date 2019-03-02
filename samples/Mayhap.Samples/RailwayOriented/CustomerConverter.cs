using System;
using Mayhap.Maybe;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.RailwayOriented
{
    public class CustomerConverter
    {
        public Maybe<Customer> ToEntity(CustomerDto customerDto) 
            => Guid.TryParse(customerDto.Id, out var id)
                ? Customer.Load(id, customerDto.Name, customerDto.AccountBalance)
                : $"INVALID.{nameof(id)}".Fail<Customer>();

        public Maybe<CustomerDto> ToDto(Customer customer)
            => new CustomerDto
            {
                Id = customer.Id.ToString("N"),
                AccountBalance = customer.AccountBalance,
                Name = customer.Name
            }.Success();
    }
}