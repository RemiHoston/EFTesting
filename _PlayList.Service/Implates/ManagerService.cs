using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PlayList.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayList.DataAccess;

namespace PlayList.Service
{
    public class ManagerService : BaseService, IManagerService
    {
        public ManagerService(PlayListDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ResponseData<bool>> CreateManagerByInfoAsync(Manager query, Manager currentUser)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (query.IsNullOrEmpty())
            {
                result.Message = "model must be not empty";
            }
            else if (query.UserName.IsNullOrEmpty())
            {
                result.Message = "model's username must be not empty";
            }
            else if (query.Password.IsNullOrEmpty())
            {
                result.Message = "model's password must be not empty";
            }
            else
            {
                if (await this.PlayListDbContext.Managers.Where(c => c.UserName == query.UserName).AnyAsync())
                {
                    result.Message = "the username has exist";
                }
                else
                {
                    try
                    {
                        query.CreateTime = DateTime.Now;
                        query.CreateUser = currentUser.Id;
                        query.UpdateTime = DateTime.Now;
                        query.UpdateUser = currentUser.Id;
                        query.Status = CommonStatusEnum.Common;
                        // do create
                        await this.PlayListDbContext.Managers.AddAsync(query);
                        this.PlayListDbContext.SaveChanges();
                        result.Data = true;
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                    }
                }
            }
            return result;
        }

        public async Task<ResponseData<bool>> DeleteManagerByIdAsync(int id, Manager currentUser)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (id < 1)
            {
                result.Message = "Id must be ge 1";
            }
            else
            {
                if (await this.PlayListDbContext.Managers.Where(c => c.Id == id&&c.Status!=CommonStatusEnum.Deleted).AnyAsync())
                {
                    var thisManager = await this.PlayListDbContext.Managers.FindAsync(id);
                    thisManager.UpdateTime = DateTime.Now;
                    thisManager.UpdateUser = currentUser.Id;
                    thisManager.Status = CommonStatusEnum.Deleted;
                    var resData = this.PlayListDbContext.Managers.Update(thisManager);
                    if (resData.IsNotNullOrEmpty())
                    {
                        result.Data = true;
                        this.PlayListDbContext.SaveChanges();
                    }
                    else
                    {
                        result.Message = "Not updated";
                    }

                }
                else
                {
                    result.Message = "Not found";
                }
            }
            return result;
        }

        public async Task<ResponseData<Manager>> GeetManagerByIdAsync(int id)
        {
            ResponseData<Manager> result = new ResponseData<Manager>();
            if (id < 1)
            {
                result.Message = "Id must be ge 1";
            }
            else
            {
                var data = await this.PlayListDbContext.Managers.FindAsync(id);
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

        public async Task<ResponseList<Manager>> GetAllManagersAsync()
        {
            ResponseList<Manager> result = new ResponseList<Manager>();
            var data = await this.PlayListDbContext.Managers.Where(c => c.Status != CommonStatusEnum.Deleted).ToListAsync();
            if (data.IsListNotNull())
            {
                result.Data = data;
            }
            else
            {
                result.Message = "Not Found";
            }
            return result;
        }

        public async Task<ResponseData<Manager>> GetManagerByPasswordAsync(string userName, string password)
        {
            ResponseData<Manager> result = new ResponseData<Manager>();
            var data = await this.PlayListDbContext.Managers.Where(c => c.UserName == userName && c.Password == password && c.Status == CommonStatusEnum.Common)
            .FirstOrDefaultAsync();
            if (data.IsNotNullOrEmpty())
            {
                result.Data = data;
            }
            else
            {
                result.Message = "Not found";
            }
            return result;
        }

        public async Task<ResponseData<bool>> UpdateManagerByInfoAsync(Manager query, Manager currentUser)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (query.IsNullOrEmpty())
            {
                result.Message = "model must be not empty";
            }
            else if (query.Id < 1)
            {
                result.Message = "model's Id must be ge 1";
            }
            else if (query.UserName.IsNullOrEmpty())
            {
                result.Message = "model's username must be not empty";
            }
            else if (query.Password.IsNullOrEmpty())
            {
                result.Message = "model's password must be not empty";
            }
            else
            {
                // query the username wheather be used
                if (await this.PlayListDbContext.Managers.Where(c => c.UserName == query.UserName && c.Id != query.Id).AnyAsync())
                {
                    result.Message = "the username has been used";
                }
                else
                {
                    //do update
                    var thisManager = await this.PlayListDbContext.Managers.FindAsync(query.Id);
                    if (thisManager.IsNotNullOrEmpty())
                    {
                        thisManager.UserName = thisManager.UserName;
                        thisManager.Password = thisManager.Password;
                        thisManager.RealName = thisManager.RealName;
                        thisManager.Address = thisManager.Address;
                        thisManager.UpdateUser = currentUser.Id;
                        thisManager.UpdateTime = DateTime.Now;
                        var data= this.PlayListDbContext.Managers.Update(thisManager);
                        if(data.IsNotNullOrEmpty())
                        {
                            result.Data=true;
                            this.PlayListDbContext.SaveChanges();
                        }
                        else
                        {
                            result.Message="Failed to update";
                        }
                    }
                    else
                    {
                        result.Message = "not found";
                    }
                }
            }
            return result;
        }
    }
}