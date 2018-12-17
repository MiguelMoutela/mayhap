using System;
using System.Threading.Tasks;
using Mayhap.Samples.Shared;
using Xunit;
using Xunit.Abstractions;

namespace Mayhap.Samples
{
    public class SampleRunner
    {
        public SampleRunner(ITestOutputHelper output)
        {
            Output = output;
        }

        private ITestOutputHelper Output { get; }

        [Fact]
        public void CustomerCreatedSuccessfully()
        {
            var context = RoContext()
                .WithAddAction(c => c.Success());

            var customer = context.Service.CreateCustomer("John Doe");
            Output.WriteLine(customer.ToString());
        }

        [Fact]
        public async Task CustomerCreatedAsynchronousSuccessfully()
        {
            var context = RoContext()
                .WithAddAsyncAction(c => Task.FromResult(c.Success()));

            var customer = await context.Service.CreateCustomerAsync("John Doe");
            Output.WriteLine(customer.ToString());
        }

        [Fact]
        public void DepositServiceUnavailable()
        {
            var deposit = RoContext().Service.Deposit(Guid.NewGuid(), 100.0m);
            Output.WriteLine(deposit.ToString());
        }

        [Fact]
        public void DepositSuccessful()
        {
            var context = RoContext()
                .WithFindAction(id => new CustomerDto { Id = id, Name = "John Doe", AccountBalance = 10.0m }.Success())
                .WithUpdateAction(c => c.Success());

            var deposit = context.Service.Deposit(Guid.NewGuid(), 100.0m);
            Output.WriteLine(deposit.ToString());
        }

        private RoCustomerServiceContext RoContext() => new RoCustomerServiceContext();
        private TraditionalCustomerServiceContext TraditionalContext() => new TraditionalCustomerServiceContext();
    }
}