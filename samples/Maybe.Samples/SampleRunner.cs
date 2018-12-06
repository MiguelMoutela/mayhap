using System;
using Maybe.Samples.Dependencies;
using Xunit;
using Xunit.Abstractions;
using FakeResponse = System.ValueTuple<System.Func<string, Maybe.Samples.Dependencies.CustomerDto, bool>, System.Func<Maybe.Samples.Dependencies.CustomerDto, Maybe.Maybe<Maybe.Samples.Dependencies.CustomerDto>>>;

namespace Maybe.Samples
{
    public class SampleRunner
    {
        private readonly ITestOutputHelper _output;

        public SampleRunner(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CustomerCreatedSuccessfully()
        {
            var context = Context()
                .WithRepositoryResponding(
                    (
                        (op, _) => op.Equals(nameof(CustomerRepository.Add), StringComparison.InvariantCulture),
                        c => c.Success()
                    ));

            var customer = context.Service.CreateCustomer("John Doe");
            _output.WriteLine(customer.ToString());
        }

        private CustomerServiceContext Context()
        {
            return new CustomerServiceContext();
        }
    }
}