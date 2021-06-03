using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayList.DataAccess;
using PlayList.Models;

namespace PlayList.Service
{
    public class VoteService : BaseService, IVoteService
    {
        public VoteService(PlayListDbContext context) : base(context)
        {
        }

        public async Task<ResponseData<bool>> VoteToPlayInfoAsync(int id, Customer thisCustomer)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (id < 1)
            {
                result.Message = "The id must be ge 1";
            }
            else
            {
                // find the record
                var thisUserVotes = await this.PlayListDbContext.Votes.Where(c => c.CustomerId == thisCustomer.Id)
                 .ToListAsync();
                if (thisUserVotes.Count() > 2)
                {
                    result.Message = "You have at least 3 times to vote for plays";
                }
                else
                {
                    if (thisUserVotes.Exists(c => c.PlayId == id))
                    {
                        result.Message = "you have voted for this play";
                    }
                    else
                    {
                        // do work
                        var thisVote = new Vote
                        {
                            PlayId = id,
                            CustomerId = thisCustomer.Id,
                            CreateUser = thisCustomer.Id,
                            CreateTime = DateTime.Now,
                            Status = CommonStatusEnum.Common
                        };
                        // find the play
                        if (await this.PlayListDbContext.PlayInfos.Where(c => c.Id == id && c.Status == CommonStatusEnum.Common).AnyAsync())
                        {
                            var data = this.PlayListDbContext.Votes.AddAsync(thisVote);
                            if(data.IsNotNullOrEmpty())
                            {
                                result.Data=true;
                                this.PlayListDbContext.SaveChanges();
                            }
                            else
                            {
                                result.Message="It's failed to vote for the play";
                            }
                        }
                        else
                        {
                            result.Message="Not found the play";
                        }
                    }
                }

            }
            return result;
        }
    }
}