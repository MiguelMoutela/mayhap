using System;
using System.Linq;
using Maybe.Samples.Dependencies;
using Xunit;
using Xunit.Abstractions;
using FakeResponse = System.ValueTuple<System.Func<string, Maybe.Samples.Dependencies.CustomerDto, bool>, System.Func<Maybe.Samples.Dependencies.CustomerDto, Maybe.Maybe<Maybe.Samples.Dependencies.CustomerDto>>>;

namespace Maybe.Samples
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
            var context = Context()
                .WithRepositoryResponding(
                    (
                        (op, _) => op.Equals(nameof(CustomerRepository.Add), StringComparison.InvariantCulture),
                        args => (args.First() as CustomerDto).Success()
                    ));

            var customer = context.Service.CreateCustomer("John Doe");
            Output.WriteLine(customer.ToString());
        }

        [Fact]
        public void DepositServiceUnavailable()
        {
            var deposit = Context().Service.Deposit(Guid.NewGuid(), 100.0m);
            Output.WriteLine(deposit.ToString());
        }

        private CustomerServiceContext Context()
        {
            return new CustomerServiceContext();
        }
    }
}