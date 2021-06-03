using System;
using System.Linq;
using PlayList.DataAccess;
using PlayList.Models;

namespace PlayList.Service
{
    public class BaseService
    {
        protected PlayListDbContext PlayListDbContext{get;set;}
        public BaseService()
        {
           
        }
        public BaseService(PlayListDbContext dbContext)
        {
            this.PlayListDbContext=dbContext;
             //Init Manager Table Data,Make sure have a super user in the system. 
            if(!this.PlayListDbContext.Managers.AsQueryable().Any())
            {
                this.PlayListDbContext.Managers.Add(new Manager{
                   UserName="admin",
                   Password="123456",
                   Address="xi'an of shannxi",
                   CreateTime=DateTime.Now,
                   UpdateTime=DateTime.Now,
                   Status=CommonStatusEnum.Common,
                   CreateUser=0,
                   UpdateUser=0,
                   RealName="王小红"
                });
                this.PlayListDbContext.SaveChanges();
            }
        }
    }
}