using System.Threading.Tasks;
using Mayhap.Samples.Shared;

namespace Mayhap.Samples.Traditional
{
    public interface ICustomerRepository
    {
        CustomerDto Add(CustomerDto customer);
        Task<CustomerDto> AddAsync(CustomerDto customer);
        CustomerDto Update(CustomerDto customer);
        Task<CustomerDto> UpdateAsync(CustomerDto customer);
        CustomerDto Delete(CustomerDto customer);
        Task<CustomerDto> DeleteAsync(CustomerDto customer);
        CustomerDto Find(string id);
        Task<CustomerDto> FindAsync(string id);
    }
}