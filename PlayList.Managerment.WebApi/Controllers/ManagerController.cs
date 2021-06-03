using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayList.Models;
using PlayList.Service;

namespace PlayList.Managerment.WebApi.Controllers
{
    [Route("Manager")]
    [Authorize]
    public class ManagerController : ControllerBase
    {
        private IManagerService managerService;
        public ManagerController(IManagerService managerService)
        {
            this.managerService=managerService;
        }
        [HttpGet]
        public ResponseData<Manager> GetCurrentMangerInfo()
        {
            return new ResponseData<Manager> { Data = User.Claims.GetMangerInfoByClaims() };
        }
        [HttpGet]
        [Route("DeleteManager")]
        public async  Task<ResponseData<bool>> DeleteManagerByIdAsync(int id)
        {
            return await this.managerService.DeleteManagerByIdAsync(id,User.Claims.GetMangerInfoByClaims());
        }
        [HttpGet]
        [Route("GetAllManagers")]
        public async Task<ResponseList<Manager>> GetAllManagersAsync()
        {
            return await this.managerService.GetAllManagersAsync();
        }
        [HttpPost]
        [Route("CreateManagerByInfo")]
        public async Task<ResponseData<bool>> CreateManagerByInfoAsync(Manager query)
        {
            return await this.managerService.CreateManagerByInfoAsync(query,User.Claims.GetMangerInfoByClaims());
        }
        [HttpPost]
        [Route("UpdateManagerByInfo")]
        public async Task<ResponseData<bool>> UpdateManagerByInfoAsync(Manager query)
        {
            return await this.managerService.UpdateManagerByInfoAsync(query,User.Claims.GetMangerInfoByClaims());
        }
        [HttpGet]
        [Route("GetManagerById")]
        public async Task<ResponseData<Manager>> GetManagerByIdAsync(int id)
        {
            return await this.managerService.GeetManagerByIdAsync(id);
        }
    }
}