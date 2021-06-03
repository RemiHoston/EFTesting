using System.Reflection;
using System.Reflection.Metadata;
using System;
using System.Threading.Tasks;
using Xunit;
using PlayList.Managerment.WebApi.Controllers;
using Moq;
using Microsoft.EntityFrameworkCore;
using PlayList.DataAccess;
using PlayList.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using PlayList.Models;

namespace PlayList.UnitTest
{
    public class ManagermentTest
    {
        private ManagerController controller { get; set; }
        private Environment environment { get; set; }
        public void Init()
        {
            this.environment = new Environment();
            this.controller = new ManagerController(environment.managerService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = environment.ManagerIdentity
                }
            };
        }
        [Fact]
        public async Task CreateManager()
        {
            Init();
            var result = await controller.CreateManagerByInfoAsync(new Models.Manager
            {
                Id = 2,
                UserName = "admin",
                RealName = "小红",
                Password = "123456"
            });

            // 底层初始化的时候已经有一个username为admin的用户了,所以创建不成功
            Assert.Equal(false, result.Success);
            // 这时候数据库还只是初始化的一个用户
            Assert.Equal(environment.playListDbContext.Managers.Count(), 1);

            // 添加一个新用户

            var result1 = await controller.CreateManagerByInfoAsync(new Models.Manager
            {
                Id = 2,
                UserName = "remi",
                RealName = "小红",
                Password = "123456"
            });

            Assert.Equal(true, result.Success);
            Assert.Equal(2, environment.playListDbContext.Managers.Count());
        }

        [Fact]
        public async Task UpdateManagerTest()
        {
            Init();
            Manager managerment = new Manager
            {
                Id = 2,
                UserName = "Remi",
                RealName = "王小红",
                Password = "123456",
                Address = "陕西宝鸡"
            };
            var result = await controller.UpdateManagerByInfoAsync(managerment);
            // 没有Id为2的记录
            Assert.Equal(false, result.Success);

            // managerment.Id = 1;
            // // 测试Id是否是自增的
            var _ = await controller.CreateManagerByInfoAsync(managerment);
            Assert.Equal(true, _.Success);
            Assert.Equal(2, environment.playListDbContext.Managers.Count());

            // 更新UserName 已存在
            managerment.Id = 2;
            managerment.UserName = "admin";
            var update1 = await controller.UpdateManagerByInfoAsync(managerment);
            // 名称已存在
            Assert.Equal(false, update1.Success);

            managerment.UserName = "RemiHoston";
            var udpate2 = await controller.UpdateManagerByInfoAsync(managerment);
            Assert.Equal(true, udpate2.Success);
            Assert.Equal("RemiHoston", environment.playListDbContext.Managers.Find(2).UserName);
        }
        [Fact]
        public async Task DeleteManager()
        {
            Init();
            Manager managerment = new Manager
            {
                Id = 2,
                UserName = "Remi",
                RealName = "王小红",
                Password = "123456",
                Address = "陕西宝鸡"
            };

            Manager managerment1 = new Manager
            {
                Id = 3,
                UserName = "Remi2",
                RealName = "王小红",
                Password = "123456",
                Address = "陕西宝鸡"
            };

            await this.controller.CreateManagerByInfoAsync(managerment);
            await this.controller.CreateManagerByInfoAsync(managerment1);

            var result = await this.controller.DeleteManagerByIdAsync(2);
            // 逻辑删除成功
            Assert.Equal(true, result.Success);
            var count = environment.playListDbContext.Managers.Where(c => c.Status.GetHashCode() == CommonStatusEnum.Common.GetHashCode()).Count();
            Assert.Equal(2, count);

            var result1 = await this.controller.DeleteManagerByIdAsync(2);
            // 逻辑删除失败
            Assert.Equal(false, result1.Success);
            Assert.Equal(2, environment.playListDbContext.Managers.Where(c => c.Status == CommonStatusEnum.Common).Count());

        }
        [Fact]
        public async Task GetManagerTest()
        {
            Init();
            Manager managerment = new Manager
            {
                Id = 2,
                UserName = "Remi",
                RealName = "王小红",
                Password = "123456",
                Address = "陕西宝鸡"
            };

            Manager managerment1 = new Manager
            {
                Id = 3,
                UserName = "Remi2",
                RealName = "王小红",
                Password = "123456",
                Address = "陕西宝鸡"
            };
            await this.controller.CreateManagerByInfoAsync(managerment);
            await this.controller.CreateManagerByInfoAsync(managerment1);

            var result = await controller.GetManagerByIdAsync(2);
            Assert.Equal(true, result.Success);
            Assert.Equal("Remi", result.Data.UserName);

            var result1 = await controller.GetManagerByIdAsync(1333);
            Assert.Equal(false, result1.Success);


            var results = await controller.GetAllManagersAsync();
            Assert.Equal(true, results.Success);
            Assert.Equal(3, results.Data.Count);

        }
    }
}