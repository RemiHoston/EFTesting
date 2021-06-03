using System;
using System.Threading.Tasks;
using PlayList.Models;

namespace PlayList.Service
{
    public interface ICustomerService
    {
        Task<ResponseData<Customer>> GetCustomerByPhoneNumberAsync(string password);
        Task<ResponseData<bool>> CreateCustomerByInfoAsync(Customer query);
        Task<ResponseData<bool>> UpdateCustomerByInfoAsync(Customer query);
        Task<ResponseData<bool>> DeleteCustomerByIdAsync(int id);
        Task<ResponseData<Customer>> GetCustomerByIdAsync(int id);
        Task<ResponseList<Customer>> GetAllCustomerAsync();
    }
}
