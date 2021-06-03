using System;
using PlayList.DataAccess;
using PlayList.Service;
using PlayList.Models;
using PlayList.Custonmer.WebApi.Controllers;
using PlayList.Managerment.WebApi.Controllers;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace PlayList.UnitTest
{
    public class Environment
    {
        public PlayListDbContext playListDbContext { get; set; }
        public IManagerService managerService { get; set; }
        public ICustomerService customerService { get; set; }
        public IPlayInfoService playInfoService { get; set; }
        public IVoteService voteService { get; set; }
        public ClaimsPrincipal ManagerIdentity { get; set; }
        public ClaimsPrincipal CustomerIdentity { get; set; }
        public Environment()
        {
            // DbContextOptions<PlayListDbContext> dbContextOptions = new Mock<DbContextOptions<PlayListDbContext>>().Object;

            // this.playListDbContext = new PlayListDbContext(dbContextOptions);


            var options = new DbContextOptionsBuilder<PlayListDbContext>()
                        .UseInMemoryDatabase(databaseName: "MovieListDatabase")
                        .Options;

            // Insert seed data into the database using one instance of the context
            this.playListDbContext = new PlayListDbContext(options); 

            // var mockPlayDbContext = new Mock<PlayListDbContext>();
            // mockPlayDbContext.SetupGet(c => c.Managers)
            // .Returns(CreateDbSetMock(new List<Manager>{
            //     new Manager{
            //         Id=1,
            //         Address="陕西.宝鸡",
            //         UserName="admin",
            //         Password="123456",
            //         RealName="小明",
            //         CreateTime=DateTime.Now,
            //         CreateUser=1,
            //         UpdateTime=DateTime.Now,
            //         UpdateUser=1
            //         }
            // }).Object);
            // mockPlayDbContext.SetupGet(c => c.Customers).Returns(CreateDbSetMock(new List<Customer>()).Object);
            // mockPlayDbContext.SetupGet(c => c.Votes).Returns(CreateDbSetMock(new List<Vote>()).Object);
            // mockPlayDbContext.SetupGet(c => c.PlayInfos).Returns(CreateDbSetMock(new List<PlayInfo>()).Object);
            // //mockPlayDbContext.Setup(c => c.Managers).Returns(CreateDbSetMock(new List<Manager>()).Object);

            //this.playListDbContext = mockPlayDbContext.Object;


            this.playInfoService = new PlayInfoService(this.playListDbContext);
            this.managerService = new ManagerService(this.playListDbContext);
            this.customerService = new CustomerService(this.playListDbContext);
            this.voteService = new VoteService(this.playListDbContext);

            var mockManagerUser = new Mock<ClaimsPrincipal>();
            mockManagerUser.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim("UserName","admin"),
                new Claim("Address","陕西.宝鸡"),
                new Claim("Id","1"),
                new Claim("RealName","remi"),
                new Claim("UpdateTime",DateTime.Now.ToString()),
                new Claim("CreateTime",DateTime.Now.ToString()),
                new Claim("CreateUser","1"),
                new Claim("UpdateUser","1"),
            });
            this.ManagerIdentity = mockManagerUser.Object;

            var mockCustomer = new Mock<ClaimsPrincipal>();
            mockCustomer.SetupGet(p => p.Claims).Returns(new List<Claim>
            {
                new Claim("NickName","Remi"),
                new Claim("PhoneNumber","18521503505"),
                new Claim("Id","1"),
                new Claim("RealName","王强")
            });
            this.CustomerIdentity = mockCustomer.Object;



        }
        private static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());
            return dbSetMock;
        }


    }
}