using System.Threading.Tasks;
using PlayList.Models;

namespace PlayList.Service
{
    public interface IManagerService
    {
        Task<ResponseData<Manager>> GetManagerByPasswordAsync(string userName, string password);
        Task<ResponseData<bool>> DeleteManagerByIdAsync(int id,Manager currentUser);
        Task<ResponseList<Manager>> GetAllManagersAsync();
        Task<ResponseData<bool>> CreateManagerByInfoAsync(Manager query,Manager currentUser);
        Task<ResponseData<bool>> UpdateManagerByInfoAsync(Manager query,Manager currentUser);
        Task<ResponseData<Manager>> GeetManagerByIdAsync(int id);
    }
}