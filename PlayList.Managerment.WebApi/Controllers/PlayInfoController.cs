using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayList.Models;
using PlayList.Service;

namespace PlayList.Managerment.WebApi.Controllers
{
    [Authorize]
    [Route("PlayInfo")]
    public class MPlayInfoController : ControllerBase
    {
        private IPlayInfoService playInfoService { get; set; }
        public MPlayInfoController(IPlayInfoService playInfoService)
        {
            this.playInfoService=playInfoService;
        }
        [HttpPost]
        [Route("CreatePlayInfoByInfo")]
        public async Task<ResponseData<bool>> CreatePlayInfoByInfoAsync(PlayInfo query)
        {
            return await this.playInfoService.CreatePlayInfoByInfoAsync(query,User.Claims.GetMangerInfoByClaims());
        }
        [Route("UpdatePlayInfoByInfo")]
        [HttpPost]
        public async Task<ResponseData<bool>> UpdatePlayInfoByInfoAsync(PlayInfo query)
        {
            return await this.playInfoService.UpdatePlayInfoByInfoAsync(query,User.Claims.GetMangerInfoByClaims());
        }
        [Route("DeletePlayInfoById")]
        [HttpGet]
        public async Task<ResponseData<bool>> DeletePlayInfoByIdAsync(int id)
        {
            return await this.playInfoService.DeletePlayInfoByIdAsync(id,User.Claims.GetMangerInfoByClaims());
        }
        [Route("GetPlayInfoById")]
        [HttpGet]
        public async Task<ResponseData<PlayInfo>> GetPlayInfoByIdAsync(int id)
        {
            return await this.playInfoService.GetPlayInfoByIdAsync(id);
        }
        [HttpGet]
        [Route("GetAllPlayInfos")]
        public async Task<ResponseList<PlayInfo>> GetAllPlayInfosAsync()
        {
            return await this.playInfoService.GetAllPlayInfosAsync();
        }
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DrawALottery")]
        public async Task<ResponseList<Customer>> DrawALotteryAsync()
        {
            return await this.playInfoService.DrawALotteryAsync();
        }
    }
}