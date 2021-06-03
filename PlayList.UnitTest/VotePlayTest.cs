using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayList.Custonmer.WebApi.Controllers;
using PlayList.Managerment.WebApi.Controllers;
using PlayList.Models;
using Xunit;

namespace PlayList.UnitTest
{
    public class VotePlayTest
    {
        private Environment environment { get; set; }
        private CustomerController customerCtl { get; set; }
        private PlayInfoController playCtl { get; set; }
        private MPlayInfoController mPlayCtl { get; set; }

        public void Init()
        {
            this.environment = new Environment();
            this.customerCtl = new CustomerController(environment.customerService);
            customerCtl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = environment.CustomerIdentity
                }
            };
            this.playCtl = new PlayInfoController(environment.playInfoService, environment.voteService);
            this.playCtl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = environment.CustomerIdentity
                }
            };
            this.mPlayCtl = new MPlayInfoController(environment.playInfoService);
            this.mPlayCtl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = environment.ManagerIdentity
                }
            };

        }

        /*
        造数据
        春晚节目单      参与人员   节目投票  
        1. 相声        1. 王五    (1,3,7)
        2. 歌曲        2. 赵四    (2,6,4)
        3. 小品        3. 李三    (4,5,8)
        4. 话剧        4. 田二    (7,8,9)
        5. 舞蹈        5. 马一    (6,9,10) 
        6. 杂技        6. 吴忠    (4,6,10)
        7. 曲艺        7. 何洁    (3,8,9) 
        8. 魔术        8. 齐晨    (1,6)
        9. 评书        9. 胡冬    (9)
       10. 脱口秀

       场景一:
       只有一个用户投票:  9.胡冬 (9)  前三的节目只有一个 9 评书  抽奖用户: 9. 胡冬
       场景二:
       两个用户投票支持三个节目  
       用户:
       8. 齐晨  前三的节目有三个  抽重用户: 8. 齐晨  9. 胡冬
       9. 胡冬  
       场景三:
       以上用户按照规定的节目投票
       投票结果如下:
       1. 2  1,8
       2. 1  2
       3. 2  1,7
       4. 3  2,3,6
       5. 1  3
       6. 4  2,5,6,8
       7. 2  1,4
       8. 3  3,4,8
       9. 4  4,5,6,7,9
       10. 2 5,6
       复票:
       总票数:21+3==24
       节目支持顺序是:
       9. 4  4,5,7,9
       6. 4  2,5,6,8
       8. 3  3,4,8
       4. 3  2,3,6
       最后中奖用户可能是:
       [2,3,4,5,6,7,8,9] 中的任意三个     
        */
        [Fact]
        public async Task GetLotterPerson()
        {
            #region  Data

            PlayInfo p1 = new PlayInfo
            {
                Id = 1,
                PlayName = "相声",
                Note = "p"
            };
            PlayInfo p2 = new PlayInfo
            {
                Id = 2,
                PlayName = "歌曲",
                Note = "p"
            };
            PlayInfo p3 = new PlayInfo
            {
                Id = 3,
                PlayName = "小品",
                Note = "p"
            };
            PlayInfo p4 = new PlayInfo
            {
                Id = 4,
                PlayName = "话剧",
                Note = "p"
            };
            PlayInfo p5 = new PlayInfo
            {
                Id = 5,
                PlayName = "舞蹈",
                Note = "p"
            };
            PlayInfo p6 = new PlayInfo
            {
                Id = 6,
                PlayName = "杂技",
                Note = "p"
            };
            PlayInfo p7 = new PlayInfo
            {
                Id = 7,
                PlayName = "曲艺",
                Note = "p"
            };
            PlayInfo p8 = new PlayInfo
            {
                Id = 8,
                PlayName = "魔术",
                Note = "p"
            };
            PlayInfo p9 = new PlayInfo
            {
                Id = 9,
                PlayName = "评书",
                Note = "p"
            };
            PlayInfo p10 = new PlayInfo
            {
                Id = 10,
                PlayName = "脱口秀",
                Note = "p"
            };

            // customers
            Customer c1 = new Customer { Id = 1, RealName = "王五",PhoneNumber="13333333331" };
            Customer c2 = new Customer { Id = 2, RealName = "赵六" ,PhoneNumber="13333333332"};
            Customer c3 = new Customer { Id = 3, RealName = "李三" ,PhoneNumber="13333333333"};
            Customer c4 = new Customer { Id = 4, RealName = "田二" ,PhoneNumber="13333333334"};
            Customer c5 = new Customer { Id = 5, RealName = "马一" ,PhoneNumber="13333333335"};
            Customer c6 = new Customer { Id = 6, RealName = "吴忠" ,PhoneNumber="13333333336"};
            Customer c7 = new Customer { Id = 7, RealName = "何洁" ,PhoneNumber="13333333337"};
            Customer c8 = new Customer { Id = 8, RealName = "齐晨" ,PhoneNumber="13333333338"};
            Customer c9 = new Customer { Id = 9, RealName = "胡冬" ,PhoneNumber="13333333339"};

            // votes
            Vote v11 = new Vote { Id = 1, CustomerId = 1, PlayId = 1 };
            Vote v12 = new Vote { Id = 2, CustomerId = 1, PlayId = 7 };
            Vote v13 = new Vote { Id = 3, CustomerId = 1, PlayId = 3 };
            Vote v21 = new Vote { Id = 4, CustomerId = 2, PlayId = 2 };
            Vote v22 = new Vote { Id = 5, CustomerId = 2, PlayId = 4 };
            Vote v23 = new Vote { Id = 6, CustomerId = 2, PlayId = 6 };
            Vote v31 = new Vote { Id = 7, CustomerId = 3, PlayId = 4 };
            Vote v32 = new Vote { Id = 8, CustomerId = 3, PlayId = 5 };
            Vote v33 = new Vote { Id = 9, CustomerId = 3, PlayId = 8 };
            Vote v41 = new Vote { Id = 10, CustomerId = 4, PlayId = 7 };
            Vote v42 = new Vote { Id = 11, CustomerId = 4, PlayId = 8 };
            Vote v43 = new Vote { Id = 12, CustomerId = 4, PlayId = 9 };
            Vote v51 = new Vote { Id = 13, CustomerId = 5, PlayId = 6 };
            Vote v52 = new Vote { Id = 14, CustomerId = 5, PlayId = 9 };
            Vote v53 = new Vote { Id = 15, CustomerId = 5, PlayId = 10 };
            Vote v61 = new Vote { Id = 16, CustomerId = 6, PlayId = 4 };
            Vote v62 = new Vote { Id = 17, CustomerId = 6, PlayId = 6 };
            Vote v63 = new Vote { Id = 18, CustomerId = 6, PlayId = 10 };
            Vote v71 = new Vote { Id = 19, CustomerId = 7, PlayId = 3 };
            Vote v72 = new Vote { Id = 20, CustomerId = 7, PlayId = 8 };
            Vote v73 = new Vote { Id = 21, CustomerId = 7, PlayId = 9 };
            Vote v81 = new Vote { Id = 22, CustomerId = 8, PlayId = 1 };
            Vote v82 = new Vote { Id = 23, CustomerId = 8, PlayId = 6 };
            Vote v9 = new Vote { Id = 24, CustomerId = 9, PlayId = 9 };
            #endregion

            this.Init();
            #region Play
            var rp1 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p1);
            var rp2 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p2);
            var rp3 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p3);
            var rp4 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p4);
            var rp5 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p5);
            var rp6 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p6);
            var rp7 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p7);
            var rp8 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p8);
            var rp9 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p9);
            var rp10 = await this.mPlayCtl.CreatePlayInfoByInfoAsync(p10);
            #endregion

            #region Customer
            var rc1 = await this.customerCtl.CreateCustomerByInfoAsync(c1);
            var rc2 = await this.customerCtl.CreateCustomerByInfoAsync(c2);
            var rc3 = await this.customerCtl.CreateCustomerByInfoAsync(c3);
            var rc4 = await this.customerCtl.CreateCustomerByInfoAsync(c4);
            var rc5 = await this.customerCtl.CreateCustomerByInfoAsync(c5);
            var rc6 = await this.customerCtl.CreateCustomerByInfoAsync(c6);
            var rc7 = await this.customerCtl.CreateCustomerByInfoAsync(c7);
            var rc8 = await this.customerCtl.CreateCustomerByInfoAsync(c8);
            var rc9 = await this.customerCtl.CreateCustomerByInfoAsync(c9);
            #endregion
            // Case 1
            var rvc0 = await this.playCtl.TestVotePlayAsync(v9);
            var dr0 = await this.mPlayCtl.DrawALotteryAsync();
            Assert.Equal(true, dr0.Success);
            Assert.Equal(9, dr0.Data[0].Id);

            // Case 2
            await this.playCtl.TestVotePlayAsync(v81);
            await this.playCtl.TestVotePlayAsync(v82);
            var dr01 = await this.mPlayCtl.DrawALotteryAsync();

            Assert.Equal(true, dr01.Success);
            Assert.Equal(2, dr01.Data.Count);
            Assert.Contains(dr01.Data[0].Id, new List<int> { 8, 9 });
            Assert.Contains(dr01.Data[1].Id, new List<int> { 8, 9 });

            // Case3
            await this.playCtl.TestVotePlayAsync(v11);
            await this.playCtl.TestVotePlayAsync(v12);
            await this.playCtl.TestVotePlayAsync(v13);
            await this.playCtl.TestVotePlayAsync(v21);
            await this.playCtl.TestVotePlayAsync(v22);
            await this.playCtl.TestVotePlayAsync(v23);
            await this.playCtl.TestVotePlayAsync(v31);
            await this.playCtl.TestVotePlayAsync(v32);
            await this.playCtl.TestVotePlayAsync(v33);
            await this.playCtl.TestVotePlayAsync(v41);
            await this.playCtl.TestVotePlayAsync(v42);
            await this.playCtl.TestVotePlayAsync(v43);
            await this.playCtl.TestVotePlayAsync(v51);
            await this.playCtl.TestVotePlayAsync(v52);
            await this.playCtl.TestVotePlayAsync(v53);
            await this.playCtl.TestVotePlayAsync(v61);
            await this.playCtl.TestVotePlayAsync(v62);
            await this.playCtl.TestVotePlayAsync(v63);
            await this.playCtl.TestVotePlayAsync(v71);
            await this.playCtl.TestVotePlayAsync(v72);
            await this.playCtl.TestVotePlayAsync(v73);

            var dr02 = await this.mPlayCtl.DrawALotteryAsync();

            Assert.Equal(true, dr02.Success);
            Assert.Equal(3, dr02.Data.Count);
            Assert.Contains(dr02.Data[0].Id, new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 });
            Assert.Contains(dr02.Data[1].Id, new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 });
            Assert.Contains(dr02.Data[2].Id, new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 });


        }
    }
}