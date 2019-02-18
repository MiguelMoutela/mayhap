using System;
using System.Threading.Tasks;
using Mayhap.Samples.Traditional;
using Mayhap.Samples.Shared;
using Moq;

namespace Mayhap.Samples
{
    internal class TraditionalCustomerServiceContext
    {
        private readonly CustomerConverter _converter = new CustomerConverter();
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Lazy<CustomerService> _customerService;

        public TraditionalCustomerServiceContext()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            SetupDefaults(_repositoryMock);
            _customerService = new Lazy<CustomerService>(
                () => new CustomerService(_repositoryMock.Object, _converter));
        }

        public CustomerService Service => _customerService.Value;

        public TraditionalCustomerServiceContext WithFindAction(Func<string, CustomerDto> action)
        {
            _repositoryMock
                .Setup(r => r.Find(It.IsAny<string>()))
                .Returns(action);
            return this;
        }

        public TraditionalCustomerServiceContext WithUpdateAction(Func<CustomerDto, CustomerDto> action)
        {
            _repositoryMock
                .Setup(r => r.Update(It.IsAny<CustomerDto>()))
                .Returns(action);
            return this;
        }

        public TraditionalCustomerServiceContext WithAddAction(Func<CustomerDto, CustomerDto> action)
        {
            _repositoryMock.Setup(r => r.Add(It.IsAny<CustomerDto>()))
                .Returns(action);
            return this;
        }

        public TraditionalCustomerServiceContext WithAddAsyncAction(Func<CustomerDto, Task<CustomerDto>> action)
        {
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<CustomerDto>()))
                .Returns(action);
            return this;
        }

        private static void SetupDefaults(Mock<ICustomerRepository> repositoryMock)
        {
            repositoryMock.Setup(r => r.Add(It.IsAny<CustomerDto>()))
                .Returns((CustomerDto) null);
            repositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((CustomerDto) null);
            repositoryMock.Setup(r => r.Update(It.IsAny<CustomerDto>()))
                .Returns((CustomerDto) null);
            repositoryMock.Setup(r => r.Delete(It.IsAny<CustomerDto>()))
                .Returns((CustomerDto) null);
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<CustomerDto>()))
                .ReturnsAsync((CustomerDto) null);
            repositoryMock.Setup(r => r.FindAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerDto) null);
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<CustomerDto>()))
                .ReturnsAsync((CustomerDto) null);
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<CustomerDto>()))
                .ReturnsAsync((CustomerDto) null);
        }

    }
}