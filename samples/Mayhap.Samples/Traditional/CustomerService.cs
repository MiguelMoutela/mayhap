using System;
using System.Threading.Tasks;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.Traditional
{
    public class CustomerService
    {
        public CustomerDto CreateCustomer(string name)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerDto> CreateCustomerAsync(string name)
        {
            throw new NotImplementedException();
        }

        public CustomerDto Deposit(Guid id, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}