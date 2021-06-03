using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using PlayList.Models;
using PlayList.Service;

namespace PlayList.IdentityServer.Validator
{
    public class ManagerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IManagerService managerService { get; set; }
        private ICustomerService customerService { get; set; }
        public ManagerPasswordValidator(IManagerService managerService, ICustomerService customerService)
        {
            this.managerService = managerService;
            this.customerService = customerService;
        }

        //public const string GrantType = "grant_managerment";
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.Request.Raw.Get("client_id") == "ManagermentClient")
            {
                await ManagermentPasswordValidateAsync(context);
            }
            else if (context.Request.Raw.Get("client_id") == "CustomerClient")
            {

            }
        }
        private async Task ManagermentPasswordValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            string userName = context.UserName;
            string password = context.Password;
            if (userName.IsNotNullOrEmpty() && password.IsNotNullOrEmpty())
            {
                ResponseData<Manager> thisManager = await managerService.GetManagerByPasswordAsync(userName, password);
                if (thisManager.Success)
                {
                    context.Result = new GrantValidationResult(subject: "UserInfo", authenticationMethod: "password", claims: GetManagerClaims(thisManager.Data));
                }
                else
                {
                    context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "UserName or Password is invalid");
                }
            }
            else
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "UserName or Password must be not Empty");
            }

        }
        private async Task CustomerPasswordValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

            string userName = context.UserName;
            string password = context.Password;

            ResponseData<Customer> thisCustomer = await this.customerService.GetCustomerByPhoneNumberAsync(password);
            if (password.IsNotNullOrEmpty())
            {
                if (thisCustomer.Success)
                {
                    context.Result = new GrantValidationResult(subject: "CustomerInfo", authenticationMethod: "password", claims: GetCustomerClaims(thisCustomer.Data));
                }
                else
                {
                    context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "UserName or Password is invalid");
                }
            }
            else
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "PhoneNumber must be not empty");
            }
        }
        public Claim[] GetManagerClaims(Manager thisManager)
        {
            return new Claim[]{
                new Claim("UserName",thisManager.UserName),
                new Claim("Address",thisManager.Address),
                new Claim("Id",thisManager.Id.ToString()),
                new Claim("RealName",thisManager.RealName),
                new Claim("UpdateTime",thisManager.UpdateTime.ToString()),
                new Claim("CreateTime",thisManager.CreateTime.ToString()),
                new Claim("CreateUser",thisManager.CreateUser.ToString()),
                new Claim("UpdateUser",thisManager.UpdateUser.ToString()),
            };
        }
        public Claim[] GetCustomerClaims(Customer thisCustomer)
        {
            return new Claim[]{
                new Claim("NickName",thisCustomer.NickName),
                new Claim("PhoneNumber",thisCustomer.PhoneNumber),
                new Claim("Id",thisCustomer.Id.ToString()),
                new Claim("RealName",thisCustomer.RealName)
            };

        }
    }
}