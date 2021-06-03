using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayList.DataAccess;
using PlayList.Models;

namespace PlayList.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(PlayListDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ResponseData<bool>> CreateCustomerByInfoAsync(Customer query)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (query.IsNullOrEmpty())
            {
                result.Message = "query must be not empty";
            }
            else if (query.PhoneNumber.IsNullOrEmpty())
            {
                result.Message = "phoneNumber must be not empty";
            }
            else
            {
                // wether the pone  existed;
                if (await this.PlayListDbContext.Customers.Where(c => c.PhoneNumber == query.PhoneNumber
                 && c.Status != CommonStatusEnum.Deleted).AnyAsync())
                {
                    result.Message = "this phonenumber has been existed";
                }
                else
                {
                    query.CreateTime = DateTime.Now;
                    query.CreateUser = 0;
                    query.Status = CommonStatusEnum.Common;
                    query.UpdateTime = DateTime.Now;
                    query.UpdateUser = 0;
                    //do the create action
                    var data = this.PlayListDbContext.Customers.AddAsync(query);
                    if (data.IsNotNullOrEmpty())
                    {
                        result.Data = true;
                        this.PlayListDbContext.SaveChanges();
                    }
                    else
                    {
                        result.Message = "the customer has not been created";
                    }
                }
            }
            return result;
        }

        public async Task<ResponseData<bool>> DeleteCustomerByIdAsync(int id)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (id < 1)
            {
                result.Message = "The customer's id must be ge 1";
            }
            else
            {
                if (await this.PlayListDbContext.Customers.Where(c => c.Id == id
                 && c.Status != CommonStatusEnum.Deleted).AnyAsync())
                {
                    // find the customer 
                    var thisCustomer = await this.PlayListDbContext.Customers.FindAsync(id);
                    thisCustomer.Status = CommonStatusEnum.Deleted;
                    thisCustomer.UpdateUser = id;
                    thisCustomer.UpdateTime = DateTime.Now;
                    var data = this.PlayListDbContext.Customers.Update(thisCustomer);
                    if (data.IsNotNullOrEmpty())
                    {
                        result.Data = true;
                        this.PlayListDbContext.SaveChanges();
                    }
                    else
                    {
                        result.Message = "This customer has not been updated";
                    }
                }
                else
                {
                    result.Message = "the customer is not existed";
                }
            }
            return result;
        }

        public async Task<ResponseList<Customer>> GetAllCustomerAsync()
        {
            ResponseList<Customer> result = new ResponseList<Customer>();
            var data = await this.PlayListDbContext.Customers
            .Where(c => c.Status != CommonStatusEnum.Deleted).ToListAsync();
            if (data.IsListNotNull())
            {
                result.Data = data;
            }
            else
            {
                result.Message = "there is no data";
            }

            return result;
        }

        public async Task<ResponseData<Customer>> GetCustomerByIdAsync(int id)
        {
            ResponseData<Customer> result = new ResponseData<Customer>();
            if (id < 1)
            {
                result.Message = "id must be ge 1";
            }
            else
            {
                var data = await this.PlayListDbContext.Customers.FindAsync(id);
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

        public async Task<ResponseData<Customer>> GetCustomerByPhoneNumberAsync(string password)
        {
            ResponseData<Customer> result = new ResponseData<Customer>();
            var data = await this.PlayListDbContext.Customers
             .Where(c => c.PhoneNumber == password && c.Status == CommonStatusEnum.Common)
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

        public async Task<ResponseData<bool>> UpdateCustomerByInfoAsync(Customer query)
        {
            ResponseData<bool> result = new ResponseData<bool>();
            if (query.IsNullOrEmpty())
            {
                result.Message = "query must be not empty";
            }
            else if (query.PhoneNumber.IsNullOrEmpty())
            {
                result.Message = "phoneNumber must be not empty";
            }
            else
            {
                // wether the pone  existed;
                if (await this.PlayListDbContext.Customers.Where(c => c.PhoneNumber == query.PhoneNumber
                 && c.Status != CommonStatusEnum.Deleted && c.Id != query.Id).AnyAsync())
                {
                    result.Message = "this phonenumber has been existed";
                }
                else
                {

                    // find this customer
                    var thisCustomer = await this.PlayListDbContext.Customers.FindAsync(query.Id);
                    if (thisCustomer.IsNotNullOrEmpty())
                    {
                        thisCustomer.UpdateTime = DateTime.Now;
                        thisCustomer.UpdateUser = query.Id;
                        thisCustomer.PhoneNumber = query.PhoneNumber;
                        thisCustomer.RealName = query.RealName;
                        thisCustomer.NickName = query.NickName;
                        var data = this.PlayListDbContext.Customers.Update(thisCustomer);
                        if (data.IsNotNullOrEmpty())
                        {
                            result.Data = true;
                            this.PlayListDbContext.SaveChanges();
                        }
                        else
                        {
                            result.Message = "this custome has not been updated";
                        } 
                    }
                    else
                    {
                        result.Message="Not found";
                    } 
                    // query.CreateTime=DateTime.Now;
                    // query.CreateUser=0;
                    // query.Status=CommonStatusEnum.Common;
                    // query.UpdateTime=DateTime.Now;
                    // query.UpdateUser=0;
                    // //do the create action
                    // var data= this.PlayListDbContext.Customers.AddAsync(query);
                    // if(data.IsNotNullOrEmpty())
                    // {
                    //     result.Data=true;
                    // }
                    // else
                    // {
                    //     result.Message="the customer has not been created";
                    // }
                }
            }
            return result;
        }
    }
}