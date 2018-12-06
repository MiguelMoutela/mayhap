using System.Collections.Generic;
using System.Linq;
using FakeResponse = System.ValueTuple<System.Func<string, Maybe.Samples.Dependencies.CustomerDto, bool>, System.Func<Maybe.Samples.Dependencies.CustomerDto, Maybe.Maybe<Maybe.Samples.Dependencies.CustomerDto>>>;

namespace Maybe.Samples.Dependencies
{
    public class CustomerRepository
    {
        private readonly IList<FakeResponse> _responses;

        private CustomerRepository(IList<FakeResponse> responses = null)
        {
            _responses = responses?.Concat(new [] { new FakeResponse((_, __) => true, _ => "SERVICE_UNAVAILABLE".Fail<CustomerDto>()) }).ToList();
        }

        public Maybe<CustomerDto> Add(CustomerDto customer)
        {
            return Respond(nameof(Add), customer);
        }

        public Maybe<CustomerDto> Update(CustomerDto customer)
        {
            return Respond(nameof(Update), customer);
        }

        public Maybe Delete(CustomerDto customer)
        {
            return Respond(nameof(Delete), customer);
        }

        public static CustomerRepository WithResponses(params FakeResponse[] responses)
        {
            return new CustomerRepository(responses);
        }

        private Maybe<CustomerDto> Respond(string operationName, CustomerDto customer)
        {
            return _responses.First(r => r.Item1(operationName, customer)).Item2(customer);
        }
    }
}