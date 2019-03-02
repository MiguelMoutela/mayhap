using System;
using System.Threading.Tasks;
using Mayhap.Maybe;
using Mayhap.Samples.RailwayOriented;
using Mayhap.Samples.Shared;
using Moq;

namespace Mayhap.Samples
{
    internal class RoCustomerServiceContext
    {
        private readonly CustomerConverter _converter = new CustomerConverter();
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Lazy<CustomerService> _customerService; 

        public RoCustomerServiceContext()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            SetupDefaults(_repositoryMock);
            _customerService = new Lazy<CustomerService>(
                () => new CustomerService(_repositoryMock.Object, _converter));
        }

        public CustomerService Service => _customerService.Value;

        public RoCustomerServiceContext WithFindAction(Func<string, Maybe<CustomerDto>> action)
        {
            _repositoryMock
                .Setup(r => r.Find(It.IsAny<string>()))
                .Returns(action);
            return this;
        }

        public RoCustomerServiceContext WithUpdateAction(Func<CustomerDto, Maybe<CustomerDto>> action)
        {
            _repositoryMock
                .Setup(r => r.Update(It.IsAny<CustomerDto>()))
                .Returns(action);
            return this;
        }

        public RoCustomerServiceContext WithAddAction(Func<CustomerDto, Maybe<CustomerDto>> action)
        {
            _repositoryMock.Setup(r => r.Add(It.IsAny<CustomerDto>()))
                .Returns(action);
            return this;
        }

        public RoCustomerServiceContext WithAddAsyncAction(Func<CustomerDto, Task<Maybe<CustomerDto>>> action)
        {
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<CustomerDto>()))
                .Returns(action);
            return this;
        }

        private static void SetupDefaults(Mock<ICustomerRepository> repositoryMock)
        {
            var defaultResponse = "SERVICE_UNAVAILABLE".Fail<CustomerDto>();
            repositoryMock.Setup(r => r.Add(It.IsAny<CustomerDto>()))
                .Returns(defaultResponse);
            repositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns(defaultResponse);
            repositoryMock.Setup(r => r.Update(It.IsAny<CustomerDto>()))
                .Returns(defaultResponse);
            repositoryMock.Setup(r => r.Delete(It.IsAny<CustomerDto>()))
                .Returns(defaultResponse);
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<CustomerDto>()))
                .ReturnsAsync(defaultResponse);
            repositoryMock.Setup(r => r.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(defaultResponse);
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<CustomerDto>()))
                .ReturnsAsync(defaultResponse);
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<CustomerDto>()))
                .ReturnsAsync(defaultResponse);
        }
    }
}