using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayList.DataAccess;
using PlayList.Models;

namespace PlayList.Service
{
    public class PlayInfoService : BaseService, IPlayInfoService
    {
        public PlayInfoService(PlayListDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ResponseData<bool>> CreatePlayInfoByInfoAsync(PlayInfo query, Manager manager)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (query.IsNullOrEmpty())
            {
                result.Message = "model must be not empty";
            }
            else if (query.PlayName.IsNullOrEmpty())
            {
                result.Message = "PlayName must be not empty";
            }
            else
            {
                if (await this.PlayListDbContext.PlayInfos.Where(c => c.PlayName == query.PlayName).AnyAsync())
                {
                    result.Message = "The playName has been exist";
                }
                else
                {
                    query.Status = CommonStatusEnum.Common;
                    query.CreateTime = DateTime.Now;
                    query.CreateUser = manager.Id;
                    query.UpdateTime = DateTime.Now;
                    query.UpdateUser = manager.Id;
                    var data = await this.PlayListDbContext.PlayInfos.AddAsync(query);
                    if (data.IsNotNullOrEmpty())
                    {
                        result.Data = true;
                        this.PlayListDbContext.SaveChanges();
                    }
                    else
                    {
                        result.Message = "Failed to create data";
                    }
                }
            }
            return result;
        }

        public async Task<ResponseData<bool>> DeletePlayInfoByIdAsync(int id, Manager manager)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (id < 1)
            {
                result.Message = "Id must be ge 1";
            }
            else
            {
                if (await this.PlayListDbContext.PlayInfos.Where(c => c.Id == id).AnyAsync())
                {
                    var thisPlayInfo = await this.PlayListDbContext.PlayInfos.FindAsync(id);
                    thisPlayInfo.Status = CommonStatusEnum.Deleted;
                    thisPlayInfo.UpdateUser = manager.Id;
                    thisPlayInfo.UpdateTime = DateTime.Now;
                    var data = this.PlayListDbContext.Update(thisPlayInfo);
                    if (data.IsNotNullOrEmpty())
                    {
                        result.Data = true;
                        this.PlayListDbContext.SaveChanges();
                    }
                    else
                    {
                        result.Message = "Failed to delete";
                    }

                }
                else
                {
                    result.Message = "Not Found";
                }
            }
            return result;
        }

        public async Task<ResponseList<Customer>> DrawALotteryAsync()
        {
            // the first get top 3 plays
            ResponseList<Customer> result = new ResponseList<Customer>();
            var allPlays = await this.GetAllPlayInfosAsync();
            var top3Plays = allPlays.Data.Where(c=>c.VoteCount>0).OrderByDescending(c=>c.VoteCount).Take(3);
            if (top3Plays.IsNotNullOrEmpty())
            {
                var playIds=top3Plays.Select(c => c.Id).ToList();
                // the second get customers who vote the 3 plays.
                var top3Customer =
                  await (from c in this.PlayListDbContext.Customers
                         join v in this.PlayListDbContext.Votes on c.Id equals v.CustomerId
                         where 
                         playIds.Contains(v.PlayId)
                         //&& playIds.Exists(c=>c==v.PlayId)
                         && c.Status == CommonStatusEnum.Common
                         orderby c.CreateTime ascending
                         select c).Distinct().Take(3).ToListAsync();
                if (top3Customer.IsListNotNull())
                {
                    result.Data = top3Customer;
                }
                else
                {
                    result.Message = "Has no luky person";
                }
            }
            else
            {
                result.Message = "This playinfo list is empty";
            } 
            return result;
        }

        public async Task<ResponseList<PlayInfo>> GetAllPlayInfosAsync()
        {
            ResponseList<PlayInfo> result = new ResponseList<PlayInfo>();
            var data=await this.PlayListDbContext.PlayInfos.Where(c=>c.Status!=CommonStatusEnum.Deleted).ToListAsync();
            // var data = await (from a in this.PlayListDbContext.PlayInfos
            //                   join v in this.PlayListDbContext.Votes
            //                   on a.Id equals v.PlayId into playVotes
            //                   from v in playVotes.DefaultIfEmpty()
            //                   group new{ a.Id,a.PlayName,a.Note,Count=v.CustomerId } by a
            //            into g
            //                   select new PlayInfo
            //                   {
            //                       Id = g.Key.Id,
            //                       PlayName = g.Key.PlayName,
            //                       Note = g.Key.Note,
            //                       VoteCount = g.Select(c=>c.Count).Where(c=>c>0).Count()
            //                   }).Where(x=>x.Status==CommonStatusEnum.Common)
            //                   .OrderByDescending(c => c.VoteCount).ToListAsync();

                  




            if (data.IsListNotNull())
            {
                data.ForEach(c=>{
                    c.VoteCount=this.PlayListDbContext.Votes.Count(x=>x.PlayId==c.Id);
                });
                result.Data = data;
                // 计算 支持量

            }
            else
            {
                result.Message = "Not found";
            }
            return result;
        }

        public async Task<ResponseData<PlayInfo>> GetPlayInfoByIdAsync(int id)
        {
            ResponseData<PlayInfo> result = new ResponseData<PlayInfo>();
            if (id < 1)
            {
                result.Message = "The Id must be ge 1";
            }
            else
            {
                var data = await this.PlayListDbContext.PlayInfos.FindAsync(id);
                if (data.IsNotNullOrEmpty())
                {
                    result.Data = data;
                }
                else
                {
                    result.Message = "Not found";
                }
            }
            return result;
        }

        public async Task<ResponseData<bool>> UpdatePlayInfoByInfoAsync(PlayInfo query, Manager manager)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (query.IsNullOrEmpty())
            {
                result.Message = "model must be not empty";
            }
            else if (query.PlayName.IsNullOrEmpty())
            {
                result.Message = "PlayName must be not empty";
            }
            else
            {
                if (await this.PlayListDbContext.PlayInfos.Where(c => c.PlayName == query.PlayName && c.Id != query.Id).AnyAsync())
                {
                    result.Message = "The playName has been exist";
                }
                else
                {
                    var thisPlay = await this.PlayListDbContext.PlayInfos.FindAsync(query.Id);
                    thisPlay.PlayName = query.PlayName;
                    thisPlay.Note = query.Note;
                    thisPlay.UpdateTime = DateTime.Now;
                    thisPlay.UpdateUser = manager.Id;
                    var data = this.PlayListDbContext.PlayInfos.Update(thisPlay);
                    if (data.IsNotNullOrEmpty())
                    {
                        result.Data = true;
                        this.PlayListDbContext.SaveChanges();
                    }
                    else
                    {
                        result.Message = "Failed to create data";
                    }
                }
            }
            return result;
        }
    }
}