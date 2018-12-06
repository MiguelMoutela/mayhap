using Maybe.Samples.Dependencies;

namespace Maybe.Samples
{
    public class CustomerService
    {
        private readonly CustomerRepository _repository;
        private readonly CustomerConverter _customerConverter;

        public CustomerService(CustomerRepository repository, CustomerConverter customerConverter)
        {
            _repository = repository;
            _customerConverter = customerConverter;
        }

        public Maybe<CustomerDto> CreateCustomer(string name)
        {
            var customer = Customer.Create(name);
            var customerDto = Track.Continue(customer, () => _customerConverter.ToDto(customer));
            return Track.Continue(customerDto, () => _repository.Add(customerDto));
        }
    }
}