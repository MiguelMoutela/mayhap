using System;
using System.Linq;
using System.Threading.Tasks;
using Mayhap.Samples.RailwayOriented;
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
            // todo consider using moq
            var context = RoContext()
                .WithRepositoryResponding(
                    (
                        (op, _) => op.Equals(nameof(CustomerRepository.Add), StringComparison.InvariantCulture),
                        args => (args.First() as CustomerDto).Success()
                    ));

            var customer = context.Service.CreateCustomer("John Doe");
            Output.WriteLine(customer.ToString());
        }

        [Fact]
        public async Task CustomerCreatedAsynchronousSuccessfully()
        {
            var context = RoContext()
                .WithRepositoryResponding(
                    (
                        (op, _) => op.Equals(nameof(CustomerRepository.AddAsync), StringComparison.InvariantCulture),
                        args => (args.First() as CustomerDto).Success()
                    ));

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
                .WithRepositoryResponding(
                    (
                        (op, _) => nameof(CustomerRepository.Update) == op,
                        args => (args.First() as CustomerDto).Success()
                    ),
                    (
                        (op, _) => nameof(CustomerRepository.Find) == op,
                        args => new CustomerDto { Id = args.First().ToString(), Name = "John Doe", AccountBalance = 10.0m }.Success()
                    ));

            var deposit = context.Service.Deposit(Guid.NewGuid(), 100.0m);
            Output.WriteLine(deposit.ToString());
        }

        private RoCustomerServiceContext RoContext() => new RoCustomerServiceContext();
        private TraditionalCustomerServiceContext TraditionalContext() => new TraditionalCustomerServiceContext();
    }

    internal class TraditionalCustomerServiceContext
    {
    }
}