using System.Diagnostics.Contracts;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayList.Models;
using PlayList.Service;

namespace PlayList.Custonmer.WebApi.Controllers
{
    public class PlayInfoController : ControllerBase
    {
        public IPlayInfoService playInfoService { get; set; }
        public IVoteService voteService{get;set;}
        public PlayInfoController(IPlayInfoService playInfoService,IVoteService voteService)
        {
            this.playInfoService=playInfoService;
            this.voteService=voteService;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetPlayInfoById")]
        [HttpGet]
        public async Task<ResponseData<PlayInfo>> GetPlayInfoByIdAsync(int id)
        {
            return await this.playInfoService.GetPlayInfoByIdAsync(id);

        }
        [Route("GetAllPlayInfo")]
        [HttpGet]
        public async Task<ResponseList<PlayInfo>> GetAllPlayInfoAsync()
        {
            return await this.playInfoService.GetAllPlayInfosAsync();
        }
        [Route("VoteToPlayInfo")]
        [HttpGet]
        public async Task<ResponseData<bool>> VoteToPlayInfoAsync(int id)
        {
            return await this.voteService.VoteToPlayInfoAsync(id,User.Claims.GetMangerInfoByClaims());
        }


        public async Task<ResponseData<bool>> TestVotePlayAsync(Vote v)
        {
            return await this.voteService.VoteToPlayInfoAsync(v.PlayId,new Customer{Id=v.CustomerId});
        }
    }
}