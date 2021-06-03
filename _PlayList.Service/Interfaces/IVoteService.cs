using System.Threading.Tasks;
using PlayList.Models;

namespace PlayList.Service
{
    public interface IVoteService
    {
        Task<ResponseData<bool>> VoteToPlayInfoAsync(int id, Customer customer);
    }
}