using System.Threading.Tasks;
using PlayList.Models;

namespace PlayList.Service
{
    public interface IPlayInfoService
    {
        Task<ResponseData<bool>> CreatePlayInfoByInfoAsync(PlayInfo query, Manager manager);
        Task<ResponseData<bool>> UpdatePlayInfoByInfoAsync(PlayInfo query, Manager manager);
        Task<ResponseData<bool>> DeletePlayInfoByIdAsync(int id, Manager manager);
        Task<ResponseData<PlayInfo>> GetPlayInfoByIdAsync(int id);
        Task<ResponseList<PlayInfo>> GetAllPlayInfosAsync();
        Task<ResponseList<Customer>> DrawALotteryAsync();
    }
}