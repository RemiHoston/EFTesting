using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayList.Models;
using PlayList.Service;

namespace PlayList.Custonmer.WebApi.Controllers
{
    [Authorize]
    [Route("Customer")]
    public class CustomerController : ControllerBase
    {
        private ICustomerService customerService { get; set; }
        public CustomerController(ICustomerService customerService)
        {
            this.customerService=customerService;
        }
        [Route("CreateCustomerByInfo")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseData<bool>> CreateCustomerByInfoAsync(Customer query)
        {
            return await this.customerService.CreateCustomerByInfoAsync(query);
        }
        [HttpPost]
        [Route("UpdateCustomerByInfo")]
        public async Task<ResponseData<bool>> UpdateCustomerByInfoAsync(Customer query)
        {
            return await this.customerService.UpdateCustomerByInfoAsync(query);
        }
        [HttpGet]
        [Route("DeleteCustomerById")]
        public async Task<ResponseData<bool>> DeleteCustomerByIdAsync(int id)
        {
            return await this.customerService.DeleteCustomerByIdAsync(id);
        }
        [HttpGet]
        [Route("GetCustomerById")]
        public async Task<ResponseData<Customer>> GetCustomerByIdAsync(int id)
        {
            return await this.customerService.GetCustomerByIdAsync(id);
        }
        [HttpGet]
        [Route("GetAllCustomer")]
        public async Task<ResponseList<Customer>> GetAllCustomerAsync()
        {
            return await this.customerService.GetAllCustomerAsync();
        }
    }
}