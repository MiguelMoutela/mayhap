using System.Threading.Tasks;
using Mayhap.Maybe;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.RailwayOriented
{
    public interface ICustomerRepository
    {
        Maybe<CustomerDto> Add(CustomerDto customer);
        Task<Maybe<CustomerDto>> AddAsync(CustomerDto customer);
        Maybe<CustomerDto> Update(CustomerDto customer);
        Task<Maybe<CustomerDto>> UpdateAsync(CustomerDto customer);
        Maybe<CustomerDto> Delete(CustomerDto customer);
        Task<Maybe<CustomerDto>> DeleteAsync(CustomerDto customer);
        Maybe<CustomerDto> Find(string id);
        Task<Maybe<CustomerDto>> FindAsync(string id);
    }
}